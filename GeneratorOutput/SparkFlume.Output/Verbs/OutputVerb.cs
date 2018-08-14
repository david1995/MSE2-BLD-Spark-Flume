using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Targets;
using SparkFlume.Output.Business;
using SparkFlume.Output.Entities;

namespace SparkFlume.Output.Verbs
{
    public class OutputVerb
    {
        public const string DefaultConfigFile = "config.json";

        private readonly AutoResetEvent _autoReset = new AutoResetEvent(true);
        private readonly StringFormatter _stringFormatter = new StringFormatter();

        public async Task<int> Execute(OutputParameters parameters)
        {
            string configFile = parameters.ConfigFilePath ?? DefaultConfigFile;
            string configFileContent = File.ReadAllText(configFile);

            var configuration = JsonConvert.DeserializeObject<Configuration>(configFileContent);
            string databaseServer = parameters.DatabaseServer ?? configuration.DatabaseServer;
            string databaseName = parameters.DatabaseName ?? configuration.DatabaseName;
            string username = parameters.Username ?? configuration.Username;
            string password = parameters.Password ?? configuration.Password;
            int checkInterval = parameters.CheckInterval ?? configuration.CheckInterval;
            int topAmount = parameters.TopAmount ?? configuration.TopAmount;
            int minutesToInclude = parameters.MinutesToInclude ?? configuration.MinutesToInclude;
            int secondsToWait = parameters.SecondsToWait ?? configuration.SecondsToWait;

            Console.TreatControlCAsInput = true;

            var cancellationTokenSource = new CancellationTokenSource();
            
            var logConfiguration = new LoggingConfiguration();

            var now = DateTime.Now;
            var logfile = new FileTarget { FileName = $"generated-{now:yyyyMMddHHmmss}.log" };
            var logconsole = new ConsoleTarget();

            logConfiguration.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            logConfiguration.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            LogManager.Configuration = logConfiguration;

            var logger = LogManager.GetCurrentClassLogger();

            logger.Log(LogLevel.Info, $"Waiting {secondsToWait}s for database startup...");

            await Task.Delay(TimeSpan.FromSeconds(secondsToWait));

            logger.Log(LogLevel.Info, "To quit the application, press [CTRL]+[C]");
            logger.Log(LogLevel.Info, $"Connecting to {databaseServer}; Database name {databaseName}; User: {username}");

            using (var dbcontext = new ProductDbContext(databaseServer, databaseName, username, password))
            {
                dbcontext.Database.Migrate();
                var timeoutLoopTask = TimeoutLoop(checkInterval, cancellationTokenSource);
                var fetchLoopTask = FetchLoop(topAmount, TimeSpan.FromMilliseconds(minutesToInclude), dbcontext, cancellationTokenSource);
                var inputLoopTask = InputLoopAsync(cancellationTokenSource);
                
                Task.WaitAll(timeoutLoopTask, fetchLoopTask, inputLoopTask);
            }


            return 0;
        }

        private Task InputLoopAsync(CancellationTokenSource ct)
        {
            var logger = LogManager.GetCurrentClassLogger();

            if (!Console.IsInputRedirected)
            {
                ConsoleKeyInfo consoleKeyInfo;

                do
                {
                    consoleKeyInfo = Console.ReadKey();
                }
                while ((consoleKeyInfo.Modifiers & ConsoleModifiers.Control) != ConsoleModifiers.Control
                       && consoleKeyInfo.Key != ConsoleKey.C);
            }
            else
            {
                Console.ReadLine();
            }

            logger.Log(LogLevel.Info, "Cancelling...");
            ct.Cancel();
            return Task.CompletedTask;
        }

        private async Task FetchLoop(int amountProducts, TimeSpan durationToInclude, ProductDbContext dbContext, CancellationTokenSource ct)
        {
            while (!ct.IsCancellationRequested)
            {
                var currentResult = await GetNextProducts(amountProducts, durationToInclude, dbContext);
                
                _stringFormatter.PrintResults(Console.Out, currentResult);
            }
        }

        private async Task TimeoutLoop(int timeout, CancellationTokenSource ct)
        {
            while (!ct.IsCancellationRequested)
            {
                await Task.Delay(timeout);
                _autoReset.Set();
            }
        }

        private async Task<IEnumerable<ProductSum>> GetNextProducts(int amountProducts, TimeSpan durationToInclude, ProductDbContext dbContext)
        {
            await Task.Run(() => _autoReset.WaitOne());

            var now = DateTime.Now;
            var to = now - durationToInclude;

            var groupings = from p in dbContext.Products.AsEnumerable()
                            where p.Minute >= to
                            group p by p.Id;

            var groupSums = from g in groupings
                            let totalPurchases = g.Sum(p => p.Purchases)
                            let totalViews = g.Sum(p => p.Views)
                            let totalRevenue = g.Sum(p => p.Revenue)
                            orderby totalRevenue descending
                            select new ProductSum(g.Key, totalViews, totalPurchases, totalRevenue);

            return groupSums.Take(amountProducts).ToArray();
        }
    }
}

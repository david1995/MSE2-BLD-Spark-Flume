using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using NLog;
using NLog.Config;
using NLog.Targets;
using SparkFlume.Common.Business;
using SparkFlume.EventGenerator.Entities;
using SparkFlume.EventGenerator.Logic;

namespace SparkFlume.EventGenerator.Verbs
{
    public class GenerateVerb
    {
        private readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        public Task<int> Execute(GenerateParameters parameters)
        {
            var mapperConfiguration = new AutoMapperConfiguration();
            Mapper.Initialize(mapperConfiguration.Configure);

            var generator = new Generator(
                new Random(),
                parameters.ProductIdMin,
                parameters.ProductIdMax,
                parameters.CustomerIdMin,
                parameters.CustomerIdMax,
                parameters.RevenueMin,
                parameters.RevenueMax);

            var config = new LoggingConfiguration();

            var now = DateTime.Now;
            var logfile = new FileTarget { FileName = $"generated-{now:yyyyMMddHHmmss}.log" };
            var logconsole = new ConsoleTarget();

            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            LogManager.Configuration = config;

            var logger = LogManager.GetCurrentClassLogger();
            var cancellationTokenSource = new CancellationTokenSource();
            Task.WaitAll(ToggleSignalStateTaskAsync(parameters, cancellationTokenSource), GeneratorTask(parameters, generator, logger, cancellationTokenSource));

            return Task.FromResult(0);
        }

        private async Task ToggleSignalStateTaskAsync(GenerateParameters parameters, CancellationTokenSource cancellationTokenSource)
        {
            var intervalTimeSpan = TimeSpan.FromMilliseconds(parameters.Interval);
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                await Task.Delay(intervalTimeSpan, cancellationTokenSource.Token);
                _autoResetEvent.Set();
            }
        }

        private async Task GeneratorTask(GenerateParameters parameters, Generator generator, ILogger logger, CancellationTokenSource cancellationTokenSource)
        {
            using (var httpClient = new HttpClient())
            {
                while (!cancellationTokenSource.IsCancellationRequested)
                {
                    _autoResetEvent.WaitOne();
                    var nextEvent = generator.GetNextEvent();
                    var nextEventDto = Mapper.Map<EventDto>(nextEvent);
                    string nextEventJson = JsonConvert.SerializeObject(nextEventDto);

                    // log
                    logger.Info($"Sending event \"{nextEventJson}\"");

                    if (parameters.Demo)
                    {
                        continue;
                    }

                    // send to flume
                    var targetUri = nextEvent is PurchaseEvent _
                        ? parameters.PurchasesTarget
                        : nextEvent is ViewEvent _
                            ? parameters.ViewsTarget
                            : throw new NotSupportedException();

                    var nextEventBytes = Encoding.UTF8.GetBytes(nextEventJson);

                    var httpContent = new ByteArrayContent(nextEventBytes)
                    {
                        Headers =
                        {
                            ContentEncoding = { "utf-8" },
                            ContentType = MediaTypeHeaderValue.Parse("application/json")
                        }
                    };

                    var response = await httpClient.PostAsync(targetUri, httpContent);

                    logger.Info($"Result: {response.StatusCode} ({(int)response.StatusCode})");
                }
            }
        }
    }
}

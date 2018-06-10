using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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
        public async Task<int> Execute(GenerateParameters parameters)
        {
            var mapperConfiguration = new AutoMapperConfiguration();
            Mapper.Initialize(mapperConfiguration.Configure);

            var generator = new Generator(
                new Random(),
                TimeSpan.FromMilliseconds(parameters.Interval),
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
            using (var httpClient = new HttpClient())
            {
                while (true)
                {
                    var nextEvent = await generator.GetNextEvent();
                    var nextEventDto = Mapper.Map<EventDto>(nextEvent);
                    string nextEventJson = JsonConvert.SerializeObject(nextEventDto);

                    // log
                    logger.Info($"Sending event \"{nextEventJson}\"");

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

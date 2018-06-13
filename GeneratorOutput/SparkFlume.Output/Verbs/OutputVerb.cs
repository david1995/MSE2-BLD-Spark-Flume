using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SparkFlume.Output.Business;
using SparkFlume.Output.Entities;

namespace SparkFlume.Output.Verbs
{
    public class OutputVerb
    {
        private readonly AutoResetEvent _autoReset = new AutoResetEvent(true);

        public Task<int> Execute(OutputParameters parameters)
        {
            Console.WriteLine("To quit the application, press [ENTER]");

            using (var dbcontext = new ProductDbContext(parameters.DatabaseServer, parameters.DatabaseName))
            {
                var cancellationTokenSource = new CancellationTokenSource();

                var timeoutLoopTask = TimeoutLoop(parameters.CheckInterval, cancellationTokenSource);
                var fetchLoopTask = FetchLoop(parameters.TopAmount, TimeSpan.FromMilliseconds(parameters.MinutesToInclude), dbcontext, cancellationTokenSource);

                Console.ReadLine();

                Console.WriteLine("Cancelling...");
                cancellationTokenSource.Cancel();

                Task.WaitAll(timeoutLoopTask, fetchLoopTask);
            }

            Console.WriteLine("Canceled. Goodbye");

            return Task.FromResult(0);
        }

        private void PrintResults(TextWriter output, IEnumerable<ProductSum> results)
            => PrintResults(output, results is ProductSum[] resultsArray ? resultsArray : results.ToArray());

        private void PrintResults(TextWriter output, params ProductSum[] results)
        {
            string[] titles = { "Product ID", "Views", "Purchases", "Revenue" };
            var valueStrings = results.Select(
                                          r => new[]
                                          {
                                              $"{r.Id}",
                                              $"{r.Views}",
                                              $"{r.Purchases}",
                                              $"{r.Revenue}"
                                          })
                                      .ToArray();

            var allStrings = new[] { titles }.Concat(valueStrings).ToArray();
            var maxPerColumn = allStrings.Aggregate(new int[titles.Length], (acc, c) =>
            {
                var replacementMatrix = acc.Zip(c, (max, current) => (Max: max, CurrentLength: current.Length))
                                           .Select(t => t.Max < t.CurrentLength)
                                           .ToArray();

                for (int n = 0; n < replacementMatrix.Length; n++)
                {
                    if (replacementMatrix[n])
                    {
                        acc[n] = c[n].Length;
                    }
                }

                return acc;
            });

            void WriteString(string value, int colWidth, char paddingChar = ' ')
            {
                string stringToWrite = $"|{paddingChar}{value.PadRight(colWidth, paddingChar)}{paddingChar}";
                output.Write(stringToWrite);
            }

            void WriteTableLine()
            {
                foreach (int col in maxPerColumn)
                {
                    WriteString(string.Empty, col, '-');
                }

                output.WriteLine("|");
            }

            WriteTableLine();

            for (int n = 0; n < titles.Length; n++)
            {
                WriteString(titles[n], maxPerColumn[n]);
            }

            output.WriteLine("|");

            WriteTableLine();

            foreach (var row in valueStrings)
            {
                for (int x = 0; x < titles.Length; x++)
                {
                    WriteString(row[x], maxPerColumn[x]);
                }

                output.WriteLine("|");
            }

            WriteTableLine();
            output.WriteLine();
        }

        private async Task FetchLoop(int amountProducts, TimeSpan durationToInclude, ProductDbContext dbContext, CancellationTokenSource ct)
        {
            while (!ct.IsCancellationRequested)
            {
                var currentResult = await GetNextProducts(amountProducts, durationToInclude, dbContext);

                PrintResults(Console.Out, currentResult);
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
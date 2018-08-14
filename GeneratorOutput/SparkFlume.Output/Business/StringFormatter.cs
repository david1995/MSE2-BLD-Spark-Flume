using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SparkFlume.Output.Business
{
    public class StringFormatter
    {
        public void PrintResults(TextWriter output, IEnumerable<ProductSum> results)
            => PrintResults(output, results is ProductSum[] resultsArray ? resultsArray : results.ToArray());

        public void PrintResults(TextWriter output, params ProductSum[] results)
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
    }
}

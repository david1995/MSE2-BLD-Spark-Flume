using System.Net;
using CommandLine;

namespace SparkFlume.Output.Verbs
{
    [Verb("output", HelpText = "Reads data from a database.")]
    public class OutputParameters
    {
        public OutputParameters(IPEndPoint database)
        {
            Database = database;
        }

        [Option(Required = true)]
        public IPEndPoint Database { get; }
    }
}

using CommandLine;

namespace SparkFlume.Output.Verbs
{
    [Verb("output", HelpText = "Reads data from a database.")]
    public class OutputParameters
    {
        public OutputParameters(string databaseServer, string databaseName, int checkInterval = 5000, int topAmount = 10, int minutesToInclude = 5)
        {
            DatabaseServer = databaseServer;
            DatabaseName = databaseName;
            CheckInterval = checkInterval;
            TopAmount = topAmount;
            MinutesToInclude = minutesToInclude;
        }

        [Option(Required = true)]
        public string DatabaseServer { get; }

        [Option(Required = true)]
        public string DatabaseName { get; }

        [Option(Default = 5000)]
        public int CheckInterval { get; }

        [Option(Default = 10)]
        public int TopAmount { get; }

        [Option(Default = 5)]
        public int MinutesToInclude { get; }
    }
}

using CommandLine;

namespace SparkFlume.Output.Verbs
{
    [Verb("output", HelpText = "Reads data from a database.")]
    public class OutputParameters
    {
        public OutputParameters(
            string configFilePath = default,
            string databaseServer = default,
            string databaseName = default,
            string username = default,
            string password = default,
            int? checkInterval = default,
            int? topAmount = default,
            int? minutesToInclude = default,
            int? secondsToWait = default)
        {
            ConfigFilePath = configFilePath;
            DatabaseServer = databaseServer;
            DatabaseName = databaseName;
            Username = username;
            Password = password;
            CheckInterval = checkInterval;
            TopAmount = topAmount;
            MinutesToInclude = minutesToInclude;
            SecondsToWait = secondsToWait;
        }

        [Option]
        public string ConfigFilePath { get; }

        [Option]
        public string DatabaseServer { get; }

        [Option]
        public string DatabaseName { get; }

        [Option]
        public string Username { get; }

        [Option]
        public string Password { get; }

        [Option(Default = 5000)]
        public int? CheckInterval { get; }

        [Option(Default = 10)]
        public int? TopAmount { get; }

        [Option(Default = 5)]
        public int? MinutesToInclude { get; }

        [Option]
        public int? SecondsToWait { get; }
    }
}

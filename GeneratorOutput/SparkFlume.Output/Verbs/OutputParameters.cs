using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace SparkFlume.Output.Verbs
{
    [Verb("output", HelpText = "Reads data from a database.")]
    public class OutputParameters
    {
        public OutputParameters(Uri databaseUri)
        {
            DatabaseUri = databaseUri;
        }

        [Option(Required = true)]
        public Uri DatabaseUri { get; }
    }
}

using System;
using CommandLine;

namespace SparkFlume.EventGenerator.Verbs
{
    [Verb("generate", HelpText = HelpText)]
    public class GenerateParameters
    {
        public const string HelpText = @"Generates random data.";

        [Option('i', "interval", Default = 100u, HelpText = @"Specifies the interval in ms between generating data.")]
        public uint Interval { get; set; } = 100u;

        [Option(Default = 0, HelpText = "Specifies the minimum product ID.")]
        public int ProductIdMin { get; set; } = 0;

        [Option(Default = 100, HelpText = "Specifies the maximum product ID.")]
        public int ProductIdMax { get; set; } = 100;

        [Option(Default = 0, HelpText = "Specifies the minimum customer ID.")]
        public int CustomerIdMin { get; set; } = 0;

        [Option(Default = 100_000, HelpText = "Specifies the maximum customer ID.")]
        public int CustomerIdMax { get; set; } = 100_000;

        [Option(HelpText = "Specifies the minimum revenue.")]
        public decimal RevenueMin { get; set; } = 10M;

        [Option(HelpText = "Specifies the maximum revenue.")]
        public decimal RevenueMax { get; set; } = 1000M;

        [Option(Required = true, HelpText = @"Specifies the target Uri for purchase events.")]
        public Uri PurchasesTarget { get; set; }

        [Option(Required = true, HelpText = @"Specifies the target Uri for view events.")]
        public Uri ViewsTarget { get; set; }

        [Option(Default = false, HelpText = @"Specifies whether data should be sent to the specified IP addresses.")]
        public bool Demo { get; set; } = false;
    }
}

using System.Threading.Tasks;
using CommandLine;
using SparkFlume.EventGenerator.Verbs;

namespace SparkFlume.EventGenerator
{
    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            return await Parser.Default.ParseArguments<GenerateParameters>(args)
                               .MapResult(
                                   new GenerateVerb().Execute,
                                   err => Task.FromResult(-1));
        }
    }
}

using System.Threading.Tasks;
using CommandLine;
using SparkFlume.Output.Verbs;

namespace SparkFlume.Output
{
    internal static class Program
    {
        private static async Task<int> Main(string[] args)
        {
            return await Parser.Default.ParseArguments<OutputParameters>(args)
                               .MapResult(
                                   new OutputVerb().Execute,
                                   err => Task.FromResult(-1));
        }
    }
}

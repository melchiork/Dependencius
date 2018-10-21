using System;
using System.IO;
using System.Linq;
using CommandLine;
using Mono.Cecil;

namespace Dependencius
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(GenerateOutput);
        }

        private static void GenerateOutput(CommandLineOptions options)
        {
            var path = options.DirectoryPath ?? Directory.GetCurrentDirectory();

            var paths = Directory.GetFiles(path, options.Filter);

            var assembliesWithDependencies = paths
                .Select(ModuleDefinition.ReadModule)
                .Select(x =>
                    new AssemblyWithDependencies(x.Assembly.Name.Name, x.AssemblyReferences.Select(ar => ar.Name)));

            var graph = new AssemblyGraph(assembliesWithDependencies);

            switch (options.OutputMode)
            {
                case OutputMode.DotGraph:
                    Console.Write(graph.ToDotGraph());
                    break;
                case OutputMode.Csv:
                    Console.Write(graph.ToCountCsv());
                    break;
            }
        }
    }
}
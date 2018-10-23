using System;
using System.IO;
using CommandLine;
using Mono.Cecil;

namespace Dependencius
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var assembliesAnalyzer = new AssembliesAnalyzer(
                Console.Write,
                Console.Error.WriteLine,
                ModuleDefinition.ReadModule,
                ReadFilesFromDirectory);

            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(assembliesAnalyzer.ReadAndWriteOutput);
        }

        private static string[] ReadFilesFromDirectory(string path, string filter)
        {
            var effectivePath = path ?? Directory.GetCurrentDirectory();

            return Directory.GetFiles(effectivePath, filter);
        }
    }
}
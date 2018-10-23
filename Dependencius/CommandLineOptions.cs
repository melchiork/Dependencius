using CommandLine;

namespace Dependencius
{
    internal class CommandLineOptions
    {
        public CommandLineOptions(OutputMode outputMode, string filter, string directoryPath)
        {
            OutputMode = outputMode;
            Filter = filter;
            DirectoryPath = directoryPath;
        }

        [Option('o', "outputMode", Required = true, HelpText = "Valid values: DotGraph, CountCsv, FullCsv")]
        public OutputMode OutputMode { get; }

        [Option('f', "filter", Default = "*.dll")]
        public string Filter { get; }

        [Option('p', "directoryPath", Default = null, HelpText = "Default is current directory.")]
        public string DirectoryPath { get; }
    }
}
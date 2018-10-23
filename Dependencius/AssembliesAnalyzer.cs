using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace Dependencius
{
    internal class AssembliesAnalyzer
    {
        private readonly Action<string> _writeOutput;
        private readonly Action<string> _writeError;
        private readonly Func<string, ModuleDefinition> _readModule;
        private readonly Func<string, string, IEnumerable<string>> _getFilesFromDirectory;

        public AssembliesAnalyzer(Action<string> writeOutput,
            Action<string> writeError,
            Func<string, ModuleDefinition> readModule,
            Func<string, string, IEnumerable<string>> getFilesFromDirectory)
        {
            _writeOutput = writeOutput;
            _writeError = writeError;
            _readModule = readModule;
            _getFilesFromDirectory = getFilesFromDirectory;
        }

        public void ReadAndWriteOutput(CommandLineOptions options)
        {
            var paths = _getFilesFromDirectory(options.DirectoryPath, options.Filter);

            var assembliesWithDependencies = paths
                .Select(ReadSafely)
                .Where(x => x != null)
                .Select(x =>
                    new AssemblyWithDependencies(x.Assembly.Name.Name, x.AssemblyReferences.Select(ar => ar.Name)));

            var graph = new AssembliesGraph(assembliesWithDependencies);

            switch (options.OutputMode)
            {
                case OutputMode.DotGraph:
                    _writeOutput(graph.ToDotGraph());
                    break;
                case OutputMode.Csv:
                    _writeOutput(graph.ToCountCsv());
                    break;
            }
        }

        private ModuleDefinition ReadSafely(string path)
        {
            ModuleDefinition moduleDefinition = null;
            try
            {
                moduleDefinition = _readModule(path);
            }
            catch (Exception e)
            {
                _writeError($"Error reading assembly: {path}");
                _writeError(e.ToString());
            }

            return moduleDefinition;
        }
    }
}
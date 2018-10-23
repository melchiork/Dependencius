using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Dependencius
{
    internal class AssembliesGraph
    {
        private readonly ImmutableList<AssemblyWithDependencies> _assembliesWithDependencies;

        public AssembliesGraph(IEnumerable<AssemblyWithDependencies> assembliesWithDependencies)
        {
            _assembliesWithDependencies = assembliesWithDependencies.ToImmutableList();
        }

        public string ToDotGraph()
        {
            var allDependencies =
                _assembliesWithDependencies.SelectMany(x =>
                        x.ReferencedAssembliesNames.Select(raf => $"{x.Name} -> {raf}"))
                    .Select(x => x.Replace(".", "_"));

            var uniqueGraphEntries = new HashSet<string>(allDependencies);

            var graphBody = string.Join("\n ", uniqueGraphEntries);

            var graphText = $"digraph{{\n {graphBody}\n}}";

            return graphText;
        }

        public string ToCountCsv()
        {
            var referencesWithCount = _assembliesWithDependencies
                .SelectMany(x => x.ReferencedAssembliesNames)
                .GroupBy(x => x)
                .Select(x => new {Dependency = x.Key, Count = x.Count()})
                .OrderByDescending(x => x.Count).ThenBy(x => x.Dependency)
                .ToList();

            var csvRows = referencesWithCount
                .Select(x => $"{x.Dependency},{x.Count}");

            var csvBody = string.Join("\n", csvRows);

            var csv = $"Dependency,Count\n{csvBody}";

            return csv;
        }

        public string ToFullCsv()
        {
            var csvRows =
                _assembliesWithDependencies.SelectMany(x =>
                        x.ReferencedAssembliesNames.Select(raf => $"{raf},{x.Name}"))
                    .Distinct()
                    .OrderBy(x => x);

            var csvBody = string.Join("\n", csvRows);

            var csv = $"Dependency,ReferencedBy\n{csvBody}";

            return csv;
        }
    }
}
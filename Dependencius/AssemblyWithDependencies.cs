using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Dependencius
{
    internal class AssemblyWithDependencies
    {
        public string Name { get; }
        public ImmutableList<string> ReferencedAssembliesNames { get; }

        public AssemblyWithDependencies(string name, IEnumerable<string> referencedAssembliesNames)
        {
            Name = name;
            ReferencedAssembliesNames = referencedAssembliesNames.ToImmutableList();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine(Name);
            foreach (var referencedAssemblyName in ReferencedAssembliesNames)
                sb.AppendLine($"---->{referencedAssemblyName}");

            return sb.ToString();
        }
    }
}
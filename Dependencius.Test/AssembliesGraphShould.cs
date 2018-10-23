using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Dependencius.Test
{
    public class AssembliesGraphShould
    {
        private readonly List<AssemblyWithDependencies> _assembliesWithDependencies;

        public AssembliesGraphShould()
        {
            _assembliesWithDependencies = new List<AssemblyWithDependencies>
            {
                new AssemblyWithDependencies("A", new List<string> {"S1", "S2"}),
                new AssemblyWithDependencies("B", new List<string> {"A", "S2", "S3"})
            };
        }

        [Fact]
        public void ReturnValidDotGraph()
        {
            var expectedResult = @"digraph{
 A -> S1
 A -> S2
 B -> A
 B -> S2
 B -> S3
}".Replace("\r", "");


            var assemblyGraph = new AssembliesGraph(_assembliesWithDependencies);

            var result = assemblyGraph.ToDotGraph();

            result.Should().Be(expectedResult);
        }

        [Fact]
        public void ReturnValidCountCsv()
        {
            var expectedResult = @"Dependency,Count
S2,2
A,1
S1,1
S3,1".Replace("\r", "");


            var assemblyGraph = new AssembliesGraph(_assembliesWithDependencies);

            var result = assemblyGraph.ToCountCsv();

            result.Should().Be(expectedResult);
        }
    }
}
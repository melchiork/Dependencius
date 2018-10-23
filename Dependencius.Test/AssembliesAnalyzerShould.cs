using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Dependencius.Test
{
    public class AssembliesAnalyzerShould
    {
        [Fact]
        public void ReportExceptionWhenErrorOccuredReadingAssembly()
        {
            var errorResult = string.Empty;
            var assembliesAnalyzer = new AssembliesAnalyzer(
                x => { }, 
                x => errorResult += x, x => throw new Exception(),
                (x, y) => new List<string> {"path1"});

            assembliesAnalyzer.ReadAndWriteOutput(new CommandLineOptions(OutputMode.Csv, "*.dll", "path"));

            errorResult.Should().Contain("path1");
        }
    }
}
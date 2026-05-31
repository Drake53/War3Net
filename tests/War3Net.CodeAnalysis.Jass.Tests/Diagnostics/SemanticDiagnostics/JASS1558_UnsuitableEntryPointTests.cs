namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetUnsuitableEntryPointTests), DynamicDataSourceType.Method)]
        public void TestUnsuitableEntryPointDiagnostic(string markedCode)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.UnsuitableEntryPoint.Id,
                markedCode,
                hasCascadingErrors: false,
                filePath: "war3map.j");
        }

        private static IEnumerable<object?[]> GetUnsuitableEntryPointTests()
        {
            yield return new object[]
            {
                @"
native [|main|] takes nothing returns nothing

function config takes nothing returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
constant function [|main|] takes nothing returns nothing
endfunction

function config takes nothing returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
constant native [|main|] takes nothing returns nothing

function config takes nothing returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
native [|config|] takes nothing returns nothing

function main takes nothing returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
constant function [|config|] takes nothing returns nothing
endfunction

function main takes nothing returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
constant native [|config|] takes nothing returns nothing

function main takes nothing returns nothing
endfunction",
            };
        }
    }
}
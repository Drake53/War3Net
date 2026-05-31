namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetMissingEntryPointTests), DynamicDataSourceType.Method)]
        public void TestMissingMainReportsDiagnostic(string code, int expectedCount = 1, string fileName = "war3map.j")
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.MissingEntryPoint.Id,
                code,
                expectedCount,
                fileName);
        }

        private static IEnumerable<object?[]> GetMissingEntryPointTests()
        {
            yield return new object[]
            {
                @"
function config takes nothing returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
function DoNothing takes nothing returns nothing
endfunction",
                2,
            };

            yield return new object[]
            {
                @"
globals
    integer main = 0
endglobals

function config takes nothing returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
function config takes nothing returns nothing
endfunction

function main takes nothing returns nothing
endfunction",
                0,
            };

            yield return new object[]
            {
                @"
function DoNothing takes nothing returns nothing
endfunction",
                0,
                "library.j",
            };
        }
    }
}
namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetNothingNotAllowedTests), DynamicDataSourceType.Method)]
        public void TestNothingNotAllowedDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.NothingNotAllowed.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetNothingNotAllowedTests()
        {
            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local [|nothing|] x
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    [|nothing|] x
endglobals",
            };

            yield return new object[]
            {
                @"
function foo takes integer x, [|nothing|] y returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    [|nothing|] array x
endglobals",
            };

            yield return new object[]
            {
                @"
type mynothing extends [|nothing|]",
            };
        }
    }
}
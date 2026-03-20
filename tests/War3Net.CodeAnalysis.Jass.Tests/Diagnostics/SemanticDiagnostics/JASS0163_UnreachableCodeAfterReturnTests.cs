namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetUnreachableCodeAfterReturnTests), DynamicDataSourceType.Method)]
        public void TestUnreachableCodeAfterReturnDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.UnreachableCodeAfterReturn.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetUnreachableCodeAfterReturnTests()
        {
            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    return
    [|local integer x = 5|]
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    integer gValue = 0
endglobals

function GetInt takes nothing returns integer
    return 42
    [|set gValue = 100|]
endfunction",
            };
        }
    }
}
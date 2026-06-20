namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSyntaxDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetArrayInitializerNotAllowedTests), DynamicDataSourceType.Method)]
        public void TestArrayInitializerNotAllowedDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.ArrayInitializerNotAllowed.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetArrayInitializerNotAllowedTests()
        {
            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    local integer array x [|= 5|]
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    integer array x [|= 5|]
endglobals",
            };

            yield return new object[]
            {
                @"
globals
    string array s [|= ""hello""|]
endglobals",
            };
        }
    }
}
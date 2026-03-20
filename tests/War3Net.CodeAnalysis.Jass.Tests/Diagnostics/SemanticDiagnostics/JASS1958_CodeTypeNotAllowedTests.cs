namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetCodeTypeNotAllowedTests), DynamicDataSourceType.Method)]
        public void TestCodeTypeNotAllowedDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.CodeTypeNotAllowed.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetCodeTypeNotAllowedTests()
        {
            yield return new object[]
            {
                @"
globals
    code array [|callbacks|]
endglobals",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local code array [|callbacks|]
endfunction",
            };
        }
    }
}
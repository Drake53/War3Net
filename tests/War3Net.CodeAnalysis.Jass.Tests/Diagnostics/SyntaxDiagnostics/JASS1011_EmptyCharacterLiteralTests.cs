namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSyntaxDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetEmptyCharacterLiteralTests), DynamicDataSourceType.Method)]
        public void TestEmptyCharacterLiteralDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.EmptySingleQuotedLiteral.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetEmptyCharacterLiteralTests()
        {
            yield return new object[]
            {
                @"
globals
    integer x = [|''|]
endglobals",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    local integer x
    set x = [|''|]
endfunction",
            };
        }
    }
}
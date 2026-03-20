namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSyntaxDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetUnterminatedStringTests), DynamicDataSourceType.Method)]
        public void TestUnterminatedStringDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.UnterminatedString.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetUnterminatedStringTests()
        {
            yield return new object?[]
            {
                @"
function main takes nothing returns nothing
    call BJDebugMsg([|""Hello World)
endfunction|]",
                true,
            };

            yield return new object?[]
            {
                @"
function main takes nothing returns nothing
    call BJDebugMsg([|""Hello|]",
                true,
            };
        }
    }
}
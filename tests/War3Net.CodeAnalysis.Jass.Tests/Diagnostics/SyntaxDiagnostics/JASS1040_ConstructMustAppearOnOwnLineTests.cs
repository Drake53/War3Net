namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSyntaxDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetConstructMustAppearOnOwnLineTests), DynamicDataSourceType.Method)]
        public void TestConstructMustAppearOnOwnLineDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.ConstructMustAppearOnOwnLine.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetConstructMustAppearOnOwnLineTests()
        {
            yield return new object?[]
            {
                @"
function Foo takes nothing returns nothing
    call
    [|Foo|]
    [|(|]
    [|)|]
endfunction",
            };
        }
    }
}
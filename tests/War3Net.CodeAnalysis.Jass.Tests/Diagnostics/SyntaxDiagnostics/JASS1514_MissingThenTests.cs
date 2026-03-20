namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSyntaxDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetMissingThenTests), DynamicDataSourceType.Method)]
        public void TestMissingThenDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.MissingThen.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetMissingThenTests()
        {
            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    if true
        [|call|] Bar()
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    if true then
        call A()
    elseif false
        [|call|] B()
    endif
endfunction",
            };
        }
    }
}
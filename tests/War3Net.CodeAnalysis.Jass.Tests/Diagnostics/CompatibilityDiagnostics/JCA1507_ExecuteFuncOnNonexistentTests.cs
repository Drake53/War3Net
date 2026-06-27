namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassCompatibilityDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetExecuteFuncOnNonexistentTests), DynamicDataSourceType.Method)]
        public void TestExecuteFuncOnNonexistentDiagnostic(string markedCode)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassCompatibilityDiagnostics.ExecuteFuncOnNonexistent.Id,
                markedCode);
        }

        private static IEnumerable<object?[]> GetExecuteFuncOnNonexistentTests()
        {
            yield return new object[]
            {
                @"
native ExecuteFunc takes string funcName returns nothing

function main takes nothing returns nothing
    call ExecuteFunc([|""NonexistentFunction""|])
endfunction",
            };

            yield return new object[]
            {
                @"
native ExecuteFunc takes string funcName returns nothing

function main takes nothing returns nothing
    call ExecuteFunc([|""Non"" + ""existent""|])
endfunction",
            };
        }
    }
}
namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    [TestClass]
    public partial class JassCodeQualityDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetDivisionByZeroTests), DynamicDataSourceType.Method)]
        public void TestDivisionByZeroDiagnostic(string markedCode)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassCodeQualityDiagnostics.DivisionByZero.Id,
                markedCode);
        }

        private static IEnumerable<object?[]> GetDivisionByZeroTests()
        {
            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x = [|10 / 0|]
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    constant integer x = 5
endglobals

function main takes nothing returns nothing
    local integer x = [|10 / (x - 5)|]
endfunction",
            };
        }
    }
}
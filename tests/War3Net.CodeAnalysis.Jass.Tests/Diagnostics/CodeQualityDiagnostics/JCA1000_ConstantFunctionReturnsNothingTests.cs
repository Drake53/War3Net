namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassCodeQualityDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetConstantFunctionReturnsNothingTests), DynamicDataSourceType.Method)]
        public void TestConstantFunctionReturnsNothingDiagnostic(string markedCode)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassCodeQualityDiagnostics.ConstantFunctionReturnsNothing.Id,
                markedCode);
        }

        private static IEnumerable<object?[]> GetConstantFunctionReturnsNothingTests()
        {
            yield return new object[]
            {
                @"
constant function [|Foo|] takes nothing returns nothing
    local integer x = 5
    set x = x + 1
endfunction",
            };
        }
    }
}
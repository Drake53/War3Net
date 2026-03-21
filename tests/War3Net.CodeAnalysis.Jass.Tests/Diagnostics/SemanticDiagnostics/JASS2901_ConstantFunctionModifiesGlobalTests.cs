namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetConstantFunctionModifiesGlobalTests), DynamicDataSourceType.Method)]
        public void TestConstantFunctionModifiesGlobalDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.ConstantFunctionModifiesGlobal.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetConstantFunctionModifiesGlobalTests()
        {
            yield return new object[]
            {
                @"
globals
    integer gValue = 0
endglobals

constant function Modify takes nothing returns integer
    set [|gValue|] = 5
    return gValue
endfunction",
            };
        }
    }
}
namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetCircularConstantDefinitionTests), DynamicDataSourceType.Method)]
        public void TestCircularConstantDefinitionDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.CircularConstantDefinition.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetCircularConstantDefinitionTests()
        {
            yield return new object[]
            {
                @"
globals
    constant integer A = [|A|]
endglobals",
            };

            yield return new object[]
            {
                @"
globals
    constant integer A = [|A|] + 1
endglobals",
            };
        }
    }
}
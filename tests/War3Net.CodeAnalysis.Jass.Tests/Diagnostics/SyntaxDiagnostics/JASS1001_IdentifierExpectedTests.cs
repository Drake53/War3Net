namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSyntaxDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetIdentifierExpectedTests), DynamicDataSourceType.Method)]
        public void TestIdentifierExpectedDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.IdentifierExpected.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetIdentifierExpectedTests()
        {
            yield return new object[]
            {
                @"function [|123|] takes nothing returns nothing
                  endfunction",
                true,
            };
        }
    }
}
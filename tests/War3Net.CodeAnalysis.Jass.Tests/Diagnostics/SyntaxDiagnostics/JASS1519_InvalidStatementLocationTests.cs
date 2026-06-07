namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSyntaxDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetInvalidStatementLocationTests), DynamicDataSourceType.Method)]
        public void TestInvalidStatementLocationDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.InvalidStatementLocation.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetInvalidStatementLocationTests()
        {
            yield return new object[]
            {
                "[|call|] DoSomething()",
            };

            yield return new object[]
            {
                @"
globals
    integer x
endglobals
[|set|] x = 5",
            };

            yield return new object?[]
            {
                @"
globals
    integer x = 5
    [|set|] x = 10
endglobals",
                true,
            };
        }
    }
}
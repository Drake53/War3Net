namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSyntaxDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetMissingExpressionAtEndTests), DynamicDataSourceType.Method)]
        public void TestMissingExpressionAtEndDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.MissingExpressionAtEnd.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetMissingExpressionAtEndTests()
        {
            yield return new object?[]
            {
                @"
function main takes nothing returns nothing
    set x [|=|]",
                true,
            };

            yield return new object?[]
            {
                @"
function main takes nothing returns nothing
    set x = 5 [|+|]",
                true,
            };

            yield return new object?[]
            {
                @"
function main takes nothing returns nothing
    loop
        [|exitwhen|]",
                true,
            };
        }
    }
}
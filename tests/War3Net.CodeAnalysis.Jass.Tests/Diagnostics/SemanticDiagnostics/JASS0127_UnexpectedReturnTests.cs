namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetUnexpectedReturnTests), DynamicDataSourceType.Method)]
        public void TestUnexpectedReturnDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.UnexpectedReturn.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetUnexpectedReturnTests()
        {
            yield return new object[]
            {
                @"
function DoNothing takes nothing returns nothing
    return [|42|]
endfunction",
            };

            yield return new object[]
            {
                @"
function DoNothing takes nothing returns nothing
    return [|""hello""|]
endfunction",
            };

            yield return new object[]
            {
                @"
function DoNothing takes nothing returns nothing
    return [|true|]
endfunction",
            };

            yield return new object[]
            {
                @"
function DoNothing takes nothing returns nothing
    return [|1 + 2|]
endfunction",
            };
        }
    }
}
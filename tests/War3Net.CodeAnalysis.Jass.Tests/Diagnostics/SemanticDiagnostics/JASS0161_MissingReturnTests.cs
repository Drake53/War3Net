namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetMissingReturnTests), DynamicDataSourceType.Method)]
        public void TestMissingReturnDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.MissingReturn.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetMissingReturnTests()
        {
            yield return new object[]
            {
                @"
function [|GetInt|] takes nothing returns integer
endfunction",
            };

            yield return new object[]
            {
                @"
function [|GetInt|] takes boolean b returns integer
    if b then
        return 1
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
function [|GetInt|] takes boolean b returns integer
    if b then
        // missing return here
    else
        return 2
    endif
endfunction",
            };
        }
    }
}
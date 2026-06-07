namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSyntaxDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetIdentifierExpectedKeywordTests), DynamicDataSourceType.Method)]
        public void TestIdentifierExpectedKeywordDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.IdentifierExpectedKeyword.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetIdentifierExpectedKeywordTests()
        {
            yield return new object[]
            {
                @"
function foo takes integer a, integer b, [|returns|] nothing
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes myParam [|returns|] nothing
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes integer a, b [|returns|] nothing
endfunction",
            };

            yield return new object[]
            {
                @"
function [|takes|] nothing returns nothing
endfunction",
            };

            yield return new object[]
            {
                "native [|takes|] integer id returns handle",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns
[|endfunction|]",
            };

            yield return new object[]
            {
                @"
globals
    integer x = 0x1FG2
[|endglobals|]",
                true,
            };

            yield return new object[]
            {
                @"
type [|integer|] extends handle",
                true,
            };
        }
    }
}
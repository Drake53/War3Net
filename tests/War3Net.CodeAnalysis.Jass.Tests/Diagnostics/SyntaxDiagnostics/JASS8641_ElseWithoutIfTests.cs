namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    [TestClass]
    public partial class JassSyntaxDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetElseWithoutIfTests), DynamicDataSourceType.Method)]
        public void TestElseWithoutIfDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.ElseWithoutIf.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetElseWithoutIfTests()
        {
            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    [|else|]
        call Bar()
    endif
endfunction",
                true,
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    if true then
        call A()
    endif
    [|else|]
        call B()
    endif
endfunction",
                true,
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    [|elseif|] true then
        call Bar()
    endif
endfunction",
                true,
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    if true then
        call A()
    endif
    [|elseif|] false then
        call B()
    endif
endfunction",
                true,
            };
        }
    }
}
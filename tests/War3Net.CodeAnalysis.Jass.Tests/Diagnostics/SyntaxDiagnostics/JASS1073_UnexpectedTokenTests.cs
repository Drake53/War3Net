namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSyntaxDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetUnexpectedTokenTests), DynamicDataSourceType.Method)]
        public void TestUnexpectedTokenDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.UnexpectedToken.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetUnexpectedTokenTests()
        {
            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    call Bar()
    [|endif|]
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    if true then
        call Bar()
    endif
    [|endif|]
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    call Bar()
    [|endloop|]
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    loop
        exitwhen true
    endloop
    [|endloop|]
endfunction",
            };

            yield return new object[]
            {
                "[|endfunction|]",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
endfunction
[|endfunction|]",
            };

            yield return new object[]
            {
                @"
globals
    integer x = 5
endglobals
[|endfunction|]",
            };

            yield return new object[]
            {
                @"
[|endglobals|]
function foo takes nothing returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    integer x = 5
endglobals
[|endglobals|]",
            };

            yield return new object?[]
            {
                @"
function foo takes nothing returns nothing
    [|endglobals|]
endfunction",
            };
        }
    }
}
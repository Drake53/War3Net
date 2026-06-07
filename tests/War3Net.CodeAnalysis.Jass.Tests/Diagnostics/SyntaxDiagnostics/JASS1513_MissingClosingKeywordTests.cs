namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSyntaxDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetMissingClosingKeywordTests), DynamicDataSourceType.Method)]
        public void TestMissingClosingKeywordDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.MissingClosingKeyword.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetMissingClosingKeywordTests()
        {
            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    call Bar()
[|function|] bar takes nothing returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    call Bar([|)|]",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    if true then
    [|endif|]",
            };

            yield return new object?[]
            {
                @"
function outer takes nothing returns nothing
    [|function|] inner takes nothing returns nothing
    endfunction
endfunction",
                true,
            };

            yield return new object?[]
            {
                @"
function outer takes nothing returns nothing
    [|native|] InnerNative takes nothing returns nothing
endfunction",
                true,
            };

            yield return new object[]
            {
                @"
globals
    integer x = 5
[|function|] foo takes nothing returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    integer x = 5
    string s = [|""hello""|]",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    if true then
        call Bar()
[|endfunction|]",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    if true then
        if false then
            call Bar()
    endif
[|endfunction|]",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    if true then
        call A()
    else
        call B()
[|endfunction|]",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    loop
        exitwhen true
[|endfunction|]",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    loop
        loop
            exitwhen true
    endloop
[|endfunction|]",
            };

            yield return new object?[]
            {
                @"
function foo takes nothing returns nothing
    call Bar()
[|globals|]
    integer x = 5
endglobals",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    [|globals|]
        integer x = 5
    endglobals
endfunction",
                true,
            };
        }
    }
}
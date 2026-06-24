namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    [TestClass]
    public partial class JassSyntaxDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetUnmatchedBlockKeywordTests), DynamicDataSourceType.Method)]
        public void TestUnmatchedBlockKeywordDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.UnmatchedBlockKeyword.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetUnmatchedBlockKeywordTests()
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

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    if true then
        call A()
    else
        call B()
    [|else|]
        call C()
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    if true then
        call A()
    else
        call B()
    [|elseif|] false then
        call C()
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    if true then
        call A()
    else
        call B()
    [|elseif|] false then
        call C()
    elseif true then
        call D()
    endif
endfunction",
            };

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
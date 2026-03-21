namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetLocalDeclarationAfterStatementTests), DynamicDataSourceType.Method)]
        public void TestLocalDeclarationAfterStatementDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.LocalDeclarationMustAppearFirst.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetLocalDeclarationAfterStatementTests()
        {
            yield return new object[]
            {
                @"
globals
    integer gValue = 0
endglobals

function main takes nothing returns nothing
    set gValue = 5
    [|local|] integer x = 10
endfunction",
            };

            yield return new object[]
            {
                @"
native DoSomething takes nothing returns nothing

function main takes nothing returns nothing
    call DoSomething()
    [|local|] integer x = 10
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    if true then
    endif
    [|local|] integer x = 10
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    loop
        exitwhen true
    endloop
    [|local|] integer x = 10
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    integer gValue = 0
endglobals

function main takes nothing returns nothing
    local integer a = 1
    set gValue = 5
    [|local|] integer b = 2
    [|local|] integer c = 3
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    if true then
        [|local|] integer x = 10
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    if true then
    else
        [|local|] integer x = 10
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    if false then
    elseif true then
        [|local|] integer x = 10
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    loop
        [|local|] integer x = 10
        exitwhen true
    endloop
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    loop
        if true then
            [|local|] integer x = 10
        endif
        exitwhen true
    endloop
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    if true then
        loop
            [|local|] integer x = 10
            exitwhen true
        endloop
    endif
endfunction",
            };
        }
    }
}
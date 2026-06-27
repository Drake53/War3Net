namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassStyleDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetInconsistentIndentationTests), DynamicDataSourceType.Method)]
        public void TestInconsistentIndentationDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassStyleDiagnostics.InconsistentIndentation.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetInconsistentIndentationTests()
        {
            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x = 5
[|  |]local integer y = 10
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x = 5
[|	|]local integer y = 10
endfunction",
            };

            yield return new object?[]
            {
                @"
function main takes nothing returns nothing
[|local|] integer x = 5
endfunction",
            };

            yield return new object[]
            {
                @"
native DoWork takes nothing returns nothing

function main takes nothing returns nothing
    if true then
[|      |]call DoWork()
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
native DoWork takes nothing returns nothing

function main takes nothing returns nothing
    loop
[|      |]call DoWork()
[|      |]exitwhen true
    endloop
endfunction",
            };

            yield return new object[]
            {
                @"
native DoWork takes nothing returns nothing

function main takes nothing returns nothing
    if true then
        loop
[|          |]call DoWork()
            exitwhen true
        endloop
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
[|  |]// comment indented differently
    local integer x = 5
endfunction",
            };

            yield return new object?[]
            {
                @"
function main takes nothing returns nothing
[|// comment at column 0|]
    local integer x = 5
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    if true then
[|    |]// comment should be at depth 2
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    integer udg_x
endglobals

function main takes nothing returns nothing
    debug if true then
[|      |]set udg_x = 5
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    debug loop
[|      |]exitwhen true
    endloop
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    // correctly indented comment
[|  |]// wrongly indented comment
    local integer x = 5
endfunction",
            };
        }
    }
}
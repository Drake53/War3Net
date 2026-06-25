namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetTypeMismatchTests), DynamicDataSourceType.Method)]
        public void TestTypeMismatchDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.TypeMismatch.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetTypeMismatchTests()
        {
            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local boolean b = [|5|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer i = [|""hello""|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local real r = [|true|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer i = 0
    set i = [|""hello""|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer array arr
    set arr[0] = [|""hello""|]
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    integer x = [|""hello""|]
endglobals",
            };

            yield return new object[]
            {
                @"
globals
    code c = [|null|]
endglobals",
            };

            yield return new object[]
            {
                @"
function DoNothing takes nothing returns nothing
endfunction

function main takes nothing returns nothing
    local integer x = [|DoNothing()|]
endfunction",
            };

            yield return new object[]
            {
                @"
type unit extends handle
type item extends handle

function main takes nothing returns nothing
    local unit u = null
    local item i = [|u|]
endfunction",
            };

            yield return new object[]
            {
                @"
type unit extends handle
type item extends handle

function main takes nothing returns nothing
    local unit u = null
    local item i = null
    set i = [|u|]
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns integer
    return [|""hello""|]
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns integer
    return [|true|]
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns string
    return [|42|]
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns boolean
    return [|42|]
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns integer
    return [|3.14|]
endfunction",
            };

            yield return new object[]
            {
                @"
type unit extends handle
type item extends handle

function GetUnit takes nothing returns unit
    local item i = null
    return [|i|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer i = [|null|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local real r = [|null|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local boolean b = [|null|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer i = 0
    set i = [|null|]
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    integer x = [|null|]
endglobals",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    if [|5|] then
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    if [|""hello""|] then
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    if [|3.14|] then
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes boolean b returns nothing
    if b then
    elseif [|5|] then
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    loop
        exitwhen [|5|]
    endloop
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    loop
        exitwhen [|""done""|]
    endloop
endfunction",
            };

            yield return new object[]
            {
                @"
function DoNothing takes nothing returns nothing
endfunction

function main takes nothing returns nothing
    if [|DoNothing()|] then
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer array arr
    local integer x = arr[[|""hello""|]]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer array arr
    local integer x = arr[[|3.14|]]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer array arr
    local integer x = arr[[|true|]]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer array arr
    set arr[[|""hello""|]] = 5
endfunction",
            };

            yield return new object[]
            {
                @"
function DoNothing takes nothing returns nothing
endfunction

function main takes nothing returns nothing
    local integer array arr
    local integer x = arr[[|DoNothing()|]]
endfunction",
            };
        }
    }
}
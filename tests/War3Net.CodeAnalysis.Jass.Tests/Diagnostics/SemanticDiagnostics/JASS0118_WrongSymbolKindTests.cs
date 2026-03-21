namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetWrongSymbolKindTests), DynamicDataSourceType.Method)]
        public void TestWrongSymbolKindDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.WrongSymbolKind.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetWrongSymbolKindTests()
        {
            yield return new object[]
            {
                @"
globals
    integer myVar = 5
endglobals

function main takes nothing returns nothing
    call [|myVar|]()
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x = 5
    call [|x|]()
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes code func returns nothing
    call [|func|]()
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    integer array myArray
endglobals

function main takes nothing returns nothing
    call [|myArray|]()
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    integer myVar = 5
endglobals

native TakeCode takes code c returns nothing

function main takes nothing returns nothing
    call TakeCode(function [|myVar|])
endfunction",
            };

            yield return new object[]
            {
                @"
type myhandle extends handle

native TakeCode takes code c returns nothing

function main takes nothing returns nothing
    call TakeCode(function [|myhandle|])
endfunction",
            };

            yield return new object[]
            {
                @"
function MyFunc takes nothing returns nothing
endfunction

function main takes nothing returns nothing
    set [|MyFunc|] = 5
endfunction",
            };

            yield return new object[]
            {
                @"
type myhandle extends handle

function main takes nothing returns nothing
    set [|myhandle|] = 5
endfunction",
            };

            yield return new object[]
            {
                @"
function MyFunc takes nothing returns nothing
endfunction

function main takes nothing returns integer
    return [|MyFunc|][0]
endfunction",
            };

            yield return new object[]
            {
                @"
type myhandle extends handle

function main takes nothing returns integer
    return [|myhandle|][0]
endfunction",
            };
        }
    }
}
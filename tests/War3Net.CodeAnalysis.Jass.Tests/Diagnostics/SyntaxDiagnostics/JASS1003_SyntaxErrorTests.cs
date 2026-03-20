namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSyntaxDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetSyntaxErrorTests), DynamicDataSourceType.Method)]
        public void TestSyntaxErrorDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.SyntaxError.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetSyntaxErrorTests()
        {
            yield return new object[]
            {
                @"
[|fucntion|] main takes nothing returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
[|golbals|]
    integer x
endglobals",
            };

            yield return new object[]
            {
                "[|@|]#$ invalid code",
                true,
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    call Foo()[|;|]
endfunction",
                true,
            };

            yield return new object?[]
            {
                "function [|main|]",
                true,
            };

            yield return new object[]
            {
                @"
globals
    integer x
endglobals

function foo takes nothing returns nothing
    set x [|5|]
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    integer x [|5|]
endglobals",
            };

            yield return new object[]
            {
                @"
globals
    integer array arr
endglobals

function foo takes nothing returns nothing
    local integer x = arr[5
[|endfunction|]",
            };

            yield return new object[]
            {
                @"
globals
    integer array arr
endglobals

function foo takes nothing returns nothing
    set arr[0 [|=|] 5
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    call Bar(5, 10
[|endfunction|]",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    set x = Bar(5, 10
[|endfunction|]",
            };

            yield return new object[]
            {
                @"
globals
    integer x
endglobals

function foo takes nothing returns nothing
    set x = (5 + 3
[|endfunction|]",
            };

            yield return new object[]
            {
                @"
function foo takes integer a [|integer|] [|b|] returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    call Bar(1 [|2|] 3)
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    call Bar(1, 2,[|)|]
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing
[|endfunction|]",
                true,
            };

            yield return new object[]
            {
                "type [|myunit|]",
                true,
            };

            yield return new object[]
            {
                "type myunit [|handle|]",
            };
        }
    }
}
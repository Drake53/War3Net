namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassStyleDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetTrailingWhitespaceTests), DynamicDataSourceType.Method)]
        public void TestTrailingWhitespaceDiagnostic(string markedCode)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassStyleDiagnostics.TrailingWhitespace.Id,
                markedCode);
        }

        private static IEnumerable<object?[]> GetTrailingWhitespaceTests()
        {
            yield return new object[]
            {
                @"
function main takes nothing returns nothing[|   |]
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    integer x
endglobals
function main takes nothing returns nothing
    set x = 5[| |]
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
endfunction
function main takes nothing returns nothing
    call foo()[| |]
endfunction",
            };

            yield return new object[]
            {
                @"
globals[|  |]
endglobals",
            };

            yield return new object[]
            {
                @"
globals
    integer x[|  |]
endglobals",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x[| |]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    if true then[|  |]
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    loop[| |]
        exitwhen true
    endloop
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns integer
    return 0[| |]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing[|  |]
    local integer x[| |]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
endfunction[| |]",
            };

            yield return new object?[]
            {
                @"
function main takes nothing returns nothing
endfunction
[|  |]",
            };

            yield return new object?[]
            {
                @"
function main takes nothing returns nothing
    // comment[|  |]
endfunction",
            };

            yield return new object?[]
            {
                @"
function main takes nothing returns nothing
[|    |]
endfunction",
            };

            yield return new object?[]
            {
                @"
function Foo takes nothing returns integer
    local integer i = 0
[|    |]
    return i + 5
endfunction",
            };
        }
    }
}
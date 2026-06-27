namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassCodeQualityDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetInfiniteLoopTests), DynamicDataSourceType.Method)]
        public void TestInfiniteLoopDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassCodeQualityDiagnostics.InfiniteLoop.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetInfiniteLoopTests()
        {
            yield return new object[]
            {
                @"
globals
    integer gValue = 0
endglobals

function main takes nothing returns nothing
    [|loop|]
        set gValue = gValue + 1
    endloop
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer i = 0
    [|loop|]
        loop
            exitwhen true
        endloop
        set i = i + 1
    endloop
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer i = 0
    loop
        [|loop|]
            set i = i + 1
        endloop
        exitwhen true
    endloop
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    [|loop|]
        exitwhen false
    endloop
endfunction",
                true,
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    [|loop|]
        if false then
            exitwhen true
        endif
    endloop
endfunction",
                true,
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer i = 20
    local integer j = 0
    [|loop|]
        loop
            set j = j + 3
            if j > i then
                exitwhen true
            endif
        endloop
        set i = i + 1
    endloop
endfunction",
            };
        }
    }
}
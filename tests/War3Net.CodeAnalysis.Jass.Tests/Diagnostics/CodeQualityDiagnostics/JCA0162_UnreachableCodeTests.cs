namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassCodeQualityDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetUnreachableCodeTests), DynamicDataSourceType.Method)]
        public void TestUnreachableCodeDiagnostic(string markedCode)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassCodeQualityDiagnostics.UnreachableCode.Id,
                markedCode);
        }

        private static IEnumerable<object?[]> GetUnreachableCodeTests()
        {
            yield return new object[]
            {
                @"
globals
    integer gValue = 0
endglobals

function main takes nothing returns nothing
    loop
        exitwhen true
        [|set gValue = 5|]
    endloop
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    integer gValue = 0
endglobals

function foo takes boolean b returns nothing
    if b then
        return
    else
        return
    endif
    [|set gValue = 5|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    if true then
        return
    else
        [|return|]
    endif

    [|call main()
    return|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes boolean b returns nothing
    if true or b then
        return
    else
        [|return|]
    endif

    [|call main()
    return|]
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    integer gValue = 0
endglobals

function main takes boolean b returns nothing
    if false and b then
        [|set gValue = 1|]
    endif
endfunction",
            };
        }
    }
}
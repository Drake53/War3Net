namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetUndefinedNameTests), DynamicDataSourceType.Method)]
        public void TestUndefinedNameDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.UndefinedName.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetUndefinedNameTests()
        {
            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    set [|x|] = 5
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer y = [|x|] + 1
endfunction",
            };

            yield return new object[]
            {
                @"
native DoSomething takes integer i returns nothing

function main takes nothing returns nothing
    call DoSomething([|unknownVar|])
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    if [|unknownBool|] then
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    call [|UnknownFunction|]()
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x = [|GetUnknownValue|]()
endfunction",
            };

            yield return new object[]
            {
                @"
native TakeInt takes integer i returns nothing

function main takes nothing returns nothing
    call TakeInt([|GetUnknownValue|]())
endfunction",
            };

            yield return new object[]
            {
                @"
native TakeCode takes code c returns nothing

function main takes nothing returns nothing
    call TakeCode(function [|UnknownFunction|])
endfunction",
            };
        }
    }
}
namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetExplicitConversionExistsTests), DynamicDataSourceType.Method)]
        public void TestExplicitConversionExistsDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.ExplicitConversionExists.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetExplicitConversionExistsTests()
        {
            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer i = [|3.14|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer i = 0
    set i = [|3.14|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer array arr
    set arr[0] = [|3.14|]
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    integer x = [|3.14|]
endglobals",
            };

            yield return new object[]
            {
                @"
globals
    constant integer C = [|3.14|]
endglobals",
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
type agent extends handle
type widget extends agent
type unit extends widget

function foo takes widget w returns nothing
    local unit u = [|w|]
endfunction",
            };

            yield return new object[]
            {
                @"
type agent extends handle
type widget extends agent
type unit extends widget

function foo takes agent a returns nothing
    local unit u = [|a|]
endfunction",
            };
        }
    }
}
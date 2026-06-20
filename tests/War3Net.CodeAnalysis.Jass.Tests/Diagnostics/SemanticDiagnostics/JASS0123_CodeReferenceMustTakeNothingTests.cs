namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetCodeReferenceMustTakeNothingTests), DynamicDataSourceType.Method)]
        public void TestCodeReferenceMustTakeNothingDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.CodeReferenceMustTakeNothing.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetCodeReferenceMustTakeNothingTests()
        {
            yield return new object[]
            {
                @"
function callback takes integer x returns nothing
endfunction

function foo takes nothing returns nothing
    local code c = function [|callback|]
endfunction",
            };

            yield return new object[]
            {
                @"
function callback takes integer x, real y returns boolean
    return true
endfunction

function bar takes code c returns nothing
endfunction

function foo takes nothing returns nothing
    call bar(function [|callback|])
endfunction",
            };
        }
    }
}
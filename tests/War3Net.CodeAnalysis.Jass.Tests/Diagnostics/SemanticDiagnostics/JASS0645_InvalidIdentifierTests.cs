namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    [TestClass]
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetInvalidIdentifierTests), DynamicDataSourceType.Method)]
        public void TestInvalidIdentifierDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.InvalidIdentifier.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetInvalidIdentifierTests()
        {
            yield return new object[]
            {
                @"
function [|_2ndFunc|] takes nothing returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
function [|SecondFunc_|] takes nothing returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
type [|_myType|] extends handle",
            };

            yield return new object[]
            {
                @"
globals
    integer [|count_|] = 0
endglobals",
            };

            yield return new object[]
            {
                @"
globals
    constant integer [|_MAX|] = 100
endglobals",
            };

            yield return new object[]
            {
                @"
function foo takes integer [|x_|] returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    local integer [|_bar|] = 0
endfunction",
            };

            yield return new object[]
            {
                @"
native [|_DoSomething|] takes nothing returns nothing",
            };
        }
    }
}
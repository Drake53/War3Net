namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSyntaxDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetInvalidDebugStatementTests), DynamicDataSourceType.Method)]
        public void TestInvalidDebugStatementDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.InvalidDebugStatement.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetInvalidDebugStatementTests()
        {
            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    [|debug return|]
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns integer
    [|debug return 5|]
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    loop
        [|debug exitwhen true|]
    endloop
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    [|debug local integer x = 5|]
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    [|debug local integer array x|]
endfunction",
            };

            yield return new object[]
            {
                @"
function bar takes nothing returns nothing
endfunction

function foo takes nothing returns nothing
    [|debug debug call bar()|]
endfunction",
            };
        }
    }
}
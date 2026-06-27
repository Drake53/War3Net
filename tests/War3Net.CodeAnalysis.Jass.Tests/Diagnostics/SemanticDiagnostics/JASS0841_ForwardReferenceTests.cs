namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetForwardReferenceTests), DynamicDataSourceType.Method)]
        public void TestForwardReferenceDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.ForwardReference.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetForwardReferenceTests()
        {
            yield return new object[]
            {
                @"
function Caller takes nothing returns nothing
    call [|Helper|]()
endfunction

function Helper takes nothing returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    integer x = [|y|]
    integer y = 5
endglobals",
            };
        }
    }
}
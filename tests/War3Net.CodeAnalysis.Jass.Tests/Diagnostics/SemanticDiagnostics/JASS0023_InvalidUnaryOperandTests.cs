namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetInvalidUnaryOperandTests), DynamicDataSourceType.Method)]
        public void TestInvalidUnaryOperandDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.InvalidUnaryOperand.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetInvalidUnaryOperandTests()
        {
            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local string s = [|-""hello""|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local boolean b = [|-true|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x = [|not 5|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local boolean b = [|not ""hello""|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local boolean b = [|not 3.14|]
endfunction",
            };
        }
    }
}
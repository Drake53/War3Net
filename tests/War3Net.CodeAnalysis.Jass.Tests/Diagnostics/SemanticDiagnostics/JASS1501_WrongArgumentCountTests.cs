namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetWrongArgumentCountTests), DynamicDataSourceType.Method)]
        public void TestWrongArgumentCountDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.WrongArgumentCount.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetWrongArgumentCountTests()
        {
            yield return new object[]
            {
                @"
native TakeTwo takes integer a, integer b returns nothing

function main takes nothing returns nothing
    call TakeTwo[|(5)|]
endfunction",
            };

            yield return new object[]
            {
                @"
native TakeOne takes integer a returns nothing

function main takes nothing returns nothing
    call TakeOne[|(5, 10, 15)|]
endfunction",
            };

            yield return new object[]
            {
                @"
native TakeNone takes nothing returns nothing

function main takes nothing returns nothing
    call TakeNone[|(5)|]
endfunction",
            };

            yield return new object[]
            {
                @"
native TakeOne takes integer a returns nothing

function main takes nothing returns nothing
    call TakeOne[|()|]
endfunction",
            };
        }
    }
}
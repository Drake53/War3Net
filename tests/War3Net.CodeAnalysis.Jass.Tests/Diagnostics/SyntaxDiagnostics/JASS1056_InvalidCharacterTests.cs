namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSyntaxDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetInvalidCharacterTests), DynamicDataSourceType.Method)]
        public void TestInvalidCharacterDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.InvalidCharacter.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetInvalidCharacterTests()
        {
            yield return new object[]
            {
                @"
globals
    boolean b = [|!|]true
endglobals",
                true,
            };

            yield return new object[]
            {
                @"
globals
    integer x = [|@|]
endglobals",
                true,
            };

            yield return new object[]
            {
                @"
globals
    integer x = [|#|]
endglobals",
                true,
            };
        }
    }
}
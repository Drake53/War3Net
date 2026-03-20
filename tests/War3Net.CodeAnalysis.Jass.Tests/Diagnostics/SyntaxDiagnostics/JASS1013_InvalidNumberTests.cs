namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSyntaxDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetInvalidNumberTests), DynamicDataSourceType.Method)]
        public void TestInvalidNumberDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.InvalidNumber.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetInvalidNumberTests()
        {
            yield return new object[]
            {
                @"
globals
    integer x = [|0x|]
endglobals",
            };

            yield return new object[]
            {
                @"
globals
    integer x = [|$|]
endglobals",
            };

            yield return new object[]
            {
                @"
globals
    integer x = [|.|]
endglobals",
            };

            yield return new object[]
            {
                @"
globals
    real x = [|0891|]
endglobals",
            };

            yield return new object[]
            {
                @"
globals
    integer x = [|0192|]
endglobals",
            };
        }
    }
}
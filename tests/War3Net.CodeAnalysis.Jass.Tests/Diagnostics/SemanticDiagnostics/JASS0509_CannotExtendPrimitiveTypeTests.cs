namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetCannotExtendPrimitiveTypeTests), DynamicDataSourceType.Method)]
        public void TestCannotExtendPrimitiveTypeDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.CannotExtendPrimitiveType.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetCannotExtendPrimitiveTypeTests()
        {
            yield return new object[]
            {
                @"
type myint extends [|integer|]",
            };

            yield return new object[]
            {
                @"
type myreal extends [|real|]",
            };

            yield return new object[]
            {
                @"
type mybool extends [|boolean|]",
            };

            yield return new object[]
            {
                @"
type mystring extends [|string|]",
            };

            yield return new object[]
            {
                @"
type mycode extends [|code|]",
            };
        }
    }
}
namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetCircularTypeExtensionTests), DynamicDataSourceType.Method)]
        public void TestCircularTypeExtensionDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.CircularTypeExtension.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetCircularTypeExtensionTests()
        {
            yield return new object[]
            {
                @"
type mytype extends [|mytype|]",
            };

            yield return new object[]
            {
                @"
type mytype extends handle
type mytype extends [|mytype|]",
                true,
            };

            yield return new object?[]
            {
                @"
type typeA extends handle
type typeB extends typeA
type typeA extends [|typeB|]",
                true,
            };

            yield return new object?[]
            {
                @"
type typeA extends handle
type typeB extends typeA
type typeC extends typeB
type typeA extends [|typeC|]",
                true,
            };

            yield return new object?[]
            {
                @"
type typeA extends handle
type typeB extends typeA
type typeA extends [|typeB|]
type typeC extends typeB
type typeD extends typeA",
                true,
            };
        }
    }
}
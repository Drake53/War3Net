namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetDuplicateParameterNameTests), DynamicDataSourceType.Method)]
        public void TestDuplicateParameterNameDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.DuplicateParameterName.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetDuplicateParameterNameTests()
        {
            yield return new object[]
            {
                @"
function foo takes integer x, integer [|x|] returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes integer x, real [|x|] returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes integer a, real b, integer [|a|] returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
native foo takes integer x, integer [|x|] returns nothing",
            };

            yield return new object[]
            {
                @"
function foo takes integer x, integer [|x|], integer [|x|] returns nothing
endfunction",
            };
        }
    }
}
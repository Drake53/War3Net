namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetDuplicateLocalDeclarationTests), DynamicDataSourceType.Method)]
        public void TestDuplicateLocalDeclarationDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.DuplicateLocalDeclaration.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetDuplicateLocalDeclarationTests()
        {
            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x
    local integer [|x|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x
    local real [|x|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x
    local integer array [|x|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x
    local integer [|x|]
    local integer [|x|]
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes integer x returns nothing
    local integer [|x|]
endfunction",
            };
        }
    }
}
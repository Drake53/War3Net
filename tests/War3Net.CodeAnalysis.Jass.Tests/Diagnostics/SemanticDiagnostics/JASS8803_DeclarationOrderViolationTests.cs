namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetDeclarationOrderViolationTests), DynamicDataSourceType.Method)]
        public void TestDeclarationOrderViolationDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.DeclarationOrderViolation.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetDeclarationOrderViolationTests()
        {
            yield return new object[]
            {
                @"
globals
endglobals

[|type|] mytype extends handle",
            };

            yield return new object[]
            {
                @"
native Foo takes nothing returns nothing

[|type|] mytype extends handle",
            };

            yield return new object[]
            {
                @"
function Foo takes nothing returns nothing
endfunction

[|type|] mytype extends handle",
            };

            yield return new object[]
            {
                @"
native Foo takes nothing returns nothing

[|globals|]
endglobals",
            };

            yield return new object[]
            {
                @"
function Foo takes nothing returns nothing
endfunction

[|globals|]
endglobals",
            };

            yield return new object[]
            {
                @"
function Foo takes nothing returns nothing
endfunction

[|native|] Bar takes nothing returns nothing",
            };

            yield return new object[]
            {
                @"
function Foo takes nothing returns nothing
endfunction

constant [|native|] Bar takes nothing returns nothing",
            };
        }
    }
}
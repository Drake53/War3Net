namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetUndefinedTypeTests), DynamicDataSourceType.Method)]
        public void TestUndefinedTypeDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.UndefinedType.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetUndefinedTypeTests()
        {
            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local [|UnknownType|] x
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    [|UnknownType|] myGlobal
endglobals",
            };

            yield return new object[]
            {
                @"
function foo takes [|UnknownType|] x returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns [|UnknownType|]
    return null
endfunction",
            };

            yield return new object[]
            {
                @"
native foo takes [|UnknownType|] x returns nothing",
            };

            yield return new object[]
            {
                @"
native foo takes nothing returns [|UnknownType|]",
            };

            yield return new object[]
            {
                @"
globals
    [|UnknownType|] array myArray
endglobals",
            };

            yield return new object[]
            {
                @"
type mytype extends [|unknowntype|]",
            };

            yield return new object[]
            {
                @"
type mytype extends [|handl|]",
            };
        }
    }
}
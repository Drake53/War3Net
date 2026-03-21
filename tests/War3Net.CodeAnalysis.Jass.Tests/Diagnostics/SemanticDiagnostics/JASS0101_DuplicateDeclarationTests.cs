namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetDuplicateDeclarationTests), DynamicDataSourceType.Method)]
        public void TestDuplicateDeclarationDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.DuplicateDeclaration.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetDuplicateDeclarationTests()
        {
            yield return new object[]
            {
                @"
globals
    integer x
    integer [|x|]
endglobals",
            };

            yield return new object[]
            {
                @"
globals
    integer x
    real [|x|]
endglobals",
            };

            yield return new object[]
            {
                @"
globals
    constant integer x = 5
    integer [|x|]
endglobals",
            };

            yield return new object[]
            {
                @"
globals
    integer x
endglobals

globals
    integer [|x|]
endglobals",
            };

            yield return new object[]
            {
                @"
globals
    integer x
    integer array [|x|]
endglobals",
            };

            yield return new object[]
            {
                @"
type myhandle extends handle
type [|myhandle|] extends handle",
            };

            yield return new object[]
            {
                @"
type agent extends handle
type mytype extends handle
type [|mytype|] extends agent",
            };

            yield return new object[]
            {
                @"
type myhandle extends handle
type [|myhandle|] extends handle
type [|myhandle|] extends handle",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
endfunction

function [|foo|] takes nothing returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
endfunction

function [|foo|] takes integer x returns integer
    return x
endfunction",
            };

            yield return new object[]
            {
                @"
native foo takes nothing returns nothing

function [|foo|] takes nothing returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
endfunction

function [|foo|] takes nothing returns nothing
endfunction

function [|foo|] takes nothing returns nothing
endfunction",
            };
        }
    }
}
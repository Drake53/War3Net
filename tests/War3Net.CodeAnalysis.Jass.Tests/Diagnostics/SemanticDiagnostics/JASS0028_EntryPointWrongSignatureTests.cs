namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetEntryPointWrongSignatureTests), DynamicDataSourceType.Method)]
        public void TestEntryPointWrongSignatureDiagnostic(string markedCode)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.EntryPointWrongSignature.Id,
                markedCode,
                hasCascadingErrors: false,
                filePath: "war3map.j");
        }

        private static IEnumerable<object?[]> GetEntryPointWrongSignatureTests()
        {
            yield return new object[]
            {
                @"
function [|main|] takes integer difficulty returns nothing
endfunction

function config takes nothing returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
function [|main|] takes nothing returns integer
    return 0
endfunction

function config takes nothing returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
endfunction

function [|config|] takes integer p returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
function [|main|] takes nothing returns integer
    return 0
endfunction

function [|config|] takes nothing returns boolean
    return false
endfunction",
            };
        }
    }
}
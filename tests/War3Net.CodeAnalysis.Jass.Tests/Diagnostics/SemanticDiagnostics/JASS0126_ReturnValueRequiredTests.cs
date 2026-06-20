namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetReturnValueRequiredTests), DynamicDataSourceType.Method)]
        public void TestReturnValueRequiredDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.ReturnValueRequired.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetReturnValueRequiredTests()
        {
            yield return new object[]
            {
                @"
function GetInt takes nothing returns integer
    [|return|]
endfunction",
            };

            yield return new object[]
            {
                @"
function GetString takes nothing returns string
    [|return|]
endfunction",
            };

            yield return new object[]
            {
                @"
function GetBool takes nothing returns boolean
    [|return|]
endfunction",
            };

            yield return new object[]
            {
                @"
function GetInt takes boolean b returns integer
    if b then
        return 1
    else
        [|return|]
    endif
endfunction",
            };
        }
    }
}
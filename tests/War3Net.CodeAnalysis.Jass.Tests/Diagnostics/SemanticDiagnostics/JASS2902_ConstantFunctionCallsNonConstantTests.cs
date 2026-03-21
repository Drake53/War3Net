namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetConstantFunctionCallsNonConstantTests), DynamicDataSourceType.Method)]
        public void TestConstantFunctionCallsNonConstantDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.ConstantFunctionCallsNonConstant.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetConstantFunctionCallsNonConstantTests()
        {
            yield return new object[]
            {
                @"
function NonConstantHelper takes nothing returns integer
    return 42
endfunction

constant function GetValue takes nothing returns integer
    return [|NonConstantHelper|]()
endfunction",
            };

            yield return new object[]
            {
                @"
function GetBase takes nothing returns integer
    return 10
endfunction

constant function GetMultiplied takes nothing returns integer
    return [|GetBase|]() * 2
endfunction",
            };

            yield return new object[]
            {
                @"
function ShouldUseAlt takes nothing returns boolean
    return true
endfunction

constant function GetValue takes nothing returns integer
    if [|ShouldUseAlt|]() then
        return 1
    endif
    return 2
endfunction",
            };

            yield return new object[]
            {
                @"
native DoSomething takes nothing returns nothing

constant function CallFunc takes nothing returns integer
    call [|DoSomething|]()
    return 0
endfunction",
            };
        }
    }
}
namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetArgumentTypeMismatchTests), DynamicDataSourceType.Method)]
        public void TestArgumentTypeMismatchDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.ArgumentTypeMismatch.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetArgumentTypeMismatchTests()
        {
            yield return new object[]
            {
                @"
native TakeInt takes integer i returns nothing

function main takes nothing returns nothing
    call TakeInt([|""hello""|])
endfunction",
            };

            yield return new object[]
            {
                @"
native TakeInt takes integer i returns nothing

function main takes nothing returns nothing
    call TakeInt([|true|])
endfunction",
            };

            yield return new object[]
            {
                @"
native TakeString takes string s returns nothing

function main takes nothing returns nothing
    call TakeString([|42|])
endfunction",
            };

            yield return new object[]
            {
                @"
native TakeBool takes boolean b returns nothing

function main takes nothing returns nothing
    call TakeBool([|42|])
endfunction",
            };

            yield return new object[]
            {
                @"
native TakeTwo takes integer i, string s returns nothing

function main takes nothing returns nothing
    call TakeTwo([|""hello""|], [|42|])
endfunction",
            };

            yield return new object[]
            {
                @"
native TakeInt takes integer i returns nothing

function DoNothing takes nothing returns nothing
endfunction

function main takes nothing returns nothing
    call TakeInt([|DoNothing()|])
endfunction",
            };

            yield return new object[]
            {
                @"
type unit extends handle
type item extends handle

native TakeItem takes item i returns nothing

function main takes nothing returns nothing
    local unit u = null
    call TakeItem([|u|])
endfunction",
            };

            yield return new object[]
            {
                @"
native TakeInt takes integer i returns nothing

function main takes nothing returns nothing
    call TakeInt([|null|])
endfunction",
            };

            yield return new object[]
            {
                @"
native TakeReal takes real r returns nothing

function main takes nothing returns nothing
    call TakeReal([|null|])
endfunction",
            };

            yield return new object[]
            {
                @"
native TakeBool takes boolean b returns nothing

function main takes nothing returns nothing
    call TakeBool([|null|])
endfunction",
            };

            yield return new object[]
            {
                @"
native TakeTwo takes integer i, boolean b returns nothing

function main takes nothing returns nothing
    call TakeTwo([|null|], [|null|])
endfunction",
            };
        }
    }
}
namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetIncompatibleOperandTypesTests), DynamicDataSourceType.Method)]
        public void TestIncompatibleOperandTypesDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.IncompatibleOperandTypes.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetIncompatibleOperandTypesTests()
        {
            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x = [|""hello"" + 5|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local boolean x = [|true - false|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local string x = [|""a"" * ""b""|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local string x = [|""hello"" / 2|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local boolean x = [|""hello"" < 5|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local boolean x = [|1 and 2|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local boolean x = [|""a"" or ""b""|]
endfunction",
            };

            yield return new object[]
            {
                @"
function DoNothing takes nothing returns nothing
endfunction

function main takes nothing returns nothing
    local integer x = [|DoNothing() + 5|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local boolean b = [|""abc"" < ""def""|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local boolean b = [|""abc"" > ""def""|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local boolean b = [|""abc"" <= ""def""|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local boolean b = [|""abc"" >= ""def""|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local boolean b = [|5 == ""hello""|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local boolean b = [|true == 5|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local boolean b = [|""hello"" != false|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local boolean b = [|3.14 > ""hello""|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local boolean b = [|true < ""hello""|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x = [|true + 1|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x = [|5 - false|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x = [|true * 2|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x = [|10 / true|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x = [|true + false|]
endfunction",
            };
        }
    }
}
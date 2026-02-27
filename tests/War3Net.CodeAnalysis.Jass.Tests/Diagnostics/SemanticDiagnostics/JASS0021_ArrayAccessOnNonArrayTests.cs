// ------------------------------------------------------------------------------
// <copyright file="JASS0021_ArrayAccessOnNonArrayTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.CodeAnalysis.Jass.Diagnostics;

namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSemanticDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetArrayAccessOnNonArrayTests), DynamicDataSourceType.Method)]
        public void TestArrayAccessOnNonArrayDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.ArrayAccessOnNonArray.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetArrayAccessOnNonArrayTests()
        {
            yield return new object[]
            {
                @"
globals
    integer myValue = 5
endglobals

function main takes nothing returns nothing
    local integer x = [|myValue|][0]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x = 5
    local integer y = [|x|][0]
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes integer x returns integer
    return [|x|][0]
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    integer myValue = 5
endglobals

function main takes nothing returns nothing
    set [|myValue|][0] = 10
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    string s = ""hello world""
endglobals

function main takes nothing returns nothing
    local string char = [|s|][0]
endfunction",
            };
        }
    }
}
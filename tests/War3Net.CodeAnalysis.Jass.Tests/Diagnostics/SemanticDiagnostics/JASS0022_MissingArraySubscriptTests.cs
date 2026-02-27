// ------------------------------------------------------------------------------
// <copyright file="JASS0022_MissingArraySubscriptTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetMissingArraySubscriptTests), DynamicDataSourceType.Method)]
        public void TestMissingArraySubscriptDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.MissingArraySubscript.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetMissingArraySubscriptTests()
        {
            yield return new object[]
            {
                @"
globals
    integer array myArray
endglobals

function main takes nothing returns nothing
    local integer x = [|myArray|]
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    integer array myArray
endglobals

function main takes nothing returns nothing
    set [|myArray|] = 5
endfunction",
            };

            yield return new object[]
            {
                @"
native TakeInt takes integer i returns nothing

globals
    integer array myArray
endglobals

function main takes nothing returns nothing
    call TakeInt([|myArray|])
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer array arr
    local integer x = [|arr|]
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    integer array myArray
endglobals

function main takes nothing returns nothing
    local integer x = [|myArray|] + 5
endfunction",
            };
        }
    }
}
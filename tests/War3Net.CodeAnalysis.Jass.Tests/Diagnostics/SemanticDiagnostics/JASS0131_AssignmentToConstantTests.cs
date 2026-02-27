// ------------------------------------------------------------------------------
// <copyright file="JASS0131_AssignmentToConstantTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetCannotAssignToConstantTests), DynamicDataSourceType.Method)]
        public void TestCannotAssignToConstantDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.CannotAssignToConstant.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetCannotAssignToConstantTests()
        {
            yield return new object[]
            {
                @"
globals
    constant integer MAX_VALUE = 100
endglobals

function main takes nothing returns nothing
    set [|MAX_VALUE|] = 200
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    constant integer MAX_VALUE = 100
endglobals

function main takes nothing returns nothing
    set [|MAX_VALUE|] = 200
    set [|MAX_VALUE|] = 300
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    constant integer MAX_VALUE = 100
endglobals

function main takes nothing returns nothing
    loop
        set [|MAX_VALUE|] = 200
        exitwhen true
    endloop
endfunction",
            };
        }
    }
}
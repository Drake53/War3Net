// ------------------------------------------------------------------------------
// <copyright file="JASS0139_ExitWhenOutsideLoopTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetExitWhenOutsideLoopTests), DynamicDataSourceType.Method)]
        public void TestExitWhenOutsideLoopDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.ExitWhenOutsideLoop.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetExitWhenOutsideLoopTests()
        {
            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    [|exitwhen true|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    if true then
        [|exitwhen true|]
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    if false then
    else
        [|exitwhen true|]
    endif
endfunction",
            };
        }
    }
}
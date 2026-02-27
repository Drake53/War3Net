// ------------------------------------------------------------------------------
// <copyright file="JASS0163_UnreachableCodeAfterReturnTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetUnreachableCodeAfterReturnTests), DynamicDataSourceType.Method)]
        public void TestUnreachableCodeAfterReturnDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.UnreachableCodeAfterReturn.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetUnreachableCodeAfterReturnTests()
        {
            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    return
    [|local integer x = 5|]
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    integer gValue = 0
endglobals

function GetInt takes nothing returns integer
    return 42
    [|set gValue = 100|]
endfunction",
            };
        }
    }
}
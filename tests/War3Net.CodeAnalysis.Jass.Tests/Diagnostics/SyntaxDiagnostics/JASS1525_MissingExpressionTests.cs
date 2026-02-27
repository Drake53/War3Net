// ------------------------------------------------------------------------------
// <copyright file="JASS1525_MissingExpressionTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.CodeAnalysis.Jass.Diagnostics;

namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    public partial class JassSyntaxDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetMissingExpressionTests), DynamicDataSourceType.Method)]
        public void TestMissingExpressionDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.MissingExpression.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetMissingExpressionTests()
        {
            yield return new object[]
            {
                @"
globals
    integer x
endglobals

function foo takes nothing returns nothing
    set x =
[|endfunction|]",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    call Bar([|,|] 5)
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    if [|then|]
        call Bar()
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    loop
        exitwhen
    [|endloop|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    set x = 5 +
[|endfunction|]",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    set x = 5 + [|*|]
[|endfunction|]",
            };
        }
    }
}
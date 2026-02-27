// ------------------------------------------------------------------------------
// <copyright file="JASS1073_UnexpectedTokenTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetUnexpectedTokenTests), DynamicDataSourceType.Method)]
        public void TestUnexpectedTokenDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.UnexpectedToken.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetUnexpectedTokenTests()
        {
            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    call Bar()
    [|endif|]
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    if true then
        call Bar()
    endif
    [|endif|]
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    call Bar()
    [|endloop|]
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    loop
        exitwhen true
    endloop
    [|endloop|]
endfunction",
            };
        }
    }
}
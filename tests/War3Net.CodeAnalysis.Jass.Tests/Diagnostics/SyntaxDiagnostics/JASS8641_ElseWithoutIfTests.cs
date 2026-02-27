// ------------------------------------------------------------------------------
// <copyright file="JASS8641_ElseWithoutIfTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.CodeAnalysis.Jass.Diagnostics;

namespace War3Net.CodeAnalysis.Jass.Tests.Diagnostics
{
    [TestClass]
    public partial class JassSyntaxDiagnosticsTests
    {
        [TestMethod]
        [DynamicData(nameof(GetElseWithoutIfTests), DynamicDataSourceType.Method)]
        public void TestElseWithoutIfDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.ElseWithoutIf.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetElseWithoutIfTests()
        {
            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    [|else|]
        call Bar()
    endif
endfunction",
                true,
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    if true then
        call A()
    endif
    [|else|]
        call B()
    endif
endfunction",
                true,
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    [|elseif|] true then
        call Bar()
    endif
endfunction",
                true,
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    if true then
        call A()
    endif
    [|elseif|] false then
        call B()
    endif
endfunction",
                true,
            };
        }
    }
}
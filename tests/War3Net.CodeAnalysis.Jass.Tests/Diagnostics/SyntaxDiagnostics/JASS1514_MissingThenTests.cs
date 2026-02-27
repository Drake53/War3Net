// ------------------------------------------------------------------------------
// <copyright file="JASS1514_MissingThenTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetMissingThenTests), DynamicDataSourceType.Method)]
        public void TestMissingThenDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.MissingThen.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetMissingThenTests()
        {
            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    if true
        [|call|] Bar()
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns nothing
    if true then
        call A()
    elseif false
        [|call|] B()
    endif
endfunction",
            };
        }
    }
}
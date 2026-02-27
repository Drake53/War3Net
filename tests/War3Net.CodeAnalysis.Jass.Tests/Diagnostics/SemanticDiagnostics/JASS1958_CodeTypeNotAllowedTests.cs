// ------------------------------------------------------------------------------
// <copyright file="JASS1958_CodeTypeNotAllowedTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetCodeTypeNotAllowedTests), DynamicDataSourceType.Method)]
        public void TestCodeTypeNotAllowedDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.CodeTypeNotAllowed.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetCodeTypeNotAllowedTests()
        {
            yield return new object[]
            {
                @"
globals
    code array [|callbacks|]
endglobals",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local code array [|callbacks|]
endfunction",
            };
        }
    }
}
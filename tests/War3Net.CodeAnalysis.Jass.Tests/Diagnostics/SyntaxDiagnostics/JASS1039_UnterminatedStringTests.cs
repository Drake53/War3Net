// ------------------------------------------------------------------------------
// <copyright file="JASS1039_UnterminatedStringTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetUnterminatedStringTests), DynamicDataSourceType.Method)]
        public void TestUnterminatedStringDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.UnterminatedString.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetUnterminatedStringTests()
        {
            yield return new object?[]
            {
                @"
function main takes nothing returns nothing
    call BJDebugMsg([|""Hello World)
endfunction|]",
                true,
            };

            yield return new object?[]
            {
                @"
function main takes nothing returns nothing
    call BJDebugMsg([|""Hello|]",
                true,
            };
        }
    }
}
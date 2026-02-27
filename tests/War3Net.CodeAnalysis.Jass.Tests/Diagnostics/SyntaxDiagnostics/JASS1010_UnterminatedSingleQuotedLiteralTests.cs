// ------------------------------------------------------------------------------
// <copyright file="JASS1010_UnterminatedSingleQuotedLiteralTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetUnterminatedSingleQuotedLiteralTests), DynamicDataSourceType.Method)]
        public void TestUnterminatedSingleQuotedLiteralDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.UnterminatedSingleQuotedLiteral.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetUnterminatedSingleQuotedLiteralTests()
        {
            yield return new object[]
            {
                @"
globals
    integer c = [|'A
endglobals|]",
                true,
            };

            yield return new object[]
            {
                @"
globals
    integer id = [|'hfoo
endglobals|]",
                true,
            };
        }
    }
}
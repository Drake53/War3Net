// ------------------------------------------------------------------------------
// <copyright file="JASS1012_InvalidSingleQuotedStringLengthTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetInvalidSingleQuotedStringLengthTests), DynamicDataSourceType.Method)]
        public void TestInvalidSingleQuotedStringLengthDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.InvalidSingleQuotedStringLength.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetInvalidSingleQuotedStringLengthTests()
        {
            yield return new object[]
            {
                @"
globals
    integer x = [|''|]
endglobals",
            };

            yield return new object[]
            {
                @"
globals
    integer id = [|'Ah'|]
endglobals",
            };

            yield return new object[]
            {
                @"
globals
    integer id = [|'foobar'|]
endglobals",
            };
        }
    }
}
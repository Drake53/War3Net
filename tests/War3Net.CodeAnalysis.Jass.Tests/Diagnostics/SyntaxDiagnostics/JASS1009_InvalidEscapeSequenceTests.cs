// ------------------------------------------------------------------------------
// <copyright file="JASS1009_InvalidEscapeSequenceTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetInvalidEscapeSequenceTests), DynamicDataSourceType.Method)]
        public void TestInvalidEscapeSequenceDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.InvalidEscapeSequence.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetInvalidEscapeSequenceTests()
        {
            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    call BJDebugMsg(""C:[|\u|]sers"")
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    call BJDebugMsg(""100[|\%|] done"")
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    integer c = '[|\u|]'
endglobals",
            };

            yield return new object[]
            {
                @"
globals
    integer c = 'AB[|\u|]D'
endglobals",
            };
        }
    }
}
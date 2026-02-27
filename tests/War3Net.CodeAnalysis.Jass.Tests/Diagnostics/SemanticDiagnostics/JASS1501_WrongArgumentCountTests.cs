// ------------------------------------------------------------------------------
// <copyright file="JASS1501_WrongArgumentCountTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetWrongArgumentCountTests), DynamicDataSourceType.Method)]
        public void TestWrongArgumentCountDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.WrongArgumentCount.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetWrongArgumentCountTests()
        {
            yield return new object[]
            {
                @"
native TakeTwo takes integer a, integer b returns nothing

function main takes nothing returns nothing
    call TakeTwo[|(5)|]
endfunction",
            };

            yield return new object[]
            {
                @"
native TakeOne takes integer a returns nothing

function main takes nothing returns nothing
    call TakeOne[|(5, 10, 15)|]
endfunction",
            };

            yield return new object[]
            {
                @"
native TakeNone takes nothing returns nothing

function main takes nothing returns nothing
    call TakeNone[|(5)|]
endfunction",
            };

            yield return new object[]
            {
                @"
native TakeOne takes integer a returns nothing

function main takes nothing returns nothing
    call TakeOne[|()|]
endfunction",
            };
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="JASS0100_DuplicateParameterNameTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetDuplicateParameterNameTests), DynamicDataSourceType.Method)]
        public void TestDuplicateParameterNameDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.DuplicateParameterName.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetDuplicateParameterNameTests()
        {
            yield return new object[]
            {
                @"
function foo takes integer x, integer [|x|] returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes integer x, real [|x|] returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes integer a, real b, integer [|a|] returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
native foo takes integer x, integer [|x|] returns nothing",
            };

            yield return new object[]
            {
                @"
function foo takes integer x, integer [|x|], integer [|x|] returns nothing
endfunction",
            };
        }
    }
}
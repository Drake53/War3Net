// ------------------------------------------------------------------------------
// <copyright file="JASS0127_UnexpectedReturnTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetUnexpectedReturnTests), DynamicDataSourceType.Method)]
        public void TestUnexpectedReturnDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.UnexpectedReturn.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetUnexpectedReturnTests()
        {
            yield return new object[]
            {
                @"
function DoNothing takes nothing returns nothing
    return [|42|]
endfunction",
            };

            yield return new object[]
            {
                @"
function DoNothing takes nothing returns nothing
    return [|""hello""|]
endfunction",
            };

            yield return new object[]
            {
                @"
function DoNothing takes nothing returns nothing
    return [|true|]
endfunction",
            };

            yield return new object[]
            {
                @"
function DoNothing takes nothing returns nothing
    return [|1 + 2|]
endfunction",
            };
        }
    }
}
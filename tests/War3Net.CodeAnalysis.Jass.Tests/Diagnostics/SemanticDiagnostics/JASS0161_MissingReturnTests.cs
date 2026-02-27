// ------------------------------------------------------------------------------
// <copyright file="JASS0161_MissingReturnTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetMissingReturnTests), DynamicDataSourceType.Method)]
        public void TestMissingReturnDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.MissingReturn.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetMissingReturnTests()
        {
            yield return new object[]
            {
                @"
function [|GetInt|] takes nothing returns integer
endfunction",
            };

            yield return new object[]
            {
                @"
function [|GetInt|] takes boolean b returns integer
    if b then
        return 1
    endif
endfunction",
            };

            yield return new object[]
            {
                @"
function [|GetInt|] takes boolean b returns integer
    if b then
        // missing return here
    else
        return 2
    endif
endfunction",
            };
        }
    }
}
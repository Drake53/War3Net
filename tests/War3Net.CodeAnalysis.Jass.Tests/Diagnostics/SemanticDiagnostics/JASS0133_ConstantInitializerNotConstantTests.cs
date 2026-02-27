// ------------------------------------------------------------------------------
// <copyright file="JASS0133_ConstantInitializerNotConstantTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetConstantInitializerNotConstantTests), DynamicDataSourceType.Method)]
        public void TestConstantInitializerNotConstantDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.ConstantInitializerNotConstant.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetConstantInitializerNotConstantTests()
        {
            yield return new object[]
            {
                @"
function GetValue takes nothing returns integer
    return 42
endfunction

globals
    constant integer VALUE = [|GetValue()|]
endglobals",
            };

            yield return new object[]
            {
                @"
globals
    integer baseValue = 10
    constant integer VALUE = [|baseValue * 2|]
endglobals",
            };

            yield return new object[]
            {
                @"
native GetRandomInt takes integer lowBound, integer highBound returns integer

globals
    constant integer VALUE = [|GetRandomInt(0, 100)|]
endglobals",
            };
        }
    }
}
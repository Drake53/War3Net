// ------------------------------------------------------------------------------
// <copyright file="JASS1040_ConstructMustAppearOnOwnLineTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetConstructMustAppearOnOwnLineTests), DynamicDataSourceType.Method)]
        public void TestConstructMustAppearOnOwnLineDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSyntaxDiagnostics.ConstructMustAppearOnOwnLine.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetConstructMustAppearOnOwnLineTests()
        {
            yield return new object?[]
            {
                @"
function Foo takes nothing returns nothing
    call
    [|Foo|]
    [|(|]
    [|)|]
endfunction",
            };
        }
    }
}
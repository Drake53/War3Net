// ------------------------------------------------------------------------------
// <copyright file="JASS0128_DuplicateLocalDeclarationTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetDuplicateLocalDeclarationTests), DynamicDataSourceType.Method)]
        public void TestDuplicateLocalDeclarationDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.DuplicateLocalDeclaration.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetDuplicateLocalDeclarationTests()
        {
            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x
    local integer [|x|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x
    local real [|x|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x
    local integer array [|x|]
endfunction",
            };

            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local integer x
    local integer [|x|]
    local integer [|x|]
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes integer x returns nothing
    local integer [|x|]
endfunction",
            };
        }
    }
}
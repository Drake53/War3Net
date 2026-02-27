// ------------------------------------------------------------------------------
// <copyright file="JASS0246_UndefinedTypeTests.cs" company="Drake53">
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
        [DynamicData(nameof(GetUndefinedTypeTests), DynamicDataSourceType.Method)]
        public void TestUndefinedTypeDiagnostic(string markedCode, bool hasCascadingErrors = false)
        {
            DiagnosticAssert.ReportsDiagnostic(
                JassSemanticDiagnostics.UndefinedType.Id,
                markedCode,
                hasCascadingErrors);
        }

        private static IEnumerable<object?[]> GetUndefinedTypeTests()
        {
            yield return new object[]
            {
                @"
function main takes nothing returns nothing
    local [|UnknownType|] x
endfunction",
            };

            yield return new object[]
            {
                @"
globals
    [|UnknownType|] myGlobal
endglobals",
            };

            yield return new object[]
            {
                @"
function foo takes [|UnknownType|] x returns nothing
endfunction",
            };

            yield return new object[]
            {
                @"
function foo takes nothing returns [|UnknownType|]
    return null
endfunction",
            };

            yield return new object[]
            {
                @"
native foo takes [|UnknownType|] x returns nothing",
            };

            yield return new object[]
            {
                @"
native foo takes nothing returns [|UnknownType|]",
            };

            yield return new object[]
            {
                @"
globals
    [|UnknownType|] array myArray
endglobals",
            };
        }
    }
}
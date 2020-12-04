// ------------------------------------------------------------------------------
// <copyright file="IntegerTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this IntegerSyntax integerNode, ref StringBuilder sb)
        {
            _ = integerNode ?? throw new ArgumentNullException(nameof(integerNode));

            integerNode.FourCCIntegerNode?.Transpile(ref sb);
            integerNode.IntegerToken?.TranspileExpression(ref sb);
        }
    }
}
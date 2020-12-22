// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public string Transpile(BinaryOperatorSyntax binaryOperator, SyntaxTokenType left, SyntaxTokenType right)
        {
            _ = binaryOperator ?? throw new ArgumentNullException(nameof(binaryOperator));

            return TranspileBinaryOperator(binaryOperator.BinaryOperatorToken, left, right);
        }
    }
}
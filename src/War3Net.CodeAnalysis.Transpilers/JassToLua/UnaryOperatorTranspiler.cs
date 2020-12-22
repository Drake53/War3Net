// ------------------------------------------------------------------------------
// <copyright file="UnaryOperatorTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public string Transpile(UnaryOperatorSyntax unaryOperator)
        {
            _ = unaryOperator ?? throw new ArgumentNullException(nameof(unaryOperator));

            return TranspileUnaryOperator(unaryOperator.UnaryOperatorToken);
        }
    }
}
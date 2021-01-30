// ------------------------------------------------------------------------------
// <copyright file="UnaryOperatorTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public SyntaxKind Transpile(UnaryOperatorType unaryOperator)
        {
            return unaryOperator switch
            {
                UnaryOperatorType.Plus => SyntaxKind.UnaryPlusExpression,
                UnaryOperatorType.Minus => SyntaxKind.UnaryPlusExpression,
                UnaryOperatorType.Not => SyntaxKind.LogicalNotExpression,
            };
        }
    }
}
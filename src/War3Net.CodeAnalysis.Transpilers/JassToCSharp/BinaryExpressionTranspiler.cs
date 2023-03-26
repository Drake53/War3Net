// ------------------------------------------------------------------------------
// <copyright file="BinaryExpressionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public ExpressionSyntax Transpile(JassBinaryExpressionSyntax binaryExpression)
        {
            return SyntaxFactory.BinaryExpression(
                TranspileBinary(binaryExpression.OperatorToken.SyntaxKind),
                Transpile(binaryExpression.Left),
                Transpile(binaryExpression.Right));
        }
    }
}
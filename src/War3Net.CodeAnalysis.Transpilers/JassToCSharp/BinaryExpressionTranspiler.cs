// ------------------------------------------------------------------------------
// <copyright file="BinaryExpressionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public ExpressionSyntax Transpile(JassBinaryExpressionSyntax binaryExpression)
        {
            return SyntaxFactory.BinaryExpression(
                TranspileBinaryExpressionKind(binaryExpression.SyntaxKind),
                Transpile(binaryExpression.Left),
                Transpile(binaryExpression.Right));
        }

        public SyntaxKind TranspileBinaryExpressionKind(JassSyntaxKind expressionKind)
        {
            return expressionKind switch
            {
                JassSyntaxKind.AddExpression => SyntaxKind.AddExpression,
                JassSyntaxKind.SubtractExpression => SyntaxKind.SubtractExpression,
                JassSyntaxKind.MultiplyExpression => SyntaxKind.MultiplyExpression,
                JassSyntaxKind.DivideExpression => SyntaxKind.DivideExpression,
                JassSyntaxKind.GreaterThanExpression => SyntaxKind.GreaterThanExpression,
                JassSyntaxKind.LessThanExpression => SyntaxKind.LessThanExpression,
                JassSyntaxKind.EqualsExpression => SyntaxKind.EqualsExpression,
                JassSyntaxKind.NotEqualsExpression => SyntaxKind.NotEqualsExpression,
                JassSyntaxKind.GreaterThanOrEqualExpression => SyntaxKind.GreaterThanOrEqualExpression,
                JassSyntaxKind.LessThanOrEqualExpression => SyntaxKind.LessThanOrEqualExpression,
                JassSyntaxKind.LogicalAndExpression => SyntaxKind.LogicalAndExpression,
                JassSyntaxKind.LogicalOrExpression => SyntaxKind.LogicalOrExpression,
            };
        }
    }
}
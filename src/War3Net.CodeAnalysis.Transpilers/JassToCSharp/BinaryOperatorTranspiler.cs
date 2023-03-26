// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.CodeAnalysis.CSharp;

using War3Net.CodeAnalysis.Jass;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToCSharpTranspiler
    {
        public SyntaxKind TranspileBinary(JassSyntaxKind binaryOperatorTokenKind)
        {
            return binaryOperatorTokenKind switch
            {
                JassSyntaxKind.PlusToken => SyntaxKind.AddExpression,
                JassSyntaxKind.MinusToken => SyntaxKind.SubtractExpression,
                JassSyntaxKind.AsteriskToken => SyntaxKind.MultiplyExpression,
                JassSyntaxKind.SlashToken => SyntaxKind.DivideExpression,
                JassSyntaxKind.GreaterThanToken => SyntaxKind.GreaterThanExpression,
                JassSyntaxKind.LessThanToken => SyntaxKind.LessThanExpression,
                JassSyntaxKind.EqualsToken => SyntaxKind.EqualsExpression,
                JassSyntaxKind.ExclamationEqualsToken => SyntaxKind.NotEqualsExpression,
                JassSyntaxKind.GreaterThanEqualsToken => SyntaxKind.GreaterThanOrEqualExpression,
                JassSyntaxKind.LessThanEqualsToken => SyntaxKind.LessThanOrEqualExpression,
                JassSyntaxKind.AndKeyword => SyntaxKind.LogicalAndExpression,
                JassSyntaxKind.OrKeyword => SyntaxKind.LogicalOrExpression,
            };
        }
    }
}
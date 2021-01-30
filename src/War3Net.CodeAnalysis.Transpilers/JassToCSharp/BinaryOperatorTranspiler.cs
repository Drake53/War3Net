// ------------------------------------------------------------------------------
// <copyright file="BinaryOperatorTranspiler.cs" company="Drake53">
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
        public SyntaxKind Transpile(BinaryOperatorType binaryOperator)
        {
            return binaryOperator switch
            {
                BinaryOperatorType.Add => SyntaxKind.AddExpression,
                BinaryOperatorType.Subtract => SyntaxKind.SubtractExpression,
                BinaryOperatorType.Multiplication => SyntaxKind.MultiplyExpression,
                BinaryOperatorType.Division => SyntaxKind.DivideExpression,
                BinaryOperatorType.GreaterThan => SyntaxKind.GreaterThanExpression,
                BinaryOperatorType.LessThan => SyntaxKind.LessThanExpression,
                BinaryOperatorType.Equals => SyntaxKind.EqualsExpression,
                BinaryOperatorType.NotEquals => SyntaxKind.NotEqualsExpression,
                BinaryOperatorType.GreaterOrEqual => SyntaxKind.GreaterThanOrEqualExpression,
                BinaryOperatorType.LessOrEqual => SyntaxKind.LessThanOrEqualExpression,
                BinaryOperatorType.And => SyntaxKind.LogicalAndExpression,
                BinaryOperatorType.Or => SyntaxKind.LogicalOrExpression,
            };
        }
    }
}
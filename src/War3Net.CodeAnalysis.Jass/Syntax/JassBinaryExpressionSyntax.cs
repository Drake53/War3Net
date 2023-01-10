// ------------------------------------------------------------------------------
// <copyright file="JassBinaryExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassBinaryExpressionSyntax : JassExpressionSyntax
    {
        public JassBinaryExpressionSyntax(
            JassExpressionSyntax left,
            JassSyntaxToken operatorToken,
            JassExpressionSyntax right)
        {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }

        public JassExpressionSyntax Left { get; }

        public JassSyntaxToken OperatorToken { get; }

        public JassExpressionSyntax Right { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassBinaryExpressionSyntax binaryExpression
                && Left.IsEquivalentTo(binaryExpression.Left)
                && OperatorToken.IsEquivalentTo(binaryExpression.OperatorToken)
                && Right.IsEquivalentTo(binaryExpression.Right);
        }

        public override void WriteTo(TextWriter writer)
        {
            Left.WriteTo(writer);
            OperatorToken.WriteTo(writer);
            Right.WriteTo(writer);
        }

        public override string ToString() => $"{Left} {OperatorToken} {Right}";

        public override JassSyntaxToken GetFirstToken() => Left.GetFirstToken();

        public override JassSyntaxToken GetLastToken() => Right.GetLastToken();

        protected internal override JassBinaryExpressionSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassBinaryExpressionSyntax(
                Left.ReplaceFirstToken(newToken),
                OperatorToken,
                Right);
        }

        protected internal override JassBinaryExpressionSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassBinaryExpressionSyntax(
                Left,
                OperatorToken,
                Right.ReplaceLastToken(newToken));
        }
    }
}
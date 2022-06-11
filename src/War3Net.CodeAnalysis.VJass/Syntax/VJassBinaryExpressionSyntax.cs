// ------------------------------------------------------------------------------
// <copyright file="VJassBinaryExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassBinaryExpressionSyntax : VJassExpressionSyntax
    {
        public VJassBinaryExpressionSyntax(
            VJassExpressionSyntax left,
            VJassSyntaxToken operatorToken,
            VJassExpressionSyntax right)
        {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }

        public VJassExpressionSyntax Left { get; }

        public VJassSyntaxToken OperatorToken { get; }

        public VJassExpressionSyntax Right { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassBinaryExpressionSyntax binaryExpression
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

        public override VJassSyntaxToken GetFirstToken() => Left.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => Right.GetLastToken();

        protected internal override VJassBinaryExpressionSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassBinaryExpressionSyntax(
                Left.ReplaceFirstToken(newToken),
                OperatorToken,
                Right);
        }

        protected internal override VJassBinaryExpressionSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassBinaryExpressionSyntax(
                Left,
                OperatorToken,
                Right.ReplaceLastToken(newToken));
        }
    }
}
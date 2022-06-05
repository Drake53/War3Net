// ------------------------------------------------------------------------------
// <copyright file="VJassMemberAccessExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassMemberAccessExpressionSyntax : IExpressionSyntax
    {
        public VJassMemberAccessExpressionSyntax(
            IExpressionSyntax expression,
            VJassIdentifierNameSyntax memberName)
        {
            Expression = expression;
            MemberName = memberName;
        }

        public IExpressionSyntax Expression { get; }

        public VJassIdentifierNameSyntax MemberName { get; }

        public bool Equals(IExpressionSyntax? other)
        {
            return other is VJassMemberAccessExpressionSyntax memberAccessExpression
                && Expression.Equals(memberAccessExpression.Expression)
                && MemberName.Equals(memberAccessExpression.MemberName);
        }

        public override string ToString() => $"{Expression}{VJassSymbol.FullStop}{MemberName}";
    }
}
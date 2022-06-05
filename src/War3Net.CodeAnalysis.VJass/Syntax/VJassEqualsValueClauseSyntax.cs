// ------------------------------------------------------------------------------
// <copyright file="VJassEqualsValueClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassEqualsValueClauseSyntax : IEquatable<VJassEqualsValueClauseSyntax>
    {
        public VJassEqualsValueClauseSyntax(
            IExpressionSyntax expression)
        {
            Expression = expression;
        }

        public IExpressionSyntax Expression { get; }

        public bool Equals(VJassEqualsValueClauseSyntax? other)
        {
            return other is not null
                && Expression.Equals(other.Expression);
        }

        public override string ToString() => $"{VJassSymbol.EqualsSign} {Expression}";
    }
}
// ------------------------------------------------------------------------------
// <copyright file="JassEqualsValueClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassEqualsValueClauseSyntax : IEquatable<JassEqualsValueClauseSyntax>, IJassSyntaxToken
    {
        public JassEqualsValueClauseSyntax(IExpressionSyntax expression)
        {
            Expression = expression;
        }

        public IExpressionSyntax Expression { get; init; }

        public bool Equals(JassEqualsValueClauseSyntax? other)
        {
            return other is not null
                && Expression.Equals(other.Expression);
        }

        public override string ToString() => $"{JassSymbol.EqualsSign} {Expression}";
    }
}
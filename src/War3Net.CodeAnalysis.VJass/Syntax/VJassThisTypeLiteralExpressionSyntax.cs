// ------------------------------------------------------------------------------
// <copyright file="VJassThisTypeLiteralExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassThisTypeLiteralExpressionSyntax : IExpressionSyntax
    {
        public static readonly VJassThisTypeLiteralExpressionSyntax Value = new();

        private VJassThisTypeLiteralExpressionSyntax()
        {
        }

        public bool Equals(IExpressionSyntax? other) => other is VJassThisTypeLiteralExpressionSyntax;

        public override string ToString() => VJassKeyword.ThisType;
    }
}
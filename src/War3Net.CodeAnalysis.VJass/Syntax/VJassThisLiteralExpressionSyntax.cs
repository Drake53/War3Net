// ------------------------------------------------------------------------------
// <copyright file="VJassThisLiteralExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassThisLiteralExpressionSyntax : IExpressionSyntax
    {
        public static readonly VJassThisLiteralExpressionSyntax Value = new();

        private VJassThisLiteralExpressionSyntax()
        {
        }

        public bool Equals(IExpressionSyntax? other) => other is VJassThisLiteralExpressionSyntax;

        public override string ToString() => VJassKeyword.This;
    }
}
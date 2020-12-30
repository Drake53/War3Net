// ------------------------------------------------------------------------------
// <copyright file="JassStringLiteralExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassStringLiteralExpressionSyntax : IExpressionSyntax
    {
        public JassStringLiteralExpressionSyntax(string value)
        {
            Value = value;
        }

        public string Value { get; init; }

        public bool Equals(IExpressionSyntax? other) => other is JassStringLiteralExpressionSyntax e && Value == e.Value;

        public override string ToString() => $"\"{Value}\"";
    }
}
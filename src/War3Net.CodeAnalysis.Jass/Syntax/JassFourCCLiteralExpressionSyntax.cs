// ------------------------------------------------------------------------------
// <copyright file="JassFourCCLiteralExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Common.Extensions;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassFourCCLiteralExpressionSyntax : IExpressionSyntax
    {
        public JassFourCCLiteralExpressionSyntax(int value)
        {
            Value = value;
        }

        public int Value { get; init; }

        public bool Equals(IExpressionSyntax? other) => other is JassFourCCLiteralExpressionSyntax e && Value == e.Value;

        public override string ToString() => $"'{Value.ToRawcode()}'";
    }
}
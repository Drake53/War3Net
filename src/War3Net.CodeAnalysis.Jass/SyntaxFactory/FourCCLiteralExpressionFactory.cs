// ------------------------------------------------------------------------------
// <copyright file="FourCCLiteralExpressionFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassFourCCLiteralExpressionSyntax FourCCLiteralExpression(int value)
        {
            return new JassFourCCLiteralExpressionSyntax(
                (value & unchecked((int)0xFF000000)) >> 24 |
                (value & 0x00FF0000) >> 8 |
                (value & 0x0000FF00) << 8 |
                (value & 0x000000FF) << 24);
        }
    }
}
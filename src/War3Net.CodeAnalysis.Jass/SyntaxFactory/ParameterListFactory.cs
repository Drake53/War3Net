// ------------------------------------------------------------------------------
// <copyright file="ParameterListFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassParameterListSyntax ParameterList(params JassParameterSyntax[] parameters)
        {
            return new JassParameterListSyntax(
                Token(JassSyntaxKind.TakesKeyword),
                SeparatedSyntaxList(JassSyntaxKind.CommaToken, parameters));
        }
    }
}
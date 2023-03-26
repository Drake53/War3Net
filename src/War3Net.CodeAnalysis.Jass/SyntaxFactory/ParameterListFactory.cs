// ------------------------------------------------------------------------------
// <copyright file="ParameterListFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassParameterListOrEmptyParameterListSyntax ParameterList(params JassParameterSyntax[] parameters)
        {
            if (parameters.Length == 0)
            {
                return JassEmptyParameterListSyntax.Value;
            }

            return new JassParameterListSyntax(
                Token(JassSyntaxKind.TakesKeyword),
                SeparatedSyntaxList(JassSyntaxKind.CommaToken, parameters));
        }

        public static JassParameterListOrEmptyParameterListSyntax ParameterList(IEnumerable<JassParameterSyntax> parameters)
        {
            if (!parameters.Any())
            {
                return JassEmptyParameterListSyntax.Value;
            }

            return new JassParameterListSyntax(
                Token(JassSyntaxKind.TakesKeyword),
                SeparatedSyntaxList(JassSyntaxKind.CommaToken, parameters));
        }
    }
}
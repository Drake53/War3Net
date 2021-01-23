// ------------------------------------------------------------------------------
// <copyright file="TypeParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassTypeSyntax> GetTypeParser(Parser<char, JassIdentifierNameSyntax> identifierNameParser)
        {
            return OneOf(
                Keyword.Code.ThenReturn(JassTypeSyntax.Code),
                Keyword.Handle.ThenReturn(JassTypeSyntax.Handle),
                Keyword.Integer.ThenReturn(JassTypeSyntax.Integer),
                Keyword.Real.ThenReturn(JassTypeSyntax.Real),
                Keyword.Boolean.ThenReturn(JassTypeSyntax.Boolean),
                Keyword.String.ThenReturn(JassTypeSyntax.String),
                identifierNameParser.Map(id => new JassTypeSyntax(id)))
                .Labelled("type");
        }
    }
}
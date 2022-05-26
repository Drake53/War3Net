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
        internal static Parser<char, JassTypeSyntax> GetTypeParser(
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, Unit> whitespaceParser)
        {
            return OneOf(
                Keyword.Code.Then(whitespaceParser).ThenReturn(JassTypeSyntax.Code),
                Keyword.Handle.Then(whitespaceParser).ThenReturn(JassTypeSyntax.Handle),
                Keyword.Integer.Then(whitespaceParser).ThenReturn(JassTypeSyntax.Integer),
                Keyword.Real.Then(whitespaceParser).ThenReturn(JassTypeSyntax.Real),
                Keyword.Boolean.Then(whitespaceParser).ThenReturn(JassTypeSyntax.Boolean),
                Keyword.String.Then(whitespaceParser).ThenReturn(JassTypeSyntax.String),
                identifierNameParser.Map(id => new JassTypeSyntax(id)))
                .Labelled("type");
        }
    }
}
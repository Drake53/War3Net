// ------------------------------------------------------------------------------
// <copyright file="IdentifierNameParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, string> GetIdentifierParser()
        {
            return Try(Token(c => char.IsLetterOrDigit(c) || c == '_').AtLeastOnceString().Assert(value => !char.IsDigit(value[0])))
                .Assert(JassSyntaxFacts.IsValidIdentifier, value => $"'{value}' is not a valid identifier name");
        }

        internal static Parser<char, JassIdentifierNameSyntax> GetIdentifierNameParser(
            Parser<char, string> identifierParser,
            Parser<char, JassSyntaxTriviaList> triviaParser)
        {
            return identifierParser
                .AsToken(triviaParser, JassSyntaxKind.IdentifierToken)
                .Map(token => new JassIdentifierNameSyntax(token))
                .Labelled("identifier name");
        }
    }
}
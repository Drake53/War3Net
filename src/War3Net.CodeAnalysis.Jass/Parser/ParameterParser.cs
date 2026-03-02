using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;
using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassParameterSyntax> GetParameterParser(
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, JassTypeSyntax> typeParser)
        {
            return Map(
                (type, identifierName) => new JassParameterSyntax(type, identifierName),
                typeParser,
                identifierNameParser.Labelled("parameter identifier name"));
        }
    }
}
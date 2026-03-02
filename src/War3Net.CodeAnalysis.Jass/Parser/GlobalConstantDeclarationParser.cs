using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassGlobalDeclarationSyntax> GetGlobalConstantDeclarationParser(
            Parser<char, JassEqualsValueClauseSyntax> equalsValueClauseParser,
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, JassTypeSyntax> typeParser,
            Parser<char, JassSyntaxTriviaList> triviaParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map(
                (constantToken, type, identifierName, value, trailingTrivia) => (JassGlobalDeclarationSyntax)new JassGlobalConstantDeclarationSyntax(
                    constantToken,
                    type,
                    identifierName,
                    value.AppendTrailingTrivia(trailingTrivia)),
                Keyword.Constant.AsToken(triviaParser, JassSyntaxKind.ConstantKeyword),
                typeParser,
                identifierNameParser,
                equalsValueClauseParser,
                trailingTriviaParser);
        }
    }
}
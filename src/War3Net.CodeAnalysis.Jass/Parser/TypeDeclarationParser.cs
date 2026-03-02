using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassTopLevelDeclarationSyntax> GetTypeDeclarationParser(
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, JassTypeSyntax> typeParser,
            Parser<char, JassSyntaxTriviaList> triviaParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map(
                (typeToken, identifierName, extendsToken, type, trailingTrivia) => (JassTopLevelDeclarationSyntax)new JassTypeDeclarationSyntax(
                    typeToken,
                    identifierName,
                    extendsToken,
                    type.AppendTrailingTrivia(trailingTrivia)),
                Keyword.Type.AsToken(triviaParser, JassSyntaxKind.TypeKeyword),
                identifierNameParser,
                Keyword.Extends.AsToken(triviaParser, JassSyntaxKind.ExtendsKeyword),
                typeParser,
                trailingTriviaParser);
        }
    }
}
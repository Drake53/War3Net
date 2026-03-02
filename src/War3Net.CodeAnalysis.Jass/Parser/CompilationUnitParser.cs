using static Pidgin.Parser<char>;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassCompilationUnitSyntax> GetCompilationUnitParser(
            Parser<char, JassTopLevelDeclarationSyntax> declarationParser,
            Parser<char, JassSyntaxTriviaList> leadingTriviaParser)
        {
            return declarationParser.UntilWithLeading(
                leadingTriviaParser,
                leadingTriviaParser,
                End,
                (leadingTrivia, declaration) => declaration.WithLeadingTrivia(leadingTrivia),
                (firstTrivia, declarations, lastTrivia, _) => new JassCompilationUnitSyntax(
                    declarations.ToImmutableArray(),
                    new JassSyntaxToken(lastTrivia, JassSyntaxKind.EndOfFileToken, string.Empty, JassSyntaxTriviaList.Empty)).WithLeadingTrivia(firstTrivia));
        }
    }
}
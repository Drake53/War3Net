using Pidgin;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;
using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassGlobalDeclarationSyntax> GetGlobalVariableDeclarationParser(
            Parser<char, JassVariableOrArrayDeclaratorSyntax> variableOrArrayDeclaratorParser,
            Parser<char, JassSyntaxTriviaList> trailingTriviaParser)
        {
            return Map(
                (declarator, trailingTrivia) => (JassGlobalDeclarationSyntax)new JassGlobalVariableDeclarationSyntax(declarator.AppendTrailingTrivia(trailingTrivia)),
                variableOrArrayDeclaratorParser,
                trailingTriviaParser);
        }
    }
}
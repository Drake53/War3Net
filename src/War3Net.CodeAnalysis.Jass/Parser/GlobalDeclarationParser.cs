using Pidgin;
using War3Net.CodeAnalysis.Jass.Syntax;
using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassGlobalDeclarationSyntax> GetGlobalDeclarationParser(
            Parser<char, JassGlobalDeclarationSyntax> globalConstantDeclarationParser,
            Parser<char, JassGlobalDeclarationSyntax> globalVariableDeclarationParser)
        {
            return OneOf(
                globalConstantDeclarationParser,
                globalVariableDeclarationParser);
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="DeclarationParser.cs" company="Drake53">
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
        internal static Parser<char, IDeclarationSyntax> GetDeclarationParser(
            Parser<char, IDeclarationSyntax> emptyDeclarationParser,
            Parser<char, IDeclarationSyntax> commentDeclarationParser,
            Parser<char, IDeclarationSyntax> globalDeclarationParser,
            Parser<char, JassFunctionDeclaratorSyntax> functionDeclaratorParser,
            Parser<char, JassIdentifierNameSyntax> identifierNameParser,
            Parser<char, JassStatementListSyntax> statementListParser,
            Parser<char, JassTypeSyntax> typeParser,
            Parser<char, Unit> endOfLineParser)
        {
            return OneOf(
                emptyDeclarationParser,
                commentDeclarationParser,
                GetTypeDeclarationParser(identifierNameParser, typeParser),
                GetGlobalDeclarationListParser(globalDeclarationParser, endOfLineParser),
                GetNativeFunctionDeclarationParser(functionDeclaratorParser),
                GetFunctionDeclarationParser(functionDeclaratorParser, statementListParser, endOfLineParser));
        }
    }
}
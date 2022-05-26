// ------------------------------------------------------------------------------
// <copyright file="FunctionDeclarationParser.cs" company="Drake53">
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
        internal static Parser<char, JassFunctionDeclarationSyntax> GetFunctionDeclarationParser(
            Parser<char, JassFunctionDeclaratorSyntax> functionDeclaratorParser,
            Parser<char, JassStatementListSyntax> statementListParser,
            Parser<char, Unit> whitespaceParser,
            Parser<char, Unit> endOfLineParser)
        {
            return Map(
                (declarator, body) => new JassFunctionDeclarationSyntax(declarator, body),
                Keyword.Constant.Then(whitespaceParser).Optional().Then(Keyword.Function.Then(whitespaceParser)).Then(functionDeclaratorParser).Before(endOfLineParser),
                statementListParser.Before(Keyword.EndFunction.Then(whitespaceParser)));
        }
    }
}
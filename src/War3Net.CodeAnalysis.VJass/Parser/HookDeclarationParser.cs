// ------------------------------------------------------------------------------
// <copyright file="HookDeclarationParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.VJass.Extensions;
using War3Net.CodeAnalysis.VJass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, VJassHookDeclarationSyntax> GetHookDeclarationParser(
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser,
            Parser<char, VJassExpressionSyntax> expressionParser,
            Parser<char, VJassSyntaxTriviaList> triviaParser)
        {
            return Map(
                (hookToken, hookedFunction, expression) => new VJassHookDeclarationSyntax(
                    hookToken,
                    hookedFunction,
                    expression),
                Keyword.Hook.AsToken(triviaParser, VJassSyntaxKind.HookKeyword),
                identifierNameParser.Labelled("hooked function identifier name"),
                expressionParser);
        }
    }
}
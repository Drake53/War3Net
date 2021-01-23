// ------------------------------------------------------------------------------
// <copyright file="ArrayReferenceExpressionParser.cs" company="Drake53">
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
        internal static Parser<char, IExpressionSyntax> GetArrayReferenceExpressionParser(
            Parser<char, IExpressionSyntax> expressionParser,
            Parser<char, JassIdentifierNameSyntax> identifierNameParser)
        {
            return Try(identifierNameParser.Before(Symbol.LeftSquareBracket))
                .Then(expressionParser, (id, indexer) => (IExpressionSyntax)new JassArrayReferenceExpressionSyntax(id, indexer))
                .Before(Symbol.RightSquareBracket);
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="ArgumentListParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassArgumentListSyntax> GetArgumentListParser(
            Parser<char, Unit> whitespaceParser,
            Parser<char, IExpressionSyntax> expressionParser)
        {
            return expressionParser.Separated(Symbol.Comma.Before(whitespaceParser))
                .Select(arguments => new JassArgumentListSyntax(arguments.ToImmutableArray()));
        }
    }
}
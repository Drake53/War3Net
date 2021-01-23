// ------------------------------------------------------------------------------
// <copyright file="CompilationUnitParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, JassCompilationUnitSyntax> GetCompilationUnitParser(
            Parser<char, IDeclarationSyntax> declarationParser,
            Parser<char, string> commentParser,
            Parser<char, Unit> newlineParser)
        {
            return declarationParser.Before(commentParser.Optional().Then(newlineParser.Or(Lookahead(End)))).Many()
                .Select(declarations => new JassCompilationUnitSyntax(declarations.ToImmutableArray()))
                .Before(End);
        }
    }
}
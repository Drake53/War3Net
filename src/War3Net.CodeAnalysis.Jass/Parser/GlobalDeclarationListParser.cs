// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationListParser.cs" company="Drake53">
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
        internal static Parser<char, ITopLevelDeclarationSyntax> GetGlobalDeclarationListParser(
            Parser<char, IGlobalDeclarationSyntax> globalDeclarationParser,
            Parser<char, Unit> whitespaceParser,
            Parser<char, Unit> endOfLineParser)
        {
            return Keyword.Globals.Then(whitespaceParser).Then(endOfLineParser).Then(globalDeclarationParser.Many()).Before(Keyword.EndGlobals.Then(whitespaceParser))
                .Select<ITopLevelDeclarationSyntax>(globals => new JassGlobalDeclarationListSyntax(globals.ToImmutableArray()));
        }
    }
}
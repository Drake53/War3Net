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
        internal static Parser<char, IDeclarationSyntax> GetGlobalDeclarationListParser(
            Parser<char, IGlobalDeclarationSyntax> globalDeclarationParser,
            Parser<char, Unit> endOfLineParser)
        {
            return Keyword.Globals.Then(endOfLineParser).Then(globalDeclarationParser.Many()).Before(Keyword.EndGlobals)
                .Select<IDeclarationSyntax>(globals => new JassGlobalDeclarationListSyntax(globals.ToImmutableArray()));
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="EndOfLineParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, Unit> GetEndOfLineParser(
            Parser<char, string> commentStringParser,
            Parser<char, Unit> newLineParser)
        {
            return commentStringParser.Optional().Then(newLineParser);
        }
    }
}
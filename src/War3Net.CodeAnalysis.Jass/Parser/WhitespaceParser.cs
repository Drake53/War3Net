// ------------------------------------------------------------------------------
// <copyright file="WhitespaceParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using static Pidgin.Parser<char>;

namespace War3Net.CodeAnalysis.Jass
{
    internal partial class JassParser
    {
        internal static Parser<char, Unit> GetWhitespaceParser()
        {
            return Token(JassSyntaxFacts.IsWhitespaceCharacter).SkipMany();
        }
    }
}
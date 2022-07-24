// ------------------------------------------------------------------------------
// <copyright file="ParameterParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Pidgin;

using War3Net.CodeAnalysis.VJass.Syntax;

using static Pidgin.Parser;

namespace War3Net.CodeAnalysis.VJass
{
    internal partial class VJassParser
    {
        internal static Parser<char, VJassParameterSyntax> GetParameterParser(
            Parser<char, VJassIdentifierNameSyntax> identifierNameParser,
            Parser<char, VJassTypeSyntax> typeParser)
        {
            return Map(
                (type, id) => new VJassParameterSyntax(type, id),
                typeParser,
                identifierNameParser.Labelled("parameter identifier name"));
        }
    }
}
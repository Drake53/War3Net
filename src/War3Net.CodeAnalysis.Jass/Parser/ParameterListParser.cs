// ------------------------------------------------------------------------------
// <copyright file="ParameterListParser.cs" company="Drake53">
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
        internal static Parser<char, JassParameterListSyntax> GetParameterListParser(Parser<char, JassParameterSyntax> parameterParser)
        {
            return parameterParser.Separated(Symbol.Comma)
                .Select(parameters => new JassParameterListSyntax(parameters.ToImmutableArray()));
        }
    }
}
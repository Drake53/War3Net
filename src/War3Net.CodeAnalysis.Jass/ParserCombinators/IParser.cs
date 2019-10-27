// ------------------------------------------------------------------------------
// <copyright file="IParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass
{
    // http://jass.sourceforge.net/doc/bnf.shtml
    // https://en.wikipedia.org/wiki/Left_recursion#Removing_left_recursion
    internal interface IParser
    {
        IEnumerable<ParseResult> Parse(ParseState state);
    }
}
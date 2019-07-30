// ------------------------------------------------------------------------------
// <copyright file="IParser.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
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
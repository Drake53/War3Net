// ------------------------------------------------------------------------------
// <copyright file="EmptyParser.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass
{
    /// <summary>
    /// An atomic parser that consumes no tokens and always succeeds.
    /// </summary>
    internal class EmptyParser : IParser
    {
        private static readonly Lazy<EmptyParser> _emptyParser = new Lazy<EmptyParser>(() => new EmptyParser());

        private EmptyParser()
        {
        }

        public static EmptyParser Get => _emptyParser.Value;

        public IEnumerable<ParseResult> Parse(ParseState state)
        {
            yield return new ParseResult(new EmptyNode(state.Position), 0);
        }
    }
}
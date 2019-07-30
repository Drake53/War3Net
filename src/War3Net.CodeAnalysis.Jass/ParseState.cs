// ------------------------------------------------------------------------------
// <copyright file="ParseState.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass
{
    internal class ParseState
    {
        public int Position;
        public List<SyntaxToken> Tokens; // TODO: make static?
    }
}
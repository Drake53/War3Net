// ------------------------------------------------------------------------------
// <copyright file="ParseState.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
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
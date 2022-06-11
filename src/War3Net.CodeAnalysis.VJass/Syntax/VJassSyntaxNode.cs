// ------------------------------------------------------------------------------
// <copyright file="VJassSyntaxNode.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public abstract class VJassSyntaxNode
    {
        internal VJassSyntaxNode()
        {
        }

        /// <summary>
        /// Determines if two nodes are the same, disregarding trivia differences.
        /// </summary>
        public abstract bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other);

        public abstract void WriteTo(TextWriter writer);

        public abstract VJassSyntaxToken GetFirstToken();

        public abstract VJassSyntaxToken GetLastToken();

        protected internal abstract VJassSyntaxNode ReplaceFirstToken(VJassSyntaxToken newToken);

        protected internal abstract VJassSyntaxNode ReplaceLastToken(VJassSyntaxToken newToken);
    }
}
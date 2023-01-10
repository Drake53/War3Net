// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxNode.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public abstract class JassSyntaxNode
    {
        internal JassSyntaxNode()
        {
        }

        /// <summary>
        /// Determines if two nodes are the same, disregarding trivia differences.
        /// </summary>
        public abstract bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other);

        public abstract void WriteTo(TextWriter writer);

        public abstract JassSyntaxToken GetFirstToken();

        public abstract JassSyntaxToken GetLastToken();

        protected internal abstract JassSyntaxNode ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal abstract JassSyntaxNode ReplaceLastToken(JassSyntaxToken newToken);
    }
}
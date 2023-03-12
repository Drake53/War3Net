// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxNode.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using OneOf;

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

        public abstract IEnumerable<JassSyntaxNode> GetChildNodes();

        public abstract IEnumerable<JassSyntaxToken> GetChildTokens();

        public abstract IEnumerable<OneOf<JassSyntaxNode, JassSyntaxToken>> GetChildNodesAndTokens();

        public abstract IEnumerable<JassSyntaxNode> GetDescendantNodes();

        public abstract IEnumerable<JassSyntaxToken> GetDescendantTokens();

        public abstract IEnumerable<OneOf<JassSyntaxNode, JassSyntaxToken>> GetDescendantNodesAndTokens();

        public abstract JassSyntaxToken GetFirstToken();

        public abstract JassSyntaxToken GetLastToken();

        protected internal abstract JassSyntaxNode ReplaceFirstToken(JassSyntaxToken newToken);

        protected internal abstract JassSyntaxNode ReplaceLastToken(JassSyntaxToken newToken);
    }
}
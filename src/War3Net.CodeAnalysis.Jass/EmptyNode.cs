// ------------------------------------------------------------------------------
// <copyright file="EmptyNode.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass
{
    /// <summary>
    /// A node for which <see cref="SyntaxNode.Span.Length"/> is zero.
    /// </summary>
    public sealed class EmptyNode : SyntaxNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyNode"/> class.
        /// </summary>
        public EmptyNode(int position)
            : base(position, true)
        {
        }
    }
}
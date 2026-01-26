// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxWalker.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    /// <summary>
    /// Represents a <see cref="JassSyntaxVisitor"/> that descends an entire <see cref="JassSyntaxNode"/> tree
    /// visiting each <see cref="JassSyntaxNode"/> and its child nodes in depth-first order.
    /// </summary>
    public abstract class JassSyntaxWalker : JassSyntaxVisitor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JassSyntaxWalker"/> class.
        /// </summary>
        protected JassSyntaxWalker()
        {
        }

        /// <inheritdoc/>
        public override void DefaultVisit(JassSyntaxNode node)
        {
            foreach (var child in node.GetChildNodes())
            {
                Visit(child);
            }
        }
    }
}
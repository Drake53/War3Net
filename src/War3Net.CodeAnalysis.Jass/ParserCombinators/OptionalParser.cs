// ------------------------------------------------------------------------------
// <copyright file="OptionalParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass
{
    /// <summary>
    /// A parser combinator that implements the unary ? operator.
    /// </summary>
    internal sealed class OptionalParser : AlternativeParser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionalParser"/> class.
        /// </summary>
        public OptionalParser(IParser baseParser)
            : base(baseParser, EmptyParser.Get)
        {
        }

        protected override SyntaxNode CreateNode(SyntaxNode node)
        {
            return node;
        }
    }
}
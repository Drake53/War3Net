// ------------------------------------------------------------------------------
// <copyright file="EmptyNode.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

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

        public override void WriteTo(StreamWriter streamWriter)
        {
        }
    }
}
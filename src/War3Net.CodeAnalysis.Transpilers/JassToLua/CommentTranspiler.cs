// ------------------------------------------------------------------------------
// <copyright file="CommentTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this CommentSyntax commentNode, ref StringBuilder sb)
        {
            _ = commentNode ?? throw new ArgumentNullException(nameof(commentNode));

            sb.Append("--");
            if (commentNode.EmptyCommentNode is null)
            {
                sb.Append(commentNode.CommentNode.ValueText);
            }

            sb.AppendLine();
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="CommentTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Text;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static string Transpile(this Syntax.CommentSyntax commentNode)
        {
            _ = commentNode ?? throw new ArgumentNullException(nameof(commentNode));

            return $"{commentNode.DoubleForwardSlashToken}{commentNode.CommentNode}{commentNode.NewlineToken}";
        }
    }

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.CommentSyntax commentNode, ref StringBuilder sb)
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
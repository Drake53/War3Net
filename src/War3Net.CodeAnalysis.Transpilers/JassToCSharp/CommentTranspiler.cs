// ------------------------------------------------------------------------------
// <copyright file="CommentTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static string Transpile(this Jass.Syntax.CommentSyntax commentNode)
        {
            _ = commentNode ?? throw new ArgumentNullException(nameof(commentNode));

            return $"{commentNode.DoubleForwardSlashToken}{commentNode.CommentNode}{commentNode.NewlineToken}";
        }
    }
}
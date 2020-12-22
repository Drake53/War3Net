// ------------------------------------------------------------------------------
// <copyright file="CommentTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public LuaStatementSyntax Transpile(CommentSyntax comment)
        {
            _ = comment ?? throw new ArgumentNullException(nameof(comment));

            return new LuaShortCommentStatement(comment.CommentNode?.ValueText ?? string.Empty);
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="SyntaxTokenTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        private const string AntiReservedKeywordConflictPrefix = "_";

        private static readonly Lazy<HashSet<string>> _reservedKeywords = new Lazy<HashSet<string>>(() => GetLuaKeywords().ToHashSet(StringComparer.Ordinal));

        public string Transpile(JassSyntaxToken token)
        {
            return _reservedKeywords.Value.Contains(token.Text)
                ? $"{AntiReservedKeywordConflictPrefix}{token.Text}"
                : token.Text;
        }

        private static IEnumerable<string> GetLuaKeywords()
        {
            yield return LuaSyntaxNode.Keyword.And;
            yield return LuaSyntaxNode.Keyword.Break;
            yield return LuaSyntaxNode.Keyword.Do;
            yield return LuaSyntaxNode.Keyword.Else;
            yield return LuaSyntaxNode.Keyword.ElseIf;
            yield return LuaSyntaxNode.Keyword.End;
            yield return LuaSyntaxNode.Keyword.False;
            yield return LuaSyntaxNode.Keyword.For;
            yield return LuaSyntaxNode.Keyword.Function;
            yield return LuaSyntaxNode.Keyword.Goto;
            yield return LuaSyntaxNode.Keyword.If;
            yield return LuaSyntaxNode.Keyword.In;
            yield return LuaSyntaxNode.Keyword.Local;
            yield return LuaSyntaxNode.Keyword.Nil;
            yield return LuaSyntaxNode.Keyword.Not;
            yield return LuaSyntaxNode.Keyword.Or;
            yield return LuaSyntaxNode.Keyword.Repeat;
            yield return LuaSyntaxNode.Keyword.Return;
            yield return LuaSyntaxNode.Keyword.Then;
            yield return LuaSyntaxNode.Keyword.True;
            yield return LuaSyntaxNode.Keyword.Until;
            yield return LuaSyntaxNode.Keyword.While;
        }
    }
}
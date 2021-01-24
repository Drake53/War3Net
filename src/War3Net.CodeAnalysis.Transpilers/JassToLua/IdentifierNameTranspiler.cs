// ------------------------------------------------------------------------------
// <copyright file="IdentifierNameTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        private const string AntiReservedKeywordConflictPrefix = "_";

        private static readonly Lazy<HashSet<string>> _reservedKeywords = new Lazy<HashSet<string>>(() => GetLuaKeywords().ToHashSet(StringComparer.Ordinal));

        public string Transpile(JassIdentifierNameSyntax identifierName)
        {
            return _reservedKeywords.Value.Contains(identifierName.Name)
                ? $"{AntiReservedKeywordConflictPrefix}{identifierName.Name}"
                : identifierName.Name;
        }

        private static IEnumerable<string> GetLuaKeywords()
        {
            yield return "and";
            yield return "break";
            yield return "do";
            yield return "else";
            yield return "elseif";
            yield return "end";
            yield return "false";
            yield return "for";
            yield return "function";
            yield return "goto";
            yield return "if";
            yield return "in";
            yield return "local";
            yield return "nil";
            yield return "not";
            yield return "or";
            yield return "repeat";
            yield return "return";
            yield return "then";
            yield return "true";
            yield return "until";
            yield return "while";
        }
    }
}
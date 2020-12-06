// ------------------------------------------------------------------------------
// <copyright file="ElseClauseTranspiler.cs" company="Drake53">
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
        [Obsolete]
        public static void Transpile(this ElseClauseSyntax elseClauseNode, ref StringBuilder sb)
        {
            _ = elseClauseNode ?? throw new ArgumentNullException(nameof(elseClauseNode));

            elseClauseNode.ElseifNode?.Transpile(ref sb);
            elseClauseNode.ElseNode?.Transpile(ref sb);
        }
    }
}
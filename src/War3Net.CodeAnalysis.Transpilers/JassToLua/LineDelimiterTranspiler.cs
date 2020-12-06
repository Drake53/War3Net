// ------------------------------------------------------------------------------
// <copyright file="LineDelimiterTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        [Obsolete]
        public static void Transpile(this LineDelimiterSyntax lineDelimiterNode, ref StringBuilder sb)
        {
            _ = lineDelimiterNode ?? throw new ArgumentNullException(nameof(lineDelimiterNode));

            foreach (var eolNode in lineDelimiterNode)
            {
                eolNode.Transpile(ref sb);
            }
        }

        public static IEnumerable<LuaStatementSyntax> TranspileToLua(this LineDelimiterSyntax lineDelimiterNode)
        {
            _ = lineDelimiterNode ?? throw new ArgumentNullException(nameof(lineDelimiterNode));

            return lineDelimiterNode.Select(eolNode => eolNode.TranspileToLua());
        }
    }
}
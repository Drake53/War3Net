// ------------------------------------------------------------------------------
// <copyright file="EndOfLineTranspiler.cs" company="Drake53">
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
        public static void Transpile(this EndOfLineSyntax endOfLineNode, ref StringBuilder sb)
        {
            _ = endOfLineNode ?? throw new ArgumentNullException(nameof(endOfLineNode));

            if (endOfLineNode.NewlineToken is null)
            {
                endOfLineNode.Comment.Transpile(ref sb);
            }
            else
            {
                sb.AppendLine();
            }
        }
    }
}
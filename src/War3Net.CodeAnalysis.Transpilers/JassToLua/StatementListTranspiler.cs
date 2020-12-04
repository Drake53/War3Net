// ------------------------------------------------------------------------------
// <copyright file="StatementListTranspiler.cs" company="Drake53">
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
        public static void Transpile(this StatementListSyntax statementListNode, ref StringBuilder sb)
        {
            _ = statementListNode ?? throw new ArgumentNullException(nameof(statementListNode));

            foreach (var statementNode in statementListNode)
            {
                statementNode.StatementNode.Transpile(ref sb);
                statementNode.LineDelimiterNode.Transpile(ref sb);

                if (statementNode.StatementNode.ReturnStatementNode != null)
                {
                    break;
                }
            }
        }
    }
}
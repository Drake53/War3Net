// ------------------------------------------------------------------------------
// <copyright file="StatementListTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable SA1402 // File may only contain a single type
#pragma warning disable SA1649 // File name should match first type name

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Jass.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static IEnumerable<StatementSyntax> Transpile(this Syntax.StatementListSyntax statementListNode)
        {
            _ = statementListNode ?? throw new ArgumentNullException(nameof(statementListNode));

            return statementListNode.Select(statement => statement.StatementNode.Transpile());
        }
    }

    public static partial class JassToLuaTranspiler
    {
        public static void Transpile(this Syntax.StatementListSyntax statementListNode, ref StringBuilder sb)
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
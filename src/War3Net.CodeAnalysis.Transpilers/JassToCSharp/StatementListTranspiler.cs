// ------------------------------------------------------------------------------
// <copyright file="StatementListTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToCSharpTranspiler
    {
        public static IEnumerable<StatementSyntax> Transpile(this Jass.Syntax.StatementListSyntax statementListNode)
        {
            _ = statementListNode ?? throw new ArgumentNullException(nameof(statementListNode));

            return statementListNode.Select(statement => statement.StatementNode.Transpile());
        }
    }
}
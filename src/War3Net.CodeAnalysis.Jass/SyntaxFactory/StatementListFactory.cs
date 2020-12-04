// ------------------------------------------------------------------------------
// <copyright file="StatementListFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public static partial class JassSyntaxFactory
    {
        public static StatementListSyntax StatementList(params NewStatementSyntax[] statements)
        {
            return statements.Any() ? new StatementListSyntax(statements) : new StatementListSyntax(Empty());
        }
    }
}
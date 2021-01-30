// ------------------------------------------------------------------------------
// <copyright file="StatementListFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassStatementListSyntax StatementList(IEnumerable<IStatementSyntax> statements)
        {
            return new JassStatementListSyntax(statements.ToImmutableArray());
        }

        public static JassStatementListSyntax StatementList(params IStatementSyntax[] statements)
        {
            return new JassStatementListSyntax(statements.ToImmutableArray());
        }
    }
}
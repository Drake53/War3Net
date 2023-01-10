// ------------------------------------------------------------------------------
// <copyright file="JassStatementListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    [Obsolete]
    public class JassStatementListSyntax : IEquatable<JassStatementListSyntax>
    {
        public JassStatementListSyntax(ImmutableArray<IStatementSyntax> statements)
        {
            Statements = statements;
        }

        public ImmutableArray<IStatementSyntax> Statements { get; init; }

        public bool Equals(JassStatementListSyntax? other)
        {
            return other is not null
                && Statements.SequenceEqual(other.Statements);
        }

        public override string ToString() => $"<{base.ToString()}> [{Statements.Length}]";
    }
}
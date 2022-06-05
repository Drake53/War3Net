// ------------------------------------------------------------------------------
// <copyright file="VJassStatementListSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Linq;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassStatementListSyntax : IEquatable<VJassStatementListSyntax>
    {
        public VJassStatementListSyntax(ImmutableArray<IStatementSyntax> statements)
        {
            Statements = statements;
        }

        public ImmutableArray<IStatementSyntax> Statements { get; }

        public bool Equals(VJassStatementListSyntax? other)
        {
            return other is not null
                && Statements.SequenceEqual(other.Statements);
        }

        public override string ToString() => $"<{base.ToString()}> [{Statements.Length}]";
    }
}
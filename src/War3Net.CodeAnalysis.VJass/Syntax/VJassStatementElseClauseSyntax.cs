// ------------------------------------------------------------------------------
// <copyright file="VJassStatementElseClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassStatementElseClauseSyntax : IEquatable<VJassStatementElseClauseSyntax>
    {
        public VJassStatementElseClauseSyntax(
            VJassStatementListSyntax body)
        {
            Body = body;
        }

        public VJassStatementListSyntax Body { get; }

        public bool Equals(VJassStatementElseClauseSyntax? other)
        {
            return other is not null
                && Body.Equals(other.Body);
        }

        public override string ToString() => $"{VJassKeyword.Else} [{Body.Statements.Length}]";
    }
}
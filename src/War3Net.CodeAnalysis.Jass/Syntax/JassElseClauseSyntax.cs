// ------------------------------------------------------------------------------
// <copyright file="JassElseClauseSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassElseClauseSyntax : IEquatable<JassElseClauseSyntax>, IJassSyntaxToken
    {
        public JassElseClauseSyntax(JassStatementListSyntax body)
        {
            Body = body;
        }

        public JassStatementListSyntax Body { get; init; }

        public bool Equals(JassElseClauseSyntax? other)
        {
            return other is not null
                && Body.Equals(other.Body);
        }

        public override string ToString() => $"{JassKeyword.Else} [{Body.Statements.Length}]";
    }
}
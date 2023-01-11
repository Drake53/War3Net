// ------------------------------------------------------------------------------
// <copyright file="JassSetStatementSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassSetStatementSyntax : JassStatementSyntax
    {
        internal JassSetStatementSyntax(
            JassSyntaxToken setToken,
            JassIdentifierNameSyntax identifierName,
            JassElementAccessClauseSyntax? elementAccessClause,
            JassEqualsValueClauseSyntax value)
        {
            SetToken = setToken;
            IdentifierName = identifierName;
            ElementAccessClause = elementAccessClause;
            Value = value;
        }

        public JassSyntaxToken SetToken { get; }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public JassElementAccessClauseSyntax? ElementAccessClause { get; }

        public JassEqualsValueClauseSyntax Value { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassSetStatementSyntax setStatement
                && IdentifierName.IsEquivalentTo(setStatement.IdentifierName)
                && ElementAccessClause.NullableEquivalentTo(setStatement.ElementAccessClause)
                && Value.IsEquivalentTo(setStatement.Value);
        }

        public override void WriteTo(TextWriter writer)
        {
            SetToken.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            ElementAccessClause?.WriteTo(writer);
            Value.WriteTo(writer);
        }

        public override string ToString() => $"{SetToken} {IdentifierName}{ElementAccessClause.Optional()} {Value}";

        public override JassSyntaxToken GetFirstToken() => SetToken;

        public override JassSyntaxToken GetLastToken() => Value.GetLastToken();

        protected internal override JassSetStatementSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassSetStatementSyntax(
                newToken,
                IdentifierName,
                ElementAccessClause,
                Value);
        }

        protected internal override JassSetStatementSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassSetStatementSyntax(
                SetToken,
                IdentifierName,
                ElementAccessClause,
                Value.ReplaceLastToken(newToken));
        }
    }
}
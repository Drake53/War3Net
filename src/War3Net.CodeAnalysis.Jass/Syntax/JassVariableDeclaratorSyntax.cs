// ------------------------------------------------------------------------------
// <copyright file="JassVariableDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassVariableDeclaratorSyntax : JassVariableOrArrayDeclaratorSyntax
    {
        internal JassVariableDeclaratorSyntax(
            JassTypeSyntax type,
            JassIdentifierNameSyntax identifierName,
            JassEqualsValueClauseSyntax? value)
        {
            Type = type;
            IdentifierName = identifierName;
            Value = value;
        }

        public JassTypeSyntax Type { get; }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public JassEqualsValueClauseSyntax? Value { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassVariableDeclaratorSyntax variableDeclarator
                && Type.IsEquivalentTo(variableDeclarator.Type)
                && IdentifierName.IsEquivalentTo(variableDeclarator.IdentifierName)
                && Value.NullableEquivalentTo(variableDeclarator.Value);
        }

        public override void WriteTo(TextWriter writer)
        {
            Type.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            Value?.WriteTo(writer);
        }

        public override string ToString() => $"{Type} {IdentifierName}{Value.OptionalPrefixed()}";

        public override JassSyntaxToken GetFirstToken() => Type.GetFirstToken();

        public override JassSyntaxToken GetLastToken() => ((JassSyntaxNode?)Value ?? IdentifierName).GetLastToken();

        protected internal override JassVariableDeclaratorSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassVariableDeclaratorSyntax(
                Type.ReplaceFirstToken(newToken),
                IdentifierName,
                Value);
        }

        protected internal override JassVariableDeclaratorSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            if (Value is not null)
            {
                return new JassVariableDeclaratorSyntax(
                    Type,
                    IdentifierName,
                    Value.ReplaceLastToken(newToken));
            }

            return new JassVariableDeclaratorSyntax(
                Type,
                IdentifierName.ReplaceLastToken(newToken),
                null);
        }
    }
}
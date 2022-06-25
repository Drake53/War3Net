// ------------------------------------------------------------------------------
// <copyright file="VJassVariableDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassVariableDeclaratorSyntax : VJassVariableOrArrayDeclaratorSyntax
    {
        internal VJassVariableDeclaratorSyntax(
            VJassTypeSyntax type,
            VJassIdentifierNameSyntax identifierName,
            VJassEqualsValueClauseSyntax? value)
        {
            Type = type;
            IdentifierName = identifierName;
            Value = value;
        }

        public VJassTypeSyntax Type { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public VJassEqualsValueClauseSyntax? Value { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassVariableDeclaratorSyntax variableDeclarator
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

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            Type.ProcessTo(writer, context);
            IdentifierName.ProcessTo(writer, context);
            Value?.ProcessTo(writer, context);
        }

        public override string ToString() => $"{Type} {IdentifierName}{Value.OptionalPrefixed()}";

        public override VJassSyntaxToken GetFirstToken() => Type.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => ((VJassSyntaxNode?)Value ?? IdentifierName).GetLastToken();

        protected internal override VJassVariableDeclaratorSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassVariableDeclaratorSyntax(
                Type.ReplaceFirstToken(newToken),
                IdentifierName,
                Value);
        }

        protected internal override VJassVariableDeclaratorSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (Value is not null)
            {
                return new VJassVariableDeclaratorSyntax(
                    Type,
                    IdentifierName,
                    Value.ReplaceLastToken(newToken));
            }

            return new VJassVariableDeclaratorSyntax(
                Type,
                IdentifierName.ReplaceLastToken(newToken),
                null);
        }
    }
}
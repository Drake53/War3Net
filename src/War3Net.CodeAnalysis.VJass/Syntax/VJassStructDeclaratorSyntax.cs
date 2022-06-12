// ------------------------------------------------------------------------------
// <copyright file="VJassStructDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassStructDeclaratorSyntax : VJassSyntaxNode
    {
        internal VJassStructDeclaratorSyntax(
            VJassSyntaxToken structToken,
            VJassIdentifierNameSyntax identifierName,
            VJassExtendsClauseSyntax? extendsClause)
        {
            StructToken = structToken;
            IdentifierName = identifierName;
            ExtendsClause = extendsClause;
        }

        public VJassSyntaxToken StructToken { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public VJassExtendsClauseSyntax? ExtendsClause { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassStructDeclaratorSyntax structDeclarator
                && IdentifierName.IsEquivalentTo(structDeclarator.IdentifierName)
                && ExtendsClause.NullableEquivalentTo(structDeclarator.ExtendsClause);
        }

        public override void WriteTo(TextWriter writer)
        {
            StructToken.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            ExtendsClause?.WriteTo(writer);
        }

        public override string ToString() => $"{StructToken} {IdentifierName}{ExtendsClause.OptionalPrefixed()}";

        public override VJassSyntaxToken GetFirstToken() => StructToken;

        public override VJassSyntaxToken GetLastToken() => ((VJassSyntaxNode?)ExtendsClause ?? IdentifierName).GetLastToken();

        protected internal override VJassStructDeclaratorSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassStructDeclaratorSyntax(
                newToken,
                IdentifierName,
                ExtendsClause);
        }

        protected internal override VJassStructDeclaratorSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (ExtendsClause is not null)
            {
                return new VJassStructDeclaratorSyntax(
                    StructToken,
                    IdentifierName,
                    ExtendsClause.ReplaceLastToken(newToken));
            }

            return new VJassStructDeclaratorSyntax(
                StructToken,
                IdentifierName.ReplaceLastToken(newToken),
                null);
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="VJassTypeDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassTypeDeclarationSyntax : VJassScopedDeclarationSyntax
    {
        internal VJassTypeDeclarationSyntax(
            VJassSyntaxToken typeToken,
            VJassIdentifierNameSyntax identifierName,
            VJassSyntaxToken extendsToken,
            VJassTypeSyntax baseType)
        {
            TypeToken = typeToken;
            IdentifierName = identifierName;
            ExtendsToken = extendsToken;
            BaseType = baseType;
        }

        public VJassSyntaxToken TypeToken { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public VJassSyntaxToken ExtendsToken { get; }

        public VJassTypeSyntax BaseType { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassTypeDeclarationSyntax typeDeclaration
                && IdentifierName.IsEquivalentTo(typeDeclaration.IdentifierName)
                && BaseType.IsEquivalentTo(typeDeclaration.BaseType);
        }

        public override void WriteTo(TextWriter writer)
        {
            TypeToken.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            ExtendsToken.WriteTo(writer);
            BaseType.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            TypeToken.ProcessTo(writer, context);
            IdentifierName.ProcessTo(writer, context);
            ExtendsToken.ProcessTo(writer, context);
            BaseType.ProcessTo(writer, context);
        }

        public override string ToString() => $"{TypeToken} {IdentifierName} {ExtendsToken} {BaseType}";

        public override VJassSyntaxToken GetFirstToken() => TypeToken;

        public override VJassSyntaxToken GetLastToken() => BaseType.GetLastToken();

        protected internal override VJassTypeDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassTypeDeclarationSyntax(
                newToken,
                IdentifierName,
                ExtendsToken,
                BaseType);
        }

        protected internal override VJassTypeDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassTypeDeclarationSyntax(
                TypeToken,
                IdentifierName,
                ExtendsToken,
                BaseType.ReplaceLastToken(newToken));
        }
    }
}
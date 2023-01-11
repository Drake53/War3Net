// ------------------------------------------------------------------------------
// <copyright file="JassGlobalConstantDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassGlobalConstantDeclarationSyntax : JassGlobalDeclarationSyntax
    {
        internal JassGlobalConstantDeclarationSyntax(
            JassSyntaxToken constantToken,
            JassTypeSyntax type,
            JassIdentifierNameSyntax identifierName,
            JassEqualsValueClauseSyntax value)
        {
            ConstantToken = constantToken;
            Type = type;
            IdentifierName = identifierName;
            Value = value;
        }

        public JassSyntaxToken ConstantToken { get; }

        public JassTypeSyntax Type { get; }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public JassEqualsValueClauseSyntax Value { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassGlobalConstantDeclarationSyntax globalConstantDeclaration
                && Type.IsEquivalentTo(globalConstantDeclaration.Type)
                && IdentifierName.IsEquivalentTo(globalConstantDeclaration.IdentifierName)
                && Value.IsEquivalentTo(globalConstantDeclaration.Value);
        }

        public override void WriteTo(TextWriter writer)
        {
            ConstantToken.WriteTo(writer);
            Type.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            Value.WriteTo(writer);
        }

        public override string ToString() => $"{ConstantToken} {Type} {IdentifierName} {Value}";

        public override JassSyntaxToken GetFirstToken() => ConstantToken;

        public override JassSyntaxToken GetLastToken() => Value.GetLastToken();

        protected internal override JassGlobalConstantDeclarationSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassGlobalConstantDeclarationSyntax(
                newToken,
                Type,
                IdentifierName,
                Value);
        }

        protected internal override JassGlobalConstantDeclarationSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassGlobalConstantDeclarationSyntax(
                ConstantToken,
                Type,
                IdentifierName,
                Value.ReplaceLastToken(newToken));
        }
    }
}
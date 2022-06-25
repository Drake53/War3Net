// ------------------------------------------------------------------------------
// <copyright file="VJassModuleDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassModuleDeclaratorSyntax : VJassSyntaxNode
    {
        internal VJassModuleDeclaratorSyntax(
            VJassSyntaxToken moduleToken,
            VJassIdentifierNameSyntax identifierName,
            VJassInitializerSyntax? initializer)
        {
            ModuleToken = moduleToken;
            IdentifierName = identifierName;
            Initializer = initializer;
        }

        public VJassSyntaxToken ModuleToken { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public VJassInitializerSyntax? Initializer { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassModuleDeclaratorSyntax moduleDeclarator
                && IdentifierName.IsEquivalentTo(moduleDeclarator.IdentifierName)
                && Initializer.NullableEquivalentTo(moduleDeclarator.Initializer);
        }

        public override void WriteTo(TextWriter writer)
        {
            ModuleToken.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            Initializer?.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            ModuleToken.ProcessTo(writer, context);
            IdentifierName.ProcessTo(writer, context);
            Initializer?.ProcessTo(writer, context);
        }

        public override string ToString() => $"{ModuleToken} {IdentifierName}{Initializer.OptionalPrefixed()}";

        public override VJassSyntaxToken GetFirstToken() => ModuleToken;

        public override VJassSyntaxToken GetLastToken() => ((VJassSyntaxNode?)Initializer ?? IdentifierName).GetLastToken();

        protected internal override VJassModuleDeclaratorSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassModuleDeclaratorSyntax(
                newToken,
                IdentifierName,
                Initializer);
        }

        protected internal override VJassModuleDeclaratorSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (Initializer is not null)
            {
                return new VJassModuleDeclaratorSyntax(
                    ModuleToken,
                    IdentifierName,
                    Initializer.ReplaceLastToken(newToken));
            }

            return new VJassModuleDeclaratorSyntax(
                ModuleToken,
                IdentifierName.ReplaceLastToken(newToken),
                null);
        }
    }
}
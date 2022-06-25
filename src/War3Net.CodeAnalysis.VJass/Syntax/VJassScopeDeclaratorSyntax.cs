// ------------------------------------------------------------------------------
// <copyright file="VJassScopeDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassScopeDeclaratorSyntax : VJassSyntaxNode
    {
        internal VJassScopeDeclaratorSyntax(
            VJassSyntaxToken scopeToken,
            VJassIdentifierNameSyntax identifierName,
            VJassInitializerSyntax? initializer)
        {
            ScopeToken = scopeToken;
            IdentifierName = identifierName;
            Initializer = initializer;
        }

        public VJassSyntaxToken ScopeToken { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public VJassInitializerSyntax? Initializer { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassScopeDeclaratorSyntax scopeDeclarator
                && IdentifierName.IsEquivalentTo(scopeDeclarator.IdentifierName)
                && Initializer.NullableEquivalentTo(scopeDeclarator.Initializer);
        }

        public override void WriteTo(TextWriter writer)
        {
            ScopeToken.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            Initializer?.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            ScopeToken.ProcessTo(writer, context);
            IdentifierName.ProcessTo(writer, context);
            Initializer?.ProcessTo(writer, context);
        }

        public override string ToString() => $"{ScopeToken} {IdentifierName}{Initializer.OptionalPrefixed()}";

        public override VJassSyntaxToken GetFirstToken() => ScopeToken;

        public override VJassSyntaxToken GetLastToken() => ((VJassSyntaxNode?)Initializer ?? IdentifierName).GetLastToken();

        protected internal override VJassScopeDeclaratorSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassScopeDeclaratorSyntax(
                newToken,
                IdentifierName,
                Initializer);
        }

        protected internal override VJassScopeDeclaratorSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            if (Initializer is not null)
            {
                return new VJassScopeDeclaratorSyntax(
                    ScopeToken,
                    IdentifierName,
                    Initializer.ReplaceLastToken(newToken));
            }

            return new VJassScopeDeclaratorSyntax(
                ScopeToken,
                IdentifierName.ReplaceLastToken(newToken),
                null);
        }
    }
}
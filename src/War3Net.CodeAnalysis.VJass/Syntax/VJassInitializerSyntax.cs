// ------------------------------------------------------------------------------
// <copyright file="VJassInitializerSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassInitializerSyntax : VJassSyntaxNode
    {
        internal VJassInitializerSyntax(
            VJassSyntaxToken initializerToken,
            VJassIdentifierNameSyntax identifierName)
        {
            InitializerToken = initializerToken;
            IdentifierName = identifierName;
        }

        public VJassSyntaxToken InitializerToken { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassInitializerSyntax initializer
                && IdentifierName.IsEquivalentTo(initializer.IdentifierName);
        }

        public override void WriteTo(TextWriter writer)
        {
            InitializerToken.WriteTo(writer);
            IdentifierName.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            InitializerToken.ProcessTo(writer, context);
            IdentifierName.ProcessTo(writer, context);
        }

        public override string ToString() => $"{InitializerToken} {IdentifierName}";

        public override VJassSyntaxToken GetFirstToken() => InitializerToken;

        public override VJassSyntaxToken GetLastToken() => IdentifierName.GetLastToken();

        protected internal override VJassInitializerSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassInitializerSyntax(
                newToken,
                IdentifierName);
        }

        protected internal override VJassInitializerSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassInitializerSyntax(
                InitializerToken,
                IdentifierName.ReplaceLastToken(newToken));
        }
    }
}
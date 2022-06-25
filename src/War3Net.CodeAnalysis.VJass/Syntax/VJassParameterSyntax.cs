// ------------------------------------------------------------------------------
// <copyright file="VJassParameterSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassParameterSyntax : VJassSyntaxNode
    {
        internal VJassParameterSyntax(
            VJassTypeSyntax type,
            VJassIdentifierNameSyntax identifierName)
        {
            Type = type;
            IdentifierName = identifierName;
        }

        public VJassTypeSyntax Type { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassParameterSyntax parameter
                && Type.IsEquivalentTo(parameter.Type)
                && IdentifierName.IsEquivalentTo(parameter.IdentifierName);
        }

        public override void WriteTo(TextWriter writer)
        {
            Type.WriteTo(writer);
            IdentifierName.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            Type.ProcessTo(writer, context);
            IdentifierName.ProcessTo(writer, context);
        }

        public override string ToString() => $"{Type} {IdentifierName}";

        public override VJassSyntaxToken GetFirstToken() => Type.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => IdentifierName.GetLastToken();

        protected internal override VJassParameterSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassParameterSyntax(
                Type.ReplaceFirstToken(newToken),
                IdentifierName);
        }

        protected internal override VJassParameterSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassParameterSyntax(
                Type,
                IdentifierName.ReplaceLastToken(newToken));
        }
    }
}
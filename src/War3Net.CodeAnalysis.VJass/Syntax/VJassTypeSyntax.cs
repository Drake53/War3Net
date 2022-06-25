// ------------------------------------------------------------------------------
// <copyright file="VJassTypeSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassTypeSyntax : VJassSyntaxNode
    {
        internal VJassTypeSyntax(
            VJassIdentifierNameSyntax typeName)
        {
            TypeName = typeName;
        }

        public VJassIdentifierNameSyntax TypeName { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassTypeSyntax type
                && TypeName.IsEquivalentTo(type.TypeName);
        }

        public override void WriteTo(TextWriter writer)
        {
            TypeName.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            TypeName.ProcessTo(writer, context);
        }

        public override string ToString() => TypeName.ToString();

        public override VJassSyntaxToken GetFirstToken() => TypeName.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => TypeName.GetLastToken();

        protected internal override VJassTypeSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassTypeSyntax(TypeName.ReplaceFirstToken(newToken));
        }

        protected internal override VJassTypeSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassTypeSyntax(TypeName.ReplaceLastToken(newToken));
        }
    }
}
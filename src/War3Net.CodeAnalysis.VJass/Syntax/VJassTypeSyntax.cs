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
        public static readonly VJassTypeSyntax Boolean = new(new(new VJassSyntaxToken(VJassSyntaxKind.BooleanKeyword, VJassKeyword.Boolean, VJassSyntaxTriviaList.SingleSpace)));
        public static readonly VJassTypeSyntax Code = new(new(new VJassSyntaxToken(VJassSyntaxKind.CodeKeyword, VJassKeyword.Code, VJassSyntaxTriviaList.SingleSpace)));
        public static readonly VJassTypeSyntax Handle = new(new(new VJassSyntaxToken(VJassSyntaxKind.HandleKeyword, VJassKeyword.Handle, VJassSyntaxTriviaList.SingleSpace)));
        public static readonly VJassTypeSyntax Integer = new(new(new VJassSyntaxToken(VJassSyntaxKind.IntegerKeyword, VJassKeyword.Integer, VJassSyntaxTriviaList.SingleSpace)));
        public static readonly VJassTypeSyntax Nothing = new(new(new VJassSyntaxToken(VJassSyntaxKind.NothingKeyword, VJassKeyword.Nothing, VJassSyntaxTriviaList.SingleSpace)));
        public static readonly VJassTypeSyntax Real = new(new(new VJassSyntaxToken(VJassSyntaxKind.RealKeyword, VJassKeyword.Real, VJassSyntaxTriviaList.SingleSpace)));
        public static readonly VJassTypeSyntax String = new(new(new VJassSyntaxToken(VJassSyntaxKind.StringKeyword, VJassKeyword.String, VJassSyntaxTriviaList.SingleSpace)));
        public static readonly VJassTypeSyntax ThisType = new(new(new VJassSyntaxToken(VJassSyntaxKind.ThisTypeKeyword, VJassKeyword.ThisType, VJassSyntaxTriviaList.SingleSpace)));

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
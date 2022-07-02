// ------------------------------------------------------------------------------
// <copyright file="VJassNoVJassPreprocessorDirectiveTrivia.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassNoVJassPreprocessorDirectiveTrivia : VJassStructuredTriviaSyntax
    {
        internal VJassNoVJassPreprocessorDirectiveTrivia(
            VJassSyntaxToken preprocessorDirectiveStartToken,
            VJassSyntaxToken noVJassToken,
            string body,
            VJassSyntaxToken preprocessorDirectiveEndToken,
            VJassSyntaxToken endNoVJassToken)
        {
            PreprocessorDirectiveStartToken = preprocessorDirectiveStartToken;
            NoVJassToken = noVJassToken;
            Body = body;
            PreprocessorDirectiveEndToken = preprocessorDirectiveEndToken;
            EndNoVJassToken = endNoVJassToken;
        }

        public VJassSyntaxToken PreprocessorDirectiveStartToken { get; }

        public VJassSyntaxToken NoVJassToken { get; }

        public string Body { get; }

        public VJassSyntaxToken PreprocessorDirectiveEndToken { get; }

        public VJassSyntaxToken EndNoVJassToken { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassNoVJassPreprocessorDirectiveTrivia noVJassPreprocessorDirectiveTrivia
                && string.Equals(Body, noVJassPreprocessorDirectiveTrivia.Body, StringComparison.Ordinal);
        }

        public override void WriteTo(TextWriter writer)
        {
            PreprocessorDirectiveStartToken.WriteTo(writer);
            NoVJassToken.WriteTo(writer);
            writer.Write(Body);
            PreprocessorDirectiveEndToken.WriteTo(writer);
            EndNoVJassToken.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            if (context.KeepNoVJassPreprocessorDirectives)
            {
                WriteTo(writer);
            }
        }

        public override string ToString() => $"{PreprocessorDirectiveStartToken} {NoVJassToken} [...]";

        public override VJassSyntaxToken GetFirstToken() => PreprocessorDirectiveStartToken;

        public override VJassSyntaxToken GetLastToken() => EndNoVJassToken;

        protected internal override VJassNoVJassPreprocessorDirectiveTrivia ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassNoVJassPreprocessorDirectiveTrivia(
                newToken,
                NoVJassToken,
                Body,
                PreprocessorDirectiveEndToken,
                EndNoVJassToken);
        }

        protected internal override VJassNoVJassPreprocessorDirectiveTrivia ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassNoVJassPreprocessorDirectiveTrivia(
                PreprocessorDirectiveStartToken,
                NoVJassToken,
                Body,
                PreprocessorDirectiveEndToken,
                newToken);
        }
    }
}
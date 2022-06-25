// ------------------------------------------------------------------------------
// <copyright file="VJassFunctionReferenceExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassFunctionReferenceExpressionSyntax : VJassExpressionSyntax
    {
        internal VJassFunctionReferenceExpressionSyntax(
            VJassSyntaxToken functionToken,
            VJassIdentifierNameSyntax identifierName)
        {
            FunctionToken = functionToken;
            IdentifierName = identifierName;
        }

        public VJassSyntaxToken FunctionToken { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassFunctionReferenceExpressionSyntax functionReferenceExpression
                && IdentifierName.IsEquivalentTo(functionReferenceExpression.IdentifierName);
        }

        public override void WriteTo(TextWriter writer)
        {
            FunctionToken.WriteTo(writer);
            IdentifierName.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            FunctionToken.ProcessTo(writer, context);
            IdentifierName.ProcessTo(writer, context);
        }

        public override string ToString() => $"{FunctionToken} {IdentifierName}";

        public override VJassSyntaxToken GetFirstToken() => FunctionToken;

        public override VJassSyntaxToken GetLastToken() => IdentifierName.GetLastToken();

        protected internal override VJassFunctionReferenceExpressionSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassFunctionReferenceExpressionSyntax(
                newToken,
                IdentifierName);
        }

        protected internal override VJassFunctionReferenceExpressionSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassFunctionReferenceExpressionSyntax(
                FunctionToken,
                IdentifierName.ReplaceLastToken(newToken));
        }
    }
}
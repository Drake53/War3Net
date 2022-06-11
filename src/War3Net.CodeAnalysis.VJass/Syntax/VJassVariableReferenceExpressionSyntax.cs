// ------------------------------------------------------------------------------
// <copyright file="VJassVariableReferenceExpressionSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassVariableReferenceExpressionSyntax : VJassExpressionSyntax
    {
        internal VJassVariableReferenceExpressionSyntax(
            VJassIdentifierNameSyntax identifierName)
        {
            IdentifierName = identifierName;
        }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassVariableReferenceExpressionSyntax variableReferenceExpression
                && IdentifierName.IsEquivalentTo(variableReferenceExpression.IdentifierName);
        }

        public override void WriteTo(TextWriter writer)
        {
            IdentifierName.WriteTo(writer);
        }

        public override string ToString() => IdentifierName.ToString();

        public override VJassSyntaxToken GetFirstToken() => IdentifierName.GetFirstToken();

        public override VJassSyntaxToken GetLastToken() => IdentifierName.GetLastToken();

        protected internal override VJassVariableReferenceExpressionSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassVariableReferenceExpressionSyntax(IdentifierName.ReplaceFirstToken(newToken));
        }

        protected internal override VJassVariableReferenceExpressionSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassVariableReferenceExpressionSyntax(IdentifierName.ReplaceLastToken(newToken));
        }
    }
}
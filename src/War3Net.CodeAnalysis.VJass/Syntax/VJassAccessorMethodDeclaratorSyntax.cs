// ------------------------------------------------------------------------------
// <copyright file="VJassAccessorMethodDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassAccessorMethodDeclaratorSyntax : VJassMethodOrOperatorDeclaratorSyntax
    {
        internal VJassAccessorMethodDeclaratorSyntax(
            VJassSyntaxToken methodToken,
            VJassSyntaxToken operatorToken,
            VJassIdentifierNameSyntax propertyIdentifierName,
            VJassParameterListOrEmptyParameterListSyntax parameterList,
            VJassReturnClauseSyntax returnClause)
        {
            MethodToken = methodToken;
            OperatorToken = operatorToken;
            PropertyIdentifierName = propertyIdentifierName;
            ParameterList = parameterList;
            ReturnClause = returnClause;
        }

        public VJassSyntaxToken MethodToken { get; }

        public VJassSyntaxToken OperatorToken { get; }

        public VJassIdentifierNameSyntax PropertyIdentifierName { get; }

        public VJassParameterListOrEmptyParameterListSyntax ParameterList { get; }

        public VJassReturnClauseSyntax ReturnClause { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassAccessorMethodDeclaratorSyntax accessorMethodDeclarator
                && PropertyIdentifierName.IsEquivalentTo(accessorMethodDeclarator.PropertyIdentifierName)
                && ParameterList.IsEquivalentTo(accessorMethodDeclarator.ParameterList)
                && ReturnClause.IsEquivalentTo(accessorMethodDeclarator.ReturnClause);
        }

        public override void WriteTo(TextWriter writer)
        {
            MethodToken.WriteTo(writer);
            OperatorToken.WriteTo(writer);
            PropertyIdentifierName.WriteTo(writer);
            ParameterList.WriteTo(writer);
            ReturnClause.WriteTo(writer);
        }

        public override string ToString() => $"{MethodToken} {OperatorToken} {PropertyIdentifierName} {ParameterList} {ReturnClause}";

        public override VJassSyntaxToken GetFirstToken() => MethodToken;

        public override VJassSyntaxToken GetLastToken() => ReturnClause.GetLastToken();

        protected internal override VJassAccessorMethodDeclaratorSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassAccessorMethodDeclaratorSyntax(
                newToken,
                OperatorToken,
                PropertyIdentifierName,
                ParameterList,
                ReturnClause);
        }

        protected internal override VJassAccessorMethodDeclaratorSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassAccessorMethodDeclaratorSyntax(
                MethodToken,
                OperatorToken,
                PropertyIdentifierName,
                ParameterList,
                ReturnClause.ReplaceLastToken(newToken));
        }
    }
}
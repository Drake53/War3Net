// ------------------------------------------------------------------------------
// <copyright file="VJassOperatorMethodDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassOperatorMethodDeclaratorSyntax : VJassMethodOrOperatorDeclaratorSyntax
    {
        internal VJassOperatorMethodDeclaratorSyntax(
            VJassSyntaxToken methodToken,
            VJassSyntaxToken operatorToken,
            VJassSyntaxToken operatorOverloadToken,
            VJassParameterListOrEmptyParameterListSyntax parameterList,
            VJassReturnClauseSyntax returnClause)
        {
            MethodToken = methodToken;
            OperatorToken = operatorToken;
            OperatorOverloadToken = operatorOverloadToken;
            ParameterList = parameterList;
            ReturnClause = returnClause;
        }

        public VJassSyntaxToken MethodToken { get; }

        public VJassSyntaxToken OperatorToken { get; }

        public VJassSyntaxToken OperatorOverloadToken { get; }

        public VJassParameterListOrEmptyParameterListSyntax ParameterList { get; }

        public VJassReturnClauseSyntax ReturnClause { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassOperatorMethodDeclaratorSyntax operatorMethodDeclarator
                && OperatorOverloadToken.IsEquivalentTo(operatorMethodDeclarator.OperatorOverloadToken)
                && ParameterList.IsEquivalentTo(operatorMethodDeclarator.ParameterList)
                && ReturnClause.IsEquivalentTo(operatorMethodDeclarator.ReturnClause);
        }

        public override void WriteTo(TextWriter writer)
        {
            MethodToken.WriteTo(writer);
            OperatorToken.WriteTo(writer);
            OperatorOverloadToken.WriteTo(writer);
            ParameterList.WriteTo(writer);
            ReturnClause.WriteTo(writer);
        }

        public override string ToString() => $"{MethodToken} {OperatorToken} {OperatorOverloadToken} {ParameterList} {ReturnClause}";

        public override VJassSyntaxToken GetFirstToken() => MethodToken;

        public override VJassSyntaxToken GetLastToken() => ReturnClause.GetLastToken();

        protected internal override VJassOperatorMethodDeclaratorSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassOperatorMethodDeclaratorSyntax(
                newToken,
                OperatorToken,
                OperatorOverloadToken,
                ParameterList,
                ReturnClause);
        }

        protected internal override VJassOperatorMethodDeclaratorSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassOperatorMethodDeclaratorSyntax(
                MethodToken,
                OperatorToken,
                OperatorOverloadToken,
                ParameterList,
                ReturnClause.ReplaceLastToken(newToken));
        }
    }
}
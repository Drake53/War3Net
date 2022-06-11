// ------------------------------------------------------------------------------
// <copyright file="VJassIndexerMethodDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassIndexerMethodDeclaratorSyntax : VJassMethodOrOperatorDeclaratorSyntax
    {
        internal VJassIndexerMethodDeclaratorSyntax(
            VJassSyntaxToken methodToken,
            VJassSyntaxToken operatorToken,
            VJassSyntaxToken indexerToken,
            VJassParameterListOrEmptyParameterListSyntax parameterList,
            VJassReturnClauseSyntax returnClause)
        {
            MethodToken = methodToken;
            OperatorToken = operatorToken;
            IndexerToken = indexerToken;
            ParameterList = parameterList;
            ReturnClause = returnClause;
        }

        public VJassSyntaxToken MethodToken { get; }

        public VJassSyntaxToken OperatorToken { get; }

        public VJassSyntaxToken IndexerToken { get; }

        public VJassParameterListOrEmptyParameterListSyntax ParameterList { get; }

        public VJassReturnClauseSyntax ReturnClause { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassIndexerMethodDeclaratorSyntax indexerMethodDeclarator
                && IndexerToken.IsEquivalentTo(indexerMethodDeclarator.IndexerToken)
                && ParameterList.IsEquivalentTo(indexerMethodDeclarator.ParameterList)
                && ReturnClause.IsEquivalentTo(indexerMethodDeclarator.ReturnClause);
        }

        public override void WriteTo(TextWriter writer)
        {
            MethodToken.WriteTo(writer);
            OperatorToken.WriteTo(writer);
            IndexerToken.WriteTo(writer);
            ParameterList.WriteTo(writer);
            ReturnClause.WriteTo(writer);
        }

        public override string ToString() => $"{MethodToken} {OperatorToken} {IndexerToken} {ParameterList} {ReturnClause}";

        public override VJassSyntaxToken GetFirstToken() => MethodToken;

        public override VJassSyntaxToken GetLastToken() => ReturnClause.GetLastToken();

        protected internal override VJassIndexerMethodDeclaratorSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassIndexerMethodDeclaratorSyntax(
                newToken,
                OperatorToken,
                IndexerToken,
                ParameterList,
                ReturnClause);
        }

        protected internal override VJassIndexerMethodDeclaratorSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassIndexerMethodDeclaratorSyntax(
                MethodToken,
                OperatorToken,
                IndexerToken,
                ParameterList,
                ReturnClause.ReplaceLastToken(newToken));
        }
    }
}
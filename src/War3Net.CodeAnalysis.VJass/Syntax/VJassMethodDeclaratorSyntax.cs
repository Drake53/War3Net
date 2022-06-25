// ------------------------------------------------------------------------------
// <copyright file="VJassMethodDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassMethodDeclaratorSyntax : VJassMethodOrOperatorDeclaratorSyntax
    {
        internal VJassMethodDeclaratorSyntax(
            VJassSyntaxToken methodToken,
            VJassIdentifierNameSyntax identifierName,
            VJassParameterListOrEmptyParameterListSyntax parameterList,
            VJassReturnClauseSyntax returnClause)
        {
            MethodToken = methodToken;
            IdentifierName = identifierName;
            ParameterList = parameterList;
            ReturnClause = returnClause;
        }

        public VJassSyntaxToken MethodToken { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public VJassParameterListOrEmptyParameterListSyntax ParameterList { get; }

        public VJassReturnClauseSyntax ReturnClause { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassMethodDeclaratorSyntax methodDeclarator
                && IdentifierName.IsEquivalentTo(methodDeclarator.IdentifierName)
                && ParameterList.IsEquivalentTo(methodDeclarator.ParameterList)
                && ReturnClause.IsEquivalentTo(methodDeclarator.ReturnClause);
        }

        public override void WriteTo(TextWriter writer)
        {
            MethodToken.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            ParameterList.WriteTo(writer);
            ReturnClause.WriteTo(writer);
        }

        public override void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            MethodToken.ProcessTo(writer, context);
            IdentifierName.ProcessTo(writer, context);
            ParameterList.ProcessTo(writer, context);
            ReturnClause.ProcessTo(writer, context);
        }

        public override string ToString() => $"{MethodToken} {IdentifierName} {ParameterList} {ReturnClause}";

        public override VJassSyntaxToken GetFirstToken() => MethodToken;

        public override VJassSyntaxToken GetLastToken() => ReturnClause.GetLastToken();

        protected internal override VJassMethodDeclaratorSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            return new VJassMethodDeclaratorSyntax(
                newToken,
                IdentifierName,
                ParameterList,
                ReturnClause);
        }

        protected internal override VJassMethodDeclaratorSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassMethodDeclaratorSyntax(
                MethodToken,
                IdentifierName,
                ParameterList,
                ReturnClause.ReplaceLastToken(newToken));
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="VJassFunctionDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassFunctionDeclaratorSyntax : VJassSyntaxNode
    {
        internal VJassFunctionDeclaratorSyntax(
            VJassSyntaxToken? constantToken,
            VJassSyntaxToken functionToken,
            VJassIdentifierNameSyntax identifierName,
            VJassParameterListOrEmptyParameterListSyntax parameterList,
            VJassReturnClauseSyntax returnClause)
        {
            ConstantToken = constantToken;
            FunctionToken = functionToken;
            IdentifierName = identifierName;
            ParameterList = parameterList;
            ReturnClause = returnClause;
        }

        public VJassSyntaxToken? ConstantToken { get; }

        public VJassSyntaxToken FunctionToken { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public VJassParameterListOrEmptyParameterListSyntax ParameterList { get; }

        public VJassReturnClauseSyntax ReturnClause { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassFunctionDeclaratorSyntax functionDeclarator
                && ConstantToken.NullableEquals(functionDeclarator.ConstantToken)
                && IdentifierName.IsEquivalentTo(functionDeclarator.IdentifierName)
                && ParameterList.IsEquivalentTo(functionDeclarator.ParameterList)
                && ReturnClause.IsEquivalentTo(functionDeclarator.ReturnClause);
        }

        public override void WriteTo(TextWriter writer)
        {
            ConstantToken?.WriteTo(writer);
            FunctionToken.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            ParameterList.WriteTo(writer);
            ReturnClause.WriteTo(writer);
        }

        public override string ToString() => $"{ConstantToken.OptionalSuffixed()}{FunctionToken} {IdentifierName} {ParameterList} {ReturnClause}";

        public override VJassSyntaxToken GetFirstToken() => ConstantToken ?? FunctionToken;

        public override VJassSyntaxToken GetLastToken() => ReturnClause.GetLastToken();

        protected internal override VJassFunctionDeclaratorSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            if (ConstantToken is not null)
            {
                return new VJassFunctionDeclaratorSyntax(
                    newToken,
                    FunctionToken,
                    IdentifierName,
                    ParameterList,
                    ReturnClause);
            }

            return new VJassFunctionDeclaratorSyntax(
                null,
                newToken,
                IdentifierName,
                ParameterList,
                ReturnClause);
        }

        protected internal override VJassFunctionDeclaratorSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassFunctionDeclaratorSyntax(
                ConstantToken,
                FunctionToken,
                IdentifierName,
                ParameterList,
                ReturnClause.ReplaceLastToken(newToken));
        }
    }
}
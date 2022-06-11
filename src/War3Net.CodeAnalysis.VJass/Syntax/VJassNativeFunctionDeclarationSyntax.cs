// ------------------------------------------------------------------------------
// <copyright file="VJassNativeFunctionDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.VJass.Extensions;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassNativeFunctionDeclarationSyntax : VJassScopedDeclarationSyntax
    {
        internal VJassNativeFunctionDeclarationSyntax(
            VJassSyntaxToken? constantToken,
            VJassSyntaxToken nativeToken,
            VJassIdentifierNameSyntax identifierName,
            VJassParameterListOrEmptyParameterListSyntax parameterList,
            VJassReturnClauseSyntax returnClause)
        {
            ConstantToken = constantToken;
            NativeToken = nativeToken;
            IdentifierName = identifierName;
            ParameterList = parameterList;
            ReturnClause = returnClause;
        }

        public VJassSyntaxToken? ConstantToken { get; }

        public VJassSyntaxToken NativeToken { get; }

        public VJassIdentifierNameSyntax IdentifierName { get; }

        public VJassParameterListOrEmptyParameterListSyntax ParameterList { get; }

        public VJassReturnClauseSyntax ReturnClause { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] VJassSyntaxNode? other)
        {
            return other is VJassNativeFunctionDeclarationSyntax nativeFunctionDeclaration
                && ConstantToken.NullableEquals(nativeFunctionDeclaration.ConstantToken)
                && IdentifierName.IsEquivalentTo(nativeFunctionDeclaration.IdentifierName)
                && ParameterList.IsEquivalentTo(nativeFunctionDeclaration.ParameterList)
                && ReturnClause.IsEquivalentTo(nativeFunctionDeclaration.ReturnClause);
        }

        public override void WriteTo(TextWriter writer)
        {
            ConstantToken?.WriteTo(writer);
            NativeToken.WriteTo(writer);
            IdentifierName.WriteTo(writer);
            ParameterList.WriteTo(writer);
            ReturnClause.WriteTo(writer);
        }

        public override string ToString() => $"{ConstantToken.OptionalSuffixed()}{NativeToken} {IdentifierName} {ParameterList} {ReturnClause}";

        public override VJassSyntaxToken GetFirstToken() => ConstantToken ?? NativeToken;

        public override VJassSyntaxToken GetLastToken() => ReturnClause.GetLastToken();

        protected internal override VJassNativeFunctionDeclarationSyntax ReplaceFirstToken(VJassSyntaxToken newToken)
        {
            if (ConstantToken is not null)
            {
                return new VJassNativeFunctionDeclarationSyntax(
                    newToken,
                    NativeToken,
                    IdentifierName,
                    ParameterList,
                    ReturnClause);
            }

            return new VJassNativeFunctionDeclarationSyntax(
                null,
                newToken,
                IdentifierName,
                ParameterList,
                ReturnClause);
        }

        protected internal override VJassNativeFunctionDeclarationSyntax ReplaceLastToken(VJassSyntaxToken newToken)
        {
            return new VJassNativeFunctionDeclarationSyntax(
                ConstantToken,
                NativeToken,
                IdentifierName,
                ParameterList,
                ReturnClause.ReplaceLastToken(newToken));
        }
    }
}
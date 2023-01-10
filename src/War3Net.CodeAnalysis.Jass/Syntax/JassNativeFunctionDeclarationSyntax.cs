// ------------------------------------------------------------------------------
// <copyright file="JassNativeFunctionDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassNativeFunctionDeclarationSyntax : JassTopLevelDeclarationSyntax
    {
        internal JassNativeFunctionDeclarationSyntax(
            JassSyntaxToken? constantToken,
            JassSyntaxToken nativeToken,
            JassIdentifierNameSyntax identifierName,
            JassParameterListOrEmptyParameterListSyntax parameterList,
            JassReturnClauseSyntax returnClause)
        {
            ConstantToken = constantToken;
            NativeToken = nativeToken;
            IdentifierName = identifierName;
            ParameterList = parameterList;
            ReturnClause = returnClause;
        }

        public JassSyntaxToken? ConstantToken { get; }

        public JassSyntaxToken NativeToken { get; }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public JassParameterListOrEmptyParameterListSyntax ParameterList { get; }

        public JassReturnClauseSyntax ReturnClause { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration
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

        public override JassSyntaxToken GetFirstToken() => ConstantToken ?? NativeToken;

        public override JassSyntaxToken GetLastToken() => ReturnClause.GetLastToken();

        protected internal override JassNativeFunctionDeclarationSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            if (ConstantToken is not null)
            {
                return new JassNativeFunctionDeclarationSyntax(
                    newToken,
                    NativeToken,
                    IdentifierName,
                    ParameterList,
                    ReturnClause);
            }

            return new JassNativeFunctionDeclarationSyntax(
                null,
                newToken,
                IdentifierName,
                ParameterList,
                ReturnClause);
        }

        protected internal override JassNativeFunctionDeclarationSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassNativeFunctionDeclarationSyntax(
                ConstantToken,
                NativeToken,
                IdentifierName,
                ParameterList,
                ReturnClause.ReplaceLastToken(newToken));
        }
    }
}
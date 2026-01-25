// ------------------------------------------------------------------------------
// <copyright file="JassNativeFunctionDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
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

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.NativeFunctionDeclaration;

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

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return IdentifierName;
            yield return ParameterList;
            yield return ReturnClause;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            if (ConstantToken is not null)
            {
                yield return ConstantToken;
            }

            yield return NativeToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            if (ConstantToken is not null)
            {
                yield return ConstantToken;
            }

            yield return NativeToken;
            yield return IdentifierName;
            yield return ParameterList;
            yield return ReturnClause;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return IdentifierName;
            foreach (var descendant in IdentifierName.GetDescendantNodes())
            {
                yield return descendant;
            }

            yield return ParameterList;
            foreach (var descendant in ParameterList.GetDescendantNodes())
            {
                yield return descendant;
            }

            yield return ReturnClause;
            foreach (var descendant in ReturnClause.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            if (ConstantToken is not null)
            {
                yield return ConstantToken;
            }

            yield return NativeToken;

            foreach (var descendant in IdentifierName.GetDescendantTokens())
            {
                yield return descendant;
            }

            foreach (var descendant in ParameterList.GetDescendantTokens())
            {
                yield return descendant;
            }

            foreach (var descendant in ReturnClause.GetDescendantTokens())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            if (ConstantToken is not null)
            {
                yield return ConstantToken;
            }

            yield return NativeToken;

            yield return IdentifierName;
            foreach (var descendant in IdentifierName.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            yield return ParameterList;
            foreach (var descendant in ParameterList.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            yield return ReturnClause;
            foreach (var descendant in ReturnClause.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }
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
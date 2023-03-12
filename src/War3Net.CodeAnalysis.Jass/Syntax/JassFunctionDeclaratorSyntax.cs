// ------------------------------------------------------------------------------
// <copyright file="JassFunctionDeclaratorSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using OneOf;

using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassFunctionDeclaratorSyntax : JassSyntaxNode
    {
        internal JassFunctionDeclaratorSyntax(
            JassSyntaxToken? constantToken,
            JassSyntaxToken functionToken,
            JassIdentifierNameSyntax identifierName,
            JassParameterListOrEmptyParameterListSyntax parameterList,
            JassReturnClauseSyntax returnClause)
        {
            ConstantToken = constantToken;
            FunctionToken = functionToken;
            IdentifierName = identifierName;
            ParameterList = parameterList;
            ReturnClause = returnClause;
        }

        public JassSyntaxToken? ConstantToken { get; }

        public JassSyntaxToken FunctionToken { get; }

        public JassIdentifierNameSyntax IdentifierName { get; }

        public JassParameterListOrEmptyParameterListSyntax ParameterList { get; }

        public JassReturnClauseSyntax ReturnClause { get; }

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassFunctionDeclaratorSyntax functionDeclarator
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

            yield return FunctionToken;
        }

        public override IEnumerable<OneOf<JassSyntaxNode, JassSyntaxToken>> GetChildNodesAndTokens()
        {
            if (ConstantToken is not null)
            {
                yield return ConstantToken;
            }

            yield return FunctionToken;
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

            yield return FunctionToken;

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

        public override IEnumerable<OneOf<JassSyntaxNode, JassSyntaxToken>> GetDescendantNodesAndTokens()
        {
            if (ConstantToken is not null)
            {
                yield return ConstantToken;
            }

            yield return FunctionToken;

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

        public override string ToString() => $"{ConstantToken.OptionalSuffixed()}{FunctionToken} {IdentifierName} {ParameterList} {ReturnClause}";

        public override JassSyntaxToken GetFirstToken() => ConstantToken ?? FunctionToken;

        public override JassSyntaxToken GetLastToken() => ReturnClause.GetLastToken();

        protected internal override JassFunctionDeclaratorSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            if (ConstantToken is not null)
            {
                return new JassFunctionDeclaratorSyntax(
                    newToken,
                    FunctionToken,
                    IdentifierName,
                    ParameterList,
                    ReturnClause);
            }

            return new JassFunctionDeclaratorSyntax(
                null,
                newToken,
                IdentifierName,
                ParameterList,
                ReturnClause);
        }

        protected internal override JassFunctionDeclaratorSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassFunctionDeclaratorSyntax(
                ConstantToken,
                FunctionToken,
                IdentifierName,
                ParameterList,
                ReturnClause.ReplaceLastToken(newToken));
        }
    }
}
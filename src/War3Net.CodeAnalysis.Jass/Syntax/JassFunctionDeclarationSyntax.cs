// ------------------------------------------------------------------------------
// <copyright file="JassFunctionDeclarationSyntax.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassFunctionDeclarationSyntax : JassTopLevelDeclarationSyntax
    {
        internal JassFunctionDeclarationSyntax(
            JassFunctionDeclaratorSyntax functionDeclarator,
            ImmutableArray<JassStatementSyntax> statements,
            JassSyntaxToken endFunctionToken)
        {
            FunctionDeclarator = functionDeclarator;
            Statements = statements;
            EndFunctionToken = endFunctionToken;
        }

        public JassFunctionDeclaratorSyntax FunctionDeclarator { get; }

        public ImmutableArray<JassStatementSyntax> Statements { get; }

        public JassSyntaxToken EndFunctionToken { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.FunctionDeclaration;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassFunctionDeclarationSyntax functionDeclaration
                && FunctionDeclarator.IsEquivalentTo(functionDeclaration.FunctionDeclarator)
                && Statements.IsEquivalentTo(functionDeclaration.Statements);
        }

        public override void WriteTo(TextWriter writer)
        {
            FunctionDeclarator.WriteTo(writer);
            Statements.WriteTo(writer);
            EndFunctionToken.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            yield return FunctionDeclarator;

            foreach (var child in Statements)
            {
                yield return child;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return EndFunctionToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return FunctionDeclarator;

            foreach (var child in Statements)
            {
                yield return child;
            }

            yield return EndFunctionToken;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            yield return FunctionDeclarator;
            foreach (var descendant in FunctionDeclarator.GetDescendantNodes())
            {
                yield return descendant;
            }

            foreach (var descendant in Statements.GetDescendantNodes())
            {
                yield return descendant;
            }
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            foreach (var descendant in FunctionDeclarator.GetDescendantTokens())
            {
                yield return descendant;
            }

            foreach (var descendant in Statements.GetDescendantTokens())
            {
                yield return descendant;
            }

            yield return EndFunctionToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return FunctionDeclarator;
            foreach (var descendant in FunctionDeclarator.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            foreach (var descendant in Statements.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            yield return EndFunctionToken;
        }

        public override string ToString() => $"{FunctionDeclarator} [...]";

        public override JassSyntaxToken GetFirstToken() => FunctionDeclarator.GetFirstToken();

        public override JassSyntaxToken GetLastToken() => EndFunctionToken;

        protected internal override JassFunctionDeclarationSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassFunctionDeclarationSyntax(
                FunctionDeclarator.ReplaceFirstToken(newToken),
                Statements,
                EndFunctionToken);
        }

        protected internal override JassFunctionDeclarationSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassFunctionDeclarationSyntax(
                FunctionDeclarator,
                Statements,
                newToken);
        }
    }
}
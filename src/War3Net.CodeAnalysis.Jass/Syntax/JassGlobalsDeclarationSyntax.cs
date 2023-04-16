// ------------------------------------------------------------------------------
// <copyright file="JassGlobalsDeclarationSyntax.cs" company="Drake53">
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
    public class JassGlobalsDeclarationSyntax : JassTopLevelDeclarationSyntax
    {
        internal JassGlobalsDeclarationSyntax(
            JassSyntaxToken globalsToken,
            ImmutableArray<JassGlobalDeclarationSyntax> globalDeclarations,
            JassSyntaxToken endGlobalsToken)
        {
            GlobalsToken = globalsToken;
            GlobalDeclarations = globalDeclarations;
            EndGlobalsToken = endGlobalsToken;
        }

        public JassSyntaxToken GlobalsToken { get; }

        public ImmutableArray<JassGlobalDeclarationSyntax> GlobalDeclarations { get; }

        public JassSyntaxToken EndGlobalsToken { get; }

        public override JassSyntaxKind SyntaxKind => JassSyntaxKind.GlobalsDeclaration;

        public override bool IsEquivalentTo([NotNullWhen(true)] JassSyntaxNode? other)
        {
            return other is JassGlobalsDeclarationSyntax globalsDeclaration
                && GlobalDeclarations.IsEquivalentTo(globalsDeclaration.GlobalDeclarations);
        }

        public override void WriteTo(TextWriter writer)
        {
            GlobalsToken.WriteTo(writer);
            GlobalDeclarations.WriteTo(writer);
            EndGlobalsToken.WriteTo(writer);
        }

        public override IEnumerable<JassSyntaxNode> GetChildNodes()
        {
            return GlobalDeclarations;
        }

        public override IEnumerable<JassSyntaxToken> GetChildTokens()
        {
            yield return GlobalsToken;
            yield return EndGlobalsToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetChildNodesAndTokens()
        {
            yield return GlobalsToken;

            foreach (var child in GlobalDeclarations)
            {
                yield return child;
            }

            yield return EndGlobalsToken;
        }

        public override IEnumerable<JassSyntaxNode> GetDescendantNodes()
        {
            return GlobalDeclarations.GetDescendantNodes();
        }

        public override IEnumerable<JassSyntaxToken> GetDescendantTokens()
        {
            yield return GlobalsToken;

            foreach (var descendant in GlobalDeclarations.GetDescendantTokens())
            {
                yield return descendant;
            }

            yield return EndGlobalsToken;
        }

        public override IEnumerable<JassSyntaxNodeOrToken> GetDescendantNodesAndTokens()
        {
            yield return GlobalsToken;

            foreach (var descendant in GlobalDeclarations.GetDescendantNodesAndTokens())
            {
                yield return descendant;
            }

            yield return EndGlobalsToken;
        }

        public override string ToString() => $"{GlobalsToken} [...]";

        public override JassSyntaxToken GetFirstToken() => GlobalsToken;

        public override JassSyntaxToken GetLastToken() => EndGlobalsToken;

        protected internal override JassGlobalsDeclarationSyntax ReplaceFirstToken(JassSyntaxToken newToken)
        {
            return new JassGlobalsDeclarationSyntax(
                newToken,
                GlobalDeclarations,
                EndGlobalsToken);
        }

        protected internal override JassGlobalsDeclarationSyntax ReplaceLastToken(JassSyntaxToken newToken)
        {
            return new JassGlobalsDeclarationSyntax(
                GlobalsToken,
                GlobalDeclarations,
                newToken);
        }
    }
}
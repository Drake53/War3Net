// ------------------------------------------------------------------------------
// <copyright file="SyntaxTokenNormalizer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        private static readonly HashSet<JassSyntaxKind> _increaseIndentationSyntaxKinds = GetIncreaseIndentationSyntaxKinds();
        private static readonly HashSet<JassSyntaxKind> _decreaseIndentationSyntaxKinds = GetDecreaseIndentationSyntaxKinds();
        private static readonly HashSet<JassSyntaxKind> _requireNewlineSyntaxKinds = GetRequireNewlineSyntaxKinds();

        /// <inheritdoc/>
        protected override bool RewriteToken(JassSyntaxToken? token, [NotNullIfNotNull("token")] out JassSyntaxToken? result)
        {
            if (token is null)
            {
                result = null;
                return false;
            }

            _currentToken = token;

            if (_decreaseIndentationSyntaxKinds.Contains(_currentToken.SyntaxKind))
            {
                _currentLevelOfIndentation--;
            }

            var normalizedLeadingTrivia = RewriteLeadingTrivia(token.LeadingTrivia, out var leadingTrivia);

            if (_requireNewlineSyntaxKinds.Contains(_currentToken.SyntaxKind))
            {
                _requireNewlineTrivia = true;
            }

            if (_increaseIndentationSyntaxKinds.Contains(_currentToken.SyntaxKind))
            {
                _currentLevelOfIndentation++;
            }

            var normalizedTrailingTrivia = RewriteTrailingTrivia(token.TrailingTrivia, out var trailingTrivia);

            if (normalizedLeadingTrivia || normalizedTrailingTrivia)
            {
                result = new JassSyntaxToken(
                    leadingTrivia,
                    token.SyntaxKind,
                    token.Text,
                    trailingTrivia);

                _previousToken = result;
                _previousNode = _nodes[^1];
                _previousNodeParent = _nodes.Count > 1 ? _nodes[^2] : null;

                return true;
            }

            result = token;

            _previousToken = token;
            _previousNode = _nodes[^1];
            _previousNodeParent = _nodes.Count > 1 ? _nodes[^2] : null;

            return false;
        }

        private static HashSet<JassSyntaxKind> GetIncreaseIndentationSyntaxKinds()
        {
            return new HashSet<JassSyntaxKind>
            {
                JassSyntaxKind.ElseKeyword,
                JassSyntaxKind.GlobalsKeyword,
                JassSyntaxKind.LoopKeyword,
                JassSyntaxKind.ThenKeyword,
            };
        }

        private static HashSet<JassSyntaxKind> GetDecreaseIndentationSyntaxKinds()
        {
            return new HashSet<JassSyntaxKind>
            {
                JassSyntaxKind.ElseIfKeyword,
                JassSyntaxKind.ElseKeyword,
                JassSyntaxKind.EndFunctionKeyword,
                JassSyntaxKind.EndGlobalsKeyword,
                JassSyntaxKind.EndIfKeyword,
                JassSyntaxKind.EndLoopKeyword,
            };
        }

        private static HashSet<JassSyntaxKind> GetRequireNewlineSyntaxKinds()
        {
            return new HashSet<JassSyntaxKind>
            {
                JassSyntaxKind.ElseKeyword,
                JassSyntaxKind.EndFunctionKeyword,
                JassSyntaxKind.EndGlobalsKeyword,
                JassSyntaxKind.EndIfKeyword,
                JassSyntaxKind.EndLoopKeyword,
                JassSyntaxKind.GlobalsKeyword,
                JassSyntaxKind.LoopKeyword,
                JassSyntaxKind.ThenKeyword,
            };
        }
    }
}
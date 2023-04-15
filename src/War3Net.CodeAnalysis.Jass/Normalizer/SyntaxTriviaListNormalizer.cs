// ------------------------------------------------------------------------------
// <copyright file="SyntaxTriviaListNormalizer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;
using System.Linq;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    partial class JassSyntaxNormalizer
    {
        /// <inheritdoc/>
        protected override bool RewriteLeadingTrivia(JassSyntaxTriviaList triviaList, out JassSyntaxTriviaList result)
        {
            var triviaBuilder = ImmutableArray.CreateBuilder<JassSyntaxTrivia>();

            if (_requireNewlineTrivia)
            {
                triviaBuilder.Add(JassSyntaxTrivia.Newline);
                _encounteredAnyTextOnCurrentLine = false;
                _requireNewlineTrivia = false;
            }

            HandleExistingTrivia(triviaList, triviaBuilder);

            if (_encounteredAnyTextOnCurrentLine)
            {
                var requireSpace = true;

                if (_previousToken.SyntaxKind == JassSyntaxKind.OpenBracketToken ||
                    _currentToken.SyntaxKind == JassSyntaxKind.OpenBracketToken ||
                    _currentToken.SyntaxKind == JassSyntaxKind.CloseBracketToken ||
                    _currentToken.SyntaxKind == JassSyntaxKind.CommaToken)
                {
                    requireSpace = false;
                }
                else
                {
                    var currentNode = _nodes[^1];
                    if (currentNode is not null)
                    {
                        if (_currentToken.SyntaxKind == JassSyntaxKind.OpenParenToken)
                        {
                            requireSpace = currentNode.SyntaxKind == JassSyntaxKind.ParenthesizedExpression;
                        }
                        else if (_currentToken.SyntaxKind == JassSyntaxKind.CloseParenToken)
                        {
                            if (currentNode.SyntaxKind == JassSyntaxKind.ArgumentList &&
                                _nodes.Count > 1)
                            {
                                var currentNodeParent = _nodes[^2];
                                requireSpace = currentNodeParent.SyntaxKind == JassSyntaxKind.CallStatement && _addSpacesToCallStatementArgumentList;
                            }
                            else
                            {
                                requireSpace = false;
                            }
                        }
                    }

                    if (_previousNode is not null)
                    {
                        if (_previousNode.SyntaxKind == JassSyntaxKind.UnaryPlusExpression ||
                            _previousNode.SyntaxKind == JassSyntaxKind.UnaryMinusExpression)
                        {
                            requireSpace = false;
                        }
                        else if (_previousToken.SyntaxKind == JassSyntaxKind.OpenParenToken)
                        {
                            if (_previousNode.SyntaxKind == JassSyntaxKind.ArgumentList &&
                                _previousNodeParent is not null)
                            {
                                requireSpace = _previousNodeParent.SyntaxKind == JassSyntaxKind.CallStatement && _addSpacesToCallStatementArgumentList;
                            }
                            else
                            {
                                requireSpace = false;
                            }
                        }
                    }
                }

                if (requireSpace)
                {
                    triviaBuilder.Add(JassSyntaxTrivia.SingleSpace);
                }
            }
            else if (!string.IsNullOrEmpty(_currentToken.Text))
            {
                _encounteredAnyTextOnCurrentLine = true;
                if (_currentLevelOfIndentation > 0)
                {
                    triviaBuilder.Add(JassSyntaxFactory.WhitespaceTrivia(string.Concat(Enumerable.Repeat(_indentationString, _currentLevelOfIndentation))));
                }
            }

            result = JassSyntaxFactory.SyntaxTriviaList(triviaBuilder.ToImmutable());
            return true;
        }

        /// <inheritdoc/>
        protected override bool RewriteTrailingTrivia(JassSyntaxTriviaList triviaList, out JassSyntaxTriviaList result)
        {
            var triviaBuilder = ImmutableArray.CreateBuilder<JassSyntaxTrivia>();

            HandleExistingTrivia(triviaList, triviaBuilder);

            result = JassSyntaxFactory.SyntaxTriviaList(triviaBuilder.ToImmutable());
            return true;
        }

        private void HandleExistingTrivia(JassSyntaxTriviaList triviaList, ImmutableArray<JassSyntaxTrivia>.Builder triviaBuilder)
        {
            for (var i = 0; i < triviaList.Trivia.Length; i++)
            {
                var trivia = triviaList.Trivia[i];
                if (trivia.SyntaxKind == JassSyntaxKind.NewlineTrivia)
                {
                    triviaBuilder.Add(trivia);
                    _encounteredAnyTextOnCurrentLine = false;
                    _requireNewlineTrivia = false;
                }
                else if (trivia.SyntaxKind == JassSyntaxKind.SingleLineCommentTrivia)
                {
                    if (!_encounteredAnyTextOnCurrentLine)
                    {
                        _encounteredAnyTextOnCurrentLine = true;
                        if (_currentLevelOfIndentation > 0)
                        {
                            triviaBuilder.Add(JassSyntaxFactory.WhitespaceTrivia(string.Concat(Enumerable.Repeat(_indentationString, _currentLevelOfIndentation))));
                        }
                    }
                    else if (_previousToken.TrailingTrivia.Trivia.IsEmpty || _previousToken.TrailingTrivia.Trivia[^1].SyntaxKind != JassSyntaxKind.WhitespaceTrivia)
                    {
                        triviaBuilder.Add(JassSyntaxTrivia.SingleSpace);
                    }

                    if (_trimComments && char.IsWhiteSpace(trivia.Text[^1]))
                    {
                        triviaBuilder.Add(JassSyntaxFactory.SingleLineCommentTrivia(trivia.Text.TrimEnd()));
                    }
                    else
                    {
                        triviaBuilder.Add(trivia);
                    }

                    _requireNewlineTrivia = true;
                }
            }

            if (_requireNewlineTrivia)
            {
                triviaBuilder.Add(JassSyntaxTrivia.Newline);
                _encounteredAnyTextOnCurrentLine = false;
                _requireNewlineTrivia = false;
            }
        }
    }
}
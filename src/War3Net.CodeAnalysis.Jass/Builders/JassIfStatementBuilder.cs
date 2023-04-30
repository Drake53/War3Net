// ------------------------------------------------------------------------------
// <copyright file="JassIfStatementBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Builders
{
    public class JassIfStatementBuilder : JassStatementListSyntaxBuilder
    {
        private readonly ImmutableArray<JassElseIfClauseSyntax>.Builder _elseIfClausesBuilder;
        private readonly JassIfClauseDeclaratorSyntax _ifClauseDeclarator;

        private JassIfClauseSyntax? _ifClause;
        private JassElseIfClauseDeclaratorSyntax? _elseIfClauseDeclarator;
        private JassSyntaxToken? _elseToken;
        private JassElseClauseSyntax? _elseClause;

        public JassIfStatementBuilder(JassIfClauseDeclaratorSyntax ifClauseDeclarator)
        {
            _elseIfClausesBuilder = ImmutableArray.CreateBuilder<JassElseIfClauseSyntax>();
            _ifClauseDeclarator = ifClauseDeclarator;
        }

        public JassIfStatementSyntax ToIfStatement(JassSyntaxToken endIfToken)
        {
            JassSyntaxFactory.ThrowHelper.ThrowIfInvalidToken(endIfToken, JassSyntaxKind.EndIfKeyword);

            EndCurrentClause();

            return new JassIfStatementSyntax(
                _ifClause,
                _elseIfClausesBuilder.ToImmutable(),
                _elseClause,
                endIfToken);
        }

        public void BeginElseIfClause(JassElseIfClauseDeclaratorSyntax elseIfClauseDeclarator)
        {
            EndCurrentClause();
            if (_elseClause is not null)
            {
                throw new InvalidOperationException();
            }

            _elseIfClauseDeclarator = elseIfClauseDeclarator.PrependTrivia(BuildTriviaList());
        }

        public void BeginElseClause(JassSyntaxToken elseToken)
        {
            JassSyntaxFactory.ThrowHelper.ThrowIfInvalidToken(elseToken, JassSyntaxKind.ElseKeyword);

            EndCurrentClause();
            if (_elseClause is not null)
            {
                throw new InvalidOperationException();
            }

            _elseIfClauseDeclarator = null;
            _elseToken = elseToken.PrependTrivia(BuildTriviaList());
        }

        [MemberNotNull(nameof(_ifClause))]
        private void EndCurrentClause()
        {
            if (_ifClause is null)
            {
                _ifClause = new JassIfClauseSyntax(_ifClauseDeclarator, BuildStatementList());
            }
            else if (_elseIfClauseDeclarator is not null)
            {
                _elseIfClausesBuilder.Add(new JassElseIfClauseSyntax(_elseIfClauseDeclarator, BuildStatementList()));
            }
            else if (_elseToken is not null)
            {
                _elseClause = new JassElseClauseSyntax(_elseToken, BuildStatementList());
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
    }
}
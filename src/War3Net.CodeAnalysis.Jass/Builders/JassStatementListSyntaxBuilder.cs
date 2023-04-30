// ------------------------------------------------------------------------------
// <copyright file="JassStatementListSyntaxBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;

using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Builders
{
    public abstract class JassStatementListSyntaxBuilder : JassSyntaxBuilder
    {
        private readonly ImmutableArray<JassStatementSyntax>.Builder _statementsBuilder;

        private JassIfStatementBuilder? _ifStatementBuilder;
        private JassLoopStatementBuilder? _loopStatementBuilder;

        public JassStatementListSyntaxBuilder()
        {
            _statementsBuilder = ImmutableArray.CreateBuilder<JassStatementSyntax>();
        }

        public void AddStatement(JassStatementSyntax statement)
        {
            if (_ifStatementBuilder is not null ||
                _loopStatementBuilder is not null)
            {
                throw new InvalidOperationException();
            }

            _statementsBuilder.Add(statement.PrependTrivia(BuildTriviaList()));
        }

        public JassIfStatementBuilder BeginIfStatement(JassIfClauseDeclaratorSyntax ifClauseDeclarator)
        {
            if (_loopStatementBuilder is not null)
            {
                throw new InvalidOperationException();
            }

            return _ifStatementBuilder = new JassIfStatementBuilder(ifClauseDeclarator.PrependTrivia(BuildTriviaList()));
        }

        public void EndIfStatement(JassSyntaxToken endifToken)
        {
            if (_ifStatementBuilder is null)
            {
                throw new InvalidOperationException();
            }

            _statementsBuilder.Add(_ifStatementBuilder.ToIfStatement(endifToken));
            _ifStatementBuilder = null;
        }

        public JassLoopStatementBuilder BeginLoopStatement(JassSyntaxToken loopToken)
        {
            if (_ifStatementBuilder is not null)
            {
                throw new InvalidOperationException();
            }

            return _loopStatementBuilder = new JassLoopStatementBuilder(loopToken.PrependTrivia(BuildTriviaList()));
        }

        public void EndLoopStatement(JassSyntaxToken endLoopToken)
        {
            if (_loopStatementBuilder is null)
            {
                throw new InvalidOperationException();
            }

            _statementsBuilder.Add(_loopStatementBuilder.ToLoopStatement(endLoopToken));
            _loopStatementBuilder = null;
        }

        protected ImmutableArray<JassStatementSyntax> BuildStatementList()
        {
            if (_ifStatementBuilder is not null ||
                _loopStatementBuilder is not null)
            {
                throw new InvalidOperationException();
            }

            if (_statementsBuilder.Count == 0)
            {
                return ImmutableArray<JassStatementSyntax>.Empty;
            }

            var result = _statementsBuilder.ToImmutable();
            _statementsBuilder.Clear();
            return result;
        }
    }
}
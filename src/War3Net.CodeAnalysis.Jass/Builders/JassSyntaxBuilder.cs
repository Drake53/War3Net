// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Builders
{
    public abstract class JassSyntaxBuilder
    {
        private readonly ImmutableArray<JassSyntaxTrivia>.Builder _triviaBuilder;

        public JassSyntaxBuilder()
        {
            _triviaBuilder = ImmutableArray.CreateBuilder<JassSyntaxTrivia>();
        }

        public void AddTrivia(JassSyntaxTrivia trivia)
        {
            _triviaBuilder.Add(trivia);
        }

        public void AddTrivia(params JassSyntaxTrivia[] trivia)
        {
            _triviaBuilder.AddRange(trivia);
        }

        public void AddTrivia(IEnumerable<JassSyntaxTrivia> trivia)
        {
            _triviaBuilder.AddRange(trivia);
        }

        protected JassSyntaxTriviaList BuildTriviaList()
        {
            if (_triviaBuilder.Count == 0)
            {
                return JassSyntaxTriviaList.Empty;
            }

            var result = new JassSyntaxTriviaList(_triviaBuilder.ToImmutable());
            _triviaBuilder.Clear();
            return result;
        }
    }
}
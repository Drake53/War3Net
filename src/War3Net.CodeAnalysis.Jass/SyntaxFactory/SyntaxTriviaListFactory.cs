// ------------------------------------------------------------------------------
// <copyright file="SyntaxTriviaListFactory.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Collections.Immutable;

using Pidgin;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public static partial class JassSyntaxFactory
    {
        public static JassSyntaxTriviaList SyntaxTriviaList(JassSyntaxTrivia trivia)
        {
            return new JassSyntaxTriviaList(ImmutableArray.Create(trivia));
        }

        public static JassSyntaxTriviaList SyntaxTriviaList(params JassSyntaxTrivia[] trivia)
        {
            return new JassSyntaxTriviaList(trivia.ToImmutableArray());
        }

        public static JassSyntaxTriviaList SyntaxTriviaList(IEnumerable<JassSyntaxTrivia> trivia)
        {
            return new JassSyntaxTriviaList(trivia.ToImmutableArray());
        }

        public static JassSyntaxTriviaList SyntaxTriviaList(ImmutableArray<JassSyntaxTrivia> trivia)
        {
            return new JassSyntaxTriviaList(trivia);
        }

        public static JassSyntaxTriviaList ParseLeadingTrivia(string text)
        {
            return JassParser.Instance.LeadingTriviaListParser.ParseOrThrow(text);
        }

        public static JassSyntaxTriviaList ParseTrailingTrivia(string text)
        {
            return JassParser.Instance.TrailingTriviaListParser.ParseOrThrow(text);
        }
    }
}
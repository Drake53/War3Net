// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxTriviaList.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;
using System.IO;

namespace War3Net.CodeAnalysis.Jass.Syntax
{
    public class JassSyntaxTriviaList
    {
        public static readonly JassSyntaxTriviaList Empty = new(ImmutableArray<JassSyntaxTrivia>.Empty);
        public static readonly JassSyntaxTriviaList SingleSpace = new(ImmutableArray.Create(new JassSyntaxTrivia(JassSyntaxKind.WhitespaceTrivia, " ")));

        internal JassSyntaxTriviaList(
            ImmutableArray<JassSyntaxTrivia> trivia)
        {
            Trivia = trivia;
        }

        public ImmutableArray<JassSyntaxTrivia> Trivia { get; }

        public void WriteTo(TextWriter writer)
        {
            for (var i = 0; i < Trivia.Length; i++)
            {
                Trivia[i].WriteTo(writer);
            }
        }

        public override string ToString() => string.Concat(Trivia);
    }
}
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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

        public static JassSyntaxTriviaList AppendTriviaToList(ImmutableArray<JassSyntaxTrivia> triviaList, JassSyntaxTrivia trivia)
        {
            var builder = ImmutableArray.CreateBuilder<JassSyntaxTrivia>(triviaList.Length + 1);
            builder.AddRange(triviaList);
            builder.Add(trivia);
            return new JassSyntaxTriviaList(builder.MoveToImmutable());
        }

        public static JassSyntaxTriviaList PrependTriviaToList(JassSyntaxTrivia trivia, ImmutableArray<JassSyntaxTrivia> triviaList)
        {
            var builder = ImmutableArray.CreateBuilder<JassSyntaxTrivia>(triviaList.Length + 1);
            builder.Add(trivia);
            builder.AddRange(triviaList);
            return new JassSyntaxTriviaList(builder.MoveToImmutable());
        }

        public static JassSyntaxTriviaList ConcatTriviaLists(ImmutableArray<JassSyntaxTrivia> firstList, ImmutableArray<JassSyntaxTrivia> secondList)
        {
            var builder = ImmutableArray.CreateBuilder<JassSyntaxTrivia>(firstList.Length + secondList.Length);
            builder.AddRange(firstList);
            builder.AddRange(secondList);
            return new JassSyntaxTriviaList(builder.MoveToImmutable());
        }

        public static JassSyntaxTriviaList ConcatTriviaLists(IEnumerable<JassSyntaxTrivia> firstList, IEnumerable<JassSyntaxTrivia> secondList)
        {
            var builder = ImmutableArray.CreateBuilder<JassSyntaxTrivia>();
            builder.AddRange(firstList);
            builder.AddRange(secondList);
            return new JassSyntaxTriviaList(builder.ToImmutable());
        }

        public static JassSyntaxTriviaList ConcatTriviaLists(JassSyntaxTriviaList firstList, JassSyntaxTriviaList secondList)
        {
            return ConcatTriviaLists(firstList.Trivia, secondList.Trivia);
        }

        public static JassSyntaxTriviaList ConcatTriviaLists(JassSyntaxTriviaList firstList, JassSyntaxTriviaList secondList, JassSyntaxTriviaList thirdList)
        {
            var builder = ImmutableArray.CreateBuilder<JassSyntaxTrivia>(
                firstList.Trivia.Length +
                secondList.Trivia.Length +
                thirdList.Trivia.Length);

            builder.AddRange(firstList.Trivia);
            builder.AddRange(secondList.Trivia);
            builder.AddRange(thirdList.Trivia);

            return SyntaxTriviaList(builder.MoveToImmutable());
        }

        public static JassSyntaxTriviaList MergeTriviaLists(JassSyntaxTriviaList firstList, JassSyntaxTriviaList secondList, JassSyntaxTriviaList thirdList)
        {
            return MergeTrivia(firstList.Trivia.Concat(secondList.Trivia).Concat(thirdList.Trivia).ToList());
        }

        public static JassSyntaxTriviaList MergeTrivia(List<JassSyntaxTrivia> triviaList)
        {
            if (triviaList.Count == 0)
            {
                return JassSyntaxTriviaList.Empty;
            }

            if (triviaList.Count == 1)
            {
                return SyntaxTriviaList(ImmutableArray.Create(triviaList[0]));
            }

            var builder = ImmutableArray.CreateBuilder<JassSyntaxTrivia>();
            var aggregatedList = new List<JassSyntaxTrivia>();
            var kind = JassSyntaxKind.None;

            foreach (var trivia in triviaList)
            {
                if (trivia.SyntaxKind == kind)
                {
                    aggregatedList.Add(trivia);
                    continue;
                }

                if (aggregatedList.Count > 0)
                {
                    builder.Add(SyntaxTrivia(kind, string.Concat(aggregatedList.Select(t => t.Text))));
                    aggregatedList.Clear();
                }

                aggregatedList.Add(trivia);
                kind = trivia.SyntaxKind;
            }

            if (aggregatedList.Count > 0)
            {
                builder.Add(SyntaxTrivia(kind, string.Concat(aggregatedList.Select(t => t.Text))));
                aggregatedList.Clear();
            }

            return SyntaxTriviaList(builder.ToImmutable());
        }
    }
}
namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public class VJassSyntaxTriviaList
    {
        public static readonly VJassSyntaxTriviaList Empty = new(ImmutableArray<ISyntaxTrivia>.Empty);
        public static readonly VJassSyntaxTriviaList SingleSpace = new(ImmutableArray.Create<ISyntaxTrivia>(new VJassSyntaxTrivia(VJassSyntaxKind.WhitespaceTrivia, " ")));

        internal VJassSyntaxTriviaList(
            ImmutableArray<ISyntaxTrivia> trivia)
        {
            Trivia = trivia;
        }

        public ImmutableArray<ISyntaxTrivia> Trivia { get; }

        public void WriteTo(TextWriter writer)
        {
            for (var i = 0; i < Trivia.Length; i++)
            {
                Trivia[i].WriteTo(writer);
            }
        }

        public void ProcessTo(TextWriter writer, VJassPreprocessorContext context)
        {
            for (var i = 0; i < Trivia.Length; i++)
            {
                Trivia[i].ProcessTo(writer, context);
            }
        }

        public override string ToString() => string.Concat(Trivia);
    }
}
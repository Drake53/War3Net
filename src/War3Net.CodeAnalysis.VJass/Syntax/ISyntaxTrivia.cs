using System.IO;

namespace War3Net.CodeAnalysis.VJass.Syntax
{
    public interface ISyntaxTrivia
    {
        void WriteTo(TextWriter writer);

        void ProcessTo(TextWriter writer, VJassPreprocessorContext context);
    }
}
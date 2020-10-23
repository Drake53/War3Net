// ------------------------------------------------------------------------------
// <copyright file="JassParser.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Linq;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public class JassParser //: IDisposable
    {
        private readonly JassTokenizer _tokenizer;

        internal JassParser(JassTokenizer tokenizer/*, CancellationToken cancellationToken = default*/)
        {
            _tokenizer = tokenizer;
        }

        public static FileSyntax ParseFile(string filePath)
        {
            return ParseString(File.ReadAllText(filePath));
        }

        public static FileSyntax ParseStream(Stream stream, bool leaveOpen = false)
        {
            using var streamReader = new StreamReader(stream, leaveOpen: leaveOpen);
            return ParseString(streamReader.ReadToEnd());
        }

        public static FileSyntax ParseString(string text)
        {
            var tokenizer = new JassTokenizer(text);
            var parser = new JassParser(tokenizer);
            return parser.Parse();
        }

        public FileSyntax Parse()
        {
            var ps = new ParseState();
            ps.Position = 0;
            ps.Tokens = _tokenizer.Tokenize().ToList();

            return (FileSyntax.Parser.Get.Parse(ps).FirstOrDefault()?.Node as FileSyntax) ?? throw new InvalidDataException("Parsing failed. There may be syntax errors.");

            /*var r = 0;
            foreach (var parseResult in FileSyntax.Parser.Get.Parse(ps))
            {
                r++;
                return parseResult.Node as FileSyntax;
            }

            if (r == 0)
            {
                throw new Exception("Failed to parse");
            }

            if (r > 1)
            {
                throw new Exception($"Ambiguous ({r})");
            }*/
        }

        /*public void Dispose()
        {
        }*/
    }
}
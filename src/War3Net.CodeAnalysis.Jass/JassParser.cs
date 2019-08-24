// ------------------------------------------------------------------------------
// <copyright file="JassParser.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
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
            var tokenizer = new JassTokenizer(File.ReadAllText(filePath));
            var parser = new JassParser(tokenizer);
            return parser.Parse();
        }

        public FileSyntax Parse()
        {
            var ps = new ParseState();
            ps.Position = 0;
            ps.Tokens = _tokenizer.Tokenize().ToList();

            return FileSyntax.Parser.Get.Parse(ps).First().Node as FileSyntax;

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
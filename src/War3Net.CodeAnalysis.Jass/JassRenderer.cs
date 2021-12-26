// ------------------------------------------------------------------------------
// <copyright file="JassRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Linq;

namespace War3Net.CodeAnalysis.Jass
{
    public partial class JassRenderer
    {
        private readonly TextWriter _writer;
        private readonly JassRendererOptions _options;

        private int _currentIndentation;
        private bool _currentLineIndented;

        public JassRenderer(TextWriter writer)
        {
            _writer = writer;
            _options = JassRendererOptions.Default;
        }

        public JassRenderer(TextWriter writer, JassRendererOptions options)
        {
            _writer = writer;
            _options = options;
        }

        public void RenderNewLine() => WriteLine();

        private void Write(char c)
        {
            if (!_currentLineIndented)
            {
                WriteIndentation();
            }

            _writer.Write(c);
        }

        private void Write(string s)
        {
            if (!_currentLineIndented)
            {
                WriteIndentation();
            }

            _writer.Write(s);
        }

        private void WriteLine()
        {
            _writer.Write(_options.NewLineString);
            _currentLineIndented = false;
        }

        private void WriteLine(string s)
        {
            Write(s);
            WriteLine();
        }

        private void WriteIndentation()
        {
            _currentLineIndented = true;
            _writer.Write(string.Concat(Enumerable.Repeat(_options.IndentationString, _currentIndentation)));
        }

        private void Indent()
        {
            _currentIndentation++;
        }

        private void Outdent()
        {
            _currentIndentation--;
        }
    }
}
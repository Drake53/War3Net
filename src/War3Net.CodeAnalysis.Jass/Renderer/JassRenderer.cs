// ------------------------------------------------------------------------------
// <copyright file="JassRenderer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Renderer
{
    public partial class JassRenderer
    {
        private readonly TextWriter _writer;

        private JassRendererOptions _options;

        private Dictionary<string, NewExpressionSyntax> _constants;

        private int _currentIndentationLevel;
        private bool _currentLineIndented;

        public JassRenderer(TextWriter textWriter)
        {
            _writer = textWriter;

            _constants = new Dictionary<string, NewExpressionSyntax>();
        }

        public JassRendererOptions Options
        {
            get => _options;
            set => _options = value;
        }

        private void Indent()
        {
            _currentIndentationLevel++;
        }

        private void Outdent()
        {
            if (_currentIndentationLevel <= 0)
            {
                throw new InvalidOperationException();
            }

            _currentIndentationLevel--;
        }

        private void WriteNewline()
        {
            _writer.Write(_options.NewlineString);
            _currentLineIndented = false;
        }

        private void WriteSpace()
        {
            Write(" ");
        }

        private void Write(string s)
        {
            if (!_currentLineIndented)
            {
                _currentLineIndented = true;
                for (var i = 0; i < _currentIndentationLevel; i++)
                {
                    _writer.Write(_options.IndentationString);
                }
            }

            _writer.Write(s);
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="JassRenderer.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
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

        private Func<string, string> _identifierOptimizer;
        private Dictionary<string, NewExpressionSyntax> _constants;

        private string _indentation;
        private int _currentIndentationLevel;
        private bool _currentLineIndented;

        private string _newlineString;
        private bool _comments;
        private bool _optionalWhitespace;
        private bool _omitEmptyLines;
        private bool _inlineConstants;

        public JassRenderer(TextWriter textWriter)
        {
            _writer = textWriter;

            _newlineString = "\r\n";

            _identifierOptimizer = (s) => { return s; };
            _constants = new Dictionary<string, NewExpressionSyntax>();
        }

        /// <summary>
        /// Gets or sets the amount of spaces used for indentation. Use -1 to indent with tabs.
        /// </summary>
        public int Indentation
        {
            get => _indentation == "\t" ? -1 : _indentation.Length;
            set => _indentation = value == -1 ? "\t" : new string(' ', value);
        }

        public string NewlineString
        {
            get => _newlineString;
        }

        /// <summary>
        /// Gets or sets a value indicating whether comments should be rendered.
        /// </summary>
        public bool Comments
        {
            get => _comments;
            set => _comments = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether optional spacebars should be inserted to improve readability.
        /// </summary>
        public bool OptionalWhitespace
        {
            get => _optionalWhitespace;
            set => _optionalWhitespace = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether empty lines should be rendered or not.
        /// </summary>
        public bool OmitEmptyLines
        {
            get => _omitEmptyLines;
            set => _omitEmptyLines = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether global constants should be inlined.
        /// </summary>
        public bool InlineConstants
        {
            get => _inlineConstants;
            set => _inlineConstants = value;
        }

        public void SetNewlineString(bool controlReturn, bool lineFeed)
        {
            _newlineString = $"{(controlReturn ? "\r" : string.Empty)}{(lineFeed || !controlReturn ? "\n" : string.Empty)}";
        }

        /// <summary>
        /// Define a custom function that renames identifier names that are declared in the script.
        /// </summary>
        public void SetIdentifierOptimizerMethod(Func<string, string> func)
        {
            _identifierOptimizer = func;
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
            _writer.Write(_newlineString);
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
                    _writer.Write(_indentation);
                }
            }

            _writer.Write(s);
        }
    }
}
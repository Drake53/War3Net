// ------------------------------------------------------------------------------
// <copyright file="JassRendererOptions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass.Renderer
{
    public class JassRendererOptions
    {
        private Func<string, string> _identifierOptimizer;
        private string _newlineString;
        private string _indentation;
        private bool _comments;
        private bool _optionalWhitespace;
        private bool _omitEmptyLines;
        private bool _inlineConstants;

        public JassRendererOptions()
        {
            _identifierOptimizer = (s) => { return s; };
            _newlineString = "\r\n";
        }

        /// <summary>
        /// Gets the default settings for the <see cref="JassRenderer"/>.
        /// </summary>
        public static JassRendererOptions Default
        {
            get => new JassRendererOptions()
            {
                Indentation = 4,
                Comments = true,
                OptionalWhitespace = true,
                OmitEmptyLines = false,
                InlineConstants = false,
            };
        }

        public string NewlineString => _newlineString;

        public string IndentationString => _indentation;

        /// <summary>
        /// Gets or sets the amount of spaces used for indentation. Use -1 to indent with tabs.
        /// </summary>
        public int Indentation
        {
            get => _indentation == "\t" ? -1 : _indentation.Length;
            set => _indentation = value == -1 ? "\t" : new string(' ', value);
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

        public string InvokeOptimizer(string input)
        {
            return _identifierOptimizer.Invoke(input);
        }
    }
}
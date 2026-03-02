using System;
using System.IO;
using System.Text;

namespace War3Net.CodeAnalysis
{
    public sealed class IndentedTextWriter : TextWriter
    {
        private const string DefaultIndentString = "    ";

        private readonly TextWriter _writer;
        private readonly string _indentString;
        private int _indentLevel;
        private bool _needsIndent;

        public IndentedTextWriter(TextWriter writer)
            : this(writer, DefaultIndentString)
        {
        }

        public IndentedTextWriter(TextWriter writer, string indentString)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
            _indentString = indentString ?? throw new ArgumentNullException(nameof(indentString));
            _needsIndent = true;
        }

        /// <param name="writer">The existing <see cref="IndentedTextWriter"/> from which to copy indent and newline strings.</param>
        public static IndentedTextWriter New(IndentedTextWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var stringWriter = new StringWriter
            {
                NewLine = writer.NewLine,
            };

            return new IndentedTextWriter(stringWriter, writer.IndentString);
        }

        public override Encoding Encoding => _writer.Encoding;

        public override string NewLine => _writer.NewLine;

        public string IndentString => _indentString;

        public int IndentLevel
        {
            get => _indentLevel;
            set => _indentLevel = value < 0 ? 0 : value;
        }

        public void Indent() => _indentLevel++;

        public void Unindent()
        {
            if (_indentLevel == 0)
            {
                throw new InvalidOperationException("Cannot unindent when indent level is 0.");
            }

            _indentLevel--;
        }

        public override void Write(char value)
        {
            if (_needsIndent)
            {
                WriteIndent();
            }

            _writer.Write(value);

            _needsIndent = value == '\n' || value == '\r';
        }

        public override void Write(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (_needsIndent)
            {
                WriteIndent();
            }

            _writer.Write(value);

            _needsIndent = value.EndsWith('\n') || value.EndsWith('\r');
        }

        public override void WriteLine()
        {
            _writer.WriteLine();
            _needsIndent = true;
        }

        public override void WriteLine(string? value)
        {
            if (_needsIndent)
            {
                WriteIndent();
            }

            _writer.WriteLine(value);
            _needsIndent = true;
        }

        public override string? ToString() => _writer.ToString();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _writer.Dispose();
            }

            base.Dispose(disposing);
        }

        private void WriteIndent()
        {
            for (var i = 0; i < _indentLevel; i++)
            {
                _writer.Write(_indentString);
            }

            _needsIndent = false;
        }
    }
}
namespace War3Net.CodeAnalysis
{
    /// <summary>
    /// Represents the result of a parse operation, containing the parsed syntax and any diagnostics.
    /// </summary>
    /// <typeparam name="T">The type of the parsed syntax.</typeparam>
    public sealed class ParseResult<T>
        where T : class
    {
        public ParseResult(T value, ImmutableArray<Diagnostic> diagnostics)
        {
            Value = value;
            Diagnostics = diagnostics;
        }

        /// <summary>
        /// Gets the parsed syntax.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Gets the diagnostics produced during parsing.
        /// </summary>
        public ImmutableArray<Diagnostic> Diagnostics { get; }

        /// <summary>
        /// Gets a value indicating whether parsing succeeded (no errors).
        /// </summary>
        public bool Success => !HasErrors;

        /// <summary>
        /// Gets a value indicating whether there are any errors.
        /// </summary>
        public bool HasErrors => Diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error);

        /// <summary>
        /// Gets a value indicating whether there are any warnings.
        /// </summary>
        public bool HasWarnings => Diagnostics.Any(d => d.Severity == DiagnosticSeverity.Warning);
    }
}
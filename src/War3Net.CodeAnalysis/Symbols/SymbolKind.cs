namespace War3Net.CodeAnalysis.Symbols
{
    /// <summary>
    /// Specifies the kind of symbol.
    /// </summary>
    public enum SymbolKind
    {
        /// <summary>
        /// A type symbol (built-in or user-defined).
        /// </summary>
        Type,

        /// <summary>
        /// A function symbol.
        /// </summary>
        Function,

        /// <summary>
        /// A variable symbol (global or local).
        /// </summary>
        Variable,

        /// <summary>
        /// A function parameter symbol.
        /// </summary>
        Parameter,
    }
}
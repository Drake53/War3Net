namespace War3Net.CodeAnalysis
{
    /// <summary>
    /// Represents symbol binding information for a syntax node.
    /// </summary>
    public readonly struct SymbolInfo
    {
        /// <summary>
        /// Represents no symbol information.
        /// </summary>
        public static readonly SymbolInfo None = new(null, ImmutableArray<ISymbol>.Empty, CandidateReason.None);

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolInfo"/> struct.
        /// </summary>
        /// <param name="symbol">The bound symbol, or <see langword="null"/> if binding failed.</param>
        /// <param name="candidateSymbols">Candidate symbols if binding was ambiguous or failed.</param>
        /// <param name="candidateReason">The reason why binding failed, if applicable.</param>
        public SymbolInfo(ISymbol? symbol, ImmutableArray<ISymbol> candidateSymbols, CandidateReason candidateReason)
        {
            Symbol = symbol;
            CandidateSymbols = candidateSymbols.IsDefault ? ImmutableArray<ISymbol>.Empty : candidateSymbols;
            CandidateReason = candidateReason;
        }

        /// <summary>
        /// Gets the symbol that the syntax node refers to, if binding succeeded.
        /// </summary>
        public ISymbol? Symbol { get; }

        /// <summary>
        /// Gets candidate symbols if binding was ambiguous or failed.
        /// </summary>
        public ImmutableArray<ISymbol> CandidateSymbols { get; }

        /// <summary>
        /// Gets the reason why binding failed, if applicable.
        /// </summary>
        public CandidateReason CandidateReason { get; }
    }
}
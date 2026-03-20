namespace War3Net.CodeAnalysis
{
    /// <summary>
    /// Indicates the reason a symbol is a candidate rather than the resolved symbol.
    /// </summary>
    public enum CandidateReason
    {
        /// <summary>
        /// No candidate symbols. Either binding succeeded or there are no candidates.
        /// </summary>
        None = 0,

        /// <summary>
        /// Multiple symbols matched ambiguously.
        /// </summary>
        Ambiguous,

        /// <summary>
        /// The symbol was found but has the wrong number of arguments/parameters.
        /// </summary>
        WrongArity,

        /// <summary>
        /// The symbol was found but has incompatible types.
        /// </summary>
        WrongType,
    }
}
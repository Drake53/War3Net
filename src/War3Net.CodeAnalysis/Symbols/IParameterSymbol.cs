namespace War3Net.CodeAnalysis.Symbols
{
    /// <summary>
    /// Represents a parameter symbol.
    /// </summary>
    public interface IParameterSymbol : ISymbol
    {
        /// <summary>
        /// Gets the type of this parameter.
        /// </summary>
        ITypeSymbol Type { get; }

        /// <summary>
        /// Gets the ordinal position of this parameter.
        /// </summary>
        int Ordinal { get; }
    }
}
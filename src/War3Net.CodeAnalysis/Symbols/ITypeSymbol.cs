namespace War3Net.CodeAnalysis.Symbols
{
    /// <summary>
    /// Represents a type symbol.
    /// </summary>
    public interface ITypeSymbol : ISymbol
    {
        /// <summary>
        /// Gets the base type this type extends, if any.
        /// </summary>
        ITypeSymbol? BaseType { get; }

        /// <summary>
        /// Gets an enumerated value that identifies built-in primitive types.
        /// Returns <see cref="SpecialType.None"/> if the type is not special.
        /// </summary>
        SpecialType SpecialType { get; }
    }
}
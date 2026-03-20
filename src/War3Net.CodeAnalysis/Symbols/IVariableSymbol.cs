namespace War3Net.CodeAnalysis.Symbols
{
    /// <summary>
    /// Represents a variable symbol (global or local).
    /// </summary>
    public interface IVariableSymbol : ISymbol
    {
        /// <summary>
        /// Gets the type of this variable.
        /// </summary>
        ITypeSymbol Type { get; }

        /// <summary>
        /// Gets a value indicating whether this is a global variable.
        /// </summary>
        bool IsGlobal { get; }

        /// <summary>
        /// Gets a value indicating whether this is a constant.
        /// </summary>
        bool IsConstant { get; }

        /// <summary>
        /// Gets a value indicating whether this is an array.
        /// </summary>
        bool IsArray { get; }
    }
}
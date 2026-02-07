// ------------------------------------------------------------------------------
// <copyright file="ISymbol.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Diagnostics;

namespace War3Net.CodeAnalysis.Symbols
{
    /// <summary>
    /// Represents a symbol (type, function, variable, etc.) exposed by the semantic model.
    /// </summary>
    public interface ISymbol
    {
        /// <summary>
        /// Gets the kind of this symbol.
        /// </summary>
        SymbolKind Kind { get; }

        /// <summary>
        /// Gets the name of this symbol.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the containing symbol (e.g., containing type or function).
        /// </summary>
        ISymbol? ContainingSymbol { get; }

        /// <summary>
        /// Gets the location where this symbol is declared.
        /// </summary>
        Location Location { get; }
    }
}
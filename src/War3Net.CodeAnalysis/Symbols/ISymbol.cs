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

        /// <summary>
        /// Accepts the visitor by calling the appropriate Visit method.
        /// </summary>
        /// <param name="visitor">The visitor to accept.</param>
        void Accept(SymbolVisitor visitor);

        /// <summary>
        /// Accepts the visitor by calling the appropriate Visit method and returns the result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="visitor">The visitor to accept.</param>
        /// <returns>The result of visiting this symbol.</returns>
        TResult? Accept<TResult>(SymbolVisitor<TResult> visitor);

        /// <summary>
        /// Accepts the visitor by calling the appropriate Visit method with an additional argument and returns the result.
        /// </summary>
        /// <typeparam name="TArgument">The type of the additional argument.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="visitor">The visitor to accept.</param>
        /// <param name="argument">An additional argument passed to the visit method.</param>
        /// <returns>The result of visiting this symbol.</returns>
        TResult Accept<TArgument, TResult>(SymbolVisitor<TArgument, TResult> visitor, TArgument argument);
    }
}
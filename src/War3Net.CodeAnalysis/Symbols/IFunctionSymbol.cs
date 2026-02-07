// ------------------------------------------------------------------------------
// <copyright file="IFunctionSymbol.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;

namespace War3Net.CodeAnalysis.Symbols
{
    /// <summary>
    /// Represents a function symbol.
    /// </summary>
    public interface IFunctionSymbol : ISymbol
    {
        /// <summary>
        /// Gets what kind of method this is.
        /// </summary>
        MethodKind MethodKind { get; }

        /// <summary>
        /// Gets a value indicating whether this function is constant.
        /// </summary>
        bool IsConstant { get; }

        /// <summary>
        /// Gets a value indicating whether this function is a native function.
        /// </summary>
        bool IsNative { get; }

        /// <summary>
        /// Gets the parameters of this function.
        /// </summary>
        ImmutableArray<IParameterSymbol> Parameters { get; }

        /// <summary>
        /// Gets the return type of this function.
        /// </summary>
        ITypeSymbol ReturnType { get; }

        /// <summary>
        /// Gets a value indicating whether this method has no return type; i.e., returns "nothing".
        /// </summary>
        bool ReturnsNothing { get; }
    }
}
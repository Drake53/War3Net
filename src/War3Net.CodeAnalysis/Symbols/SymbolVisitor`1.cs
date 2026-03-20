namespace War3Net.CodeAnalysis.Symbols
{
    /// <summary>
    /// Represents a <see cref="ISymbol"/> visitor that visits only the single symbol passed into its <see cref="Visit(ISymbol?)"/> method
    /// and produces a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="TResult">The type of the return value of the visitor's visit methods.</typeparam>
    public abstract class SymbolVisitor<TResult>
    {
        /// <summary>
        /// Called when visiting a symbol. Dispatches to the specific visit method for the symbol type.
        /// </summary>
        /// <param name="symbol">The symbol to visit.</param>
        /// <returns>The result of visiting the symbol.</returns>
        public virtual TResult? Visit(ISymbol? symbol)
        {
            if (symbol is not null)
            {
                return symbol.Accept(this);
            }

            return default;
        }

        /// <summary>
        /// Called when visiting a symbol that does not have a specific visit method.
        /// </summary>
        /// <param name="symbol">The symbol being visited.</param>
        /// <returns>The default result.</returns>
        public virtual TResult? DefaultVisit(ISymbol symbol)
        {
            return default;
        }

        /// <summary>
        /// Called when visiting a <see cref="IFunctionSymbol"/>.
        /// </summary>
        /// <param name="symbol">The symbol to visit.</param>
        /// <returns>The result of visiting the symbol.</returns>
        public virtual TResult? VisitFunction(IFunctionSymbol symbol) => DefaultVisit(symbol);

        /// <summary>
        /// Called when visiting a <see cref="IParameterSymbol"/>.
        /// </summary>
        /// <param name="symbol">The symbol to visit.</param>
        /// <returns>The result of visiting the symbol.</returns>
        public virtual TResult? VisitParameter(IParameterSymbol symbol) => DefaultVisit(symbol);

        /// <summary>
        /// Called when visiting a <see cref="ITypeSymbol"/>.
        /// </summary>
        /// <param name="symbol">The symbol to visit.</param>
        /// <returns>The result of visiting the symbol.</returns>
        public virtual TResult? VisitType(ITypeSymbol symbol) => DefaultVisit(symbol);

        /// <summary>
        /// Called when visiting a <see cref="IVariableSymbol"/>.
        /// </summary>
        /// <param name="symbol">The symbol to visit.</param>
        /// <returns>The result of visiting the symbol.</returns>
        public virtual TResult? VisitVariable(IVariableSymbol symbol) => DefaultVisit(symbol);
    }
}
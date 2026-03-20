namespace War3Net.CodeAnalysis.Symbols
{
    /// <summary>
    /// Represents a <see cref="ISymbol"/> visitor that visits only the single symbol passed into its <see cref="Visit(ISymbol?, TArgument)"/> method,
    /// with an additional argument of the type specified by the <typeparamref name="TArgument"/> parameter,
    /// and produces a value of the type specified by the <typeparamref name="TResult"/> parameter.
    /// </summary>
    /// <typeparam name="TArgument">The type of the additional argument passed to the visitor's visit methods.</typeparam>
    /// <typeparam name="TResult">The type of the return value of the visitor's visit methods.</typeparam>
    public abstract class SymbolVisitor<TArgument, TResult>
    {
        /// <summary>
        /// Gets the default result to return when a symbol is <see langword="null"/> or when the visit method is not overridden.
        /// </summary>
        protected abstract TResult DefaultResult { get; }

        /// <summary>
        /// Called when visiting a symbol. Dispatches to the specific visit method for the symbol type.
        /// </summary>
        /// <param name="symbol">The symbol to visit.</param>
        /// <param name="argument">An additional argument passed to the visit method.</param>
        /// <returns>The result of visiting the symbol.</returns>
        public virtual TResult Visit(ISymbol? symbol, TArgument argument)
        {
            if (symbol is not null)
            {
                return symbol.Accept(this, argument);
            }

            return DefaultResult;
        }

        /// <summary>
        /// Called when visiting a symbol that does not have a specific visit method.
        /// </summary>
        /// <param name="symbol">The symbol being visited.</param>
        /// <param name="argument">An additional argument passed to the visit method.</param>
        /// <returns>The default result.</returns>
        public virtual TResult DefaultVisit(ISymbol symbol, TArgument argument)
        {
            return DefaultResult;
        }

        /// <summary>
        /// Called when visiting a <see cref="IFunctionSymbol"/>.
        /// </summary>
        /// <param name="symbol">The symbol to visit.</param>
        /// <param name="argument">An additional argument passed to the visit method.</param>
        /// <returns>The result of visiting the symbol.</returns>
        public virtual TResult VisitFunction(IFunctionSymbol symbol, TArgument argument) => DefaultVisit(symbol, argument);

        /// <summary>
        /// Called when visiting a <see cref="IParameterSymbol"/>.
        /// </summary>
        /// <param name="symbol">The symbol to visit.</param>
        /// <param name="argument">An additional argument passed to the visit method.</param>
        /// <returns>The result of visiting the symbol.</returns>
        public virtual TResult VisitParameter(IParameterSymbol symbol, TArgument argument) => DefaultVisit(symbol, argument);

        /// <summary>
        /// Called when visiting a <see cref="ITypeSymbol"/>.
        /// </summary>
        /// <param name="symbol">The symbol to visit.</param>
        /// <param name="argument">An additional argument passed to the visit method.</param>
        /// <returns>The result of visiting the symbol.</returns>
        public virtual TResult VisitType(ITypeSymbol symbol, TArgument argument) => DefaultVisit(symbol, argument);

        /// <summary>
        /// Called when visiting a <see cref="IVariableSymbol"/>.
        /// </summary>
        /// <param name="symbol">The symbol to visit.</param>
        /// <param name="argument">An additional argument passed to the visit method.</param>
        /// <returns>The result of visiting the symbol.</returns>
        public virtual TResult VisitVariable(IVariableSymbol symbol, TArgument argument) => DefaultVisit(symbol, argument);
    }
}
namespace War3Net.CodeAnalysis.Symbols
{
    /// <summary>
    /// Represents a <see cref="ISymbol"/> visitor that visits only the single symbol passed into its <see cref="Visit(ISymbol?)"/> method.
    /// </summary>
    public abstract class SymbolVisitor
    {
        /// <summary>
        /// Called when visiting a symbol. Dispatches to the specific visit method for the symbol type.
        /// </summary>
        /// <param name="symbol">The symbol to visit.</param>
        public virtual void Visit(ISymbol? symbol)
        {
            symbol?.Accept(this);
        }

        /// <summary>
        /// Called when visiting a symbol that does not have a specific visit method.
        /// </summary>
        /// <param name="symbol">The symbol being visited.</param>
        public virtual void DefaultVisit(ISymbol symbol)
        {
        }

        /// <summary>
        /// Called when visiting a <see cref="IFunctionSymbol"/>.
        /// </summary>
        /// <param name="symbol">The symbol to visit.</param>
        public virtual void VisitFunction(IFunctionSymbol symbol) => DefaultVisit(symbol);

        /// <summary>
        /// Called when visiting a <see cref="IParameterSymbol"/>.
        /// </summary>
        /// <param name="symbol">The symbol to visit.</param>
        public virtual void VisitParameter(IParameterSymbol symbol) => DefaultVisit(symbol);

        /// <summary>
        /// Called when visiting a <see cref="ITypeSymbol"/>.
        /// </summary>
        /// <param name="symbol">The symbol to visit.</param>
        public virtual void VisitType(ITypeSymbol symbol) => DefaultVisit(symbol);

        /// <summary>
        /// Called when visiting a <see cref="IVariableSymbol"/>.
        /// </summary>
        /// <param name="symbol">The symbol to visit.</param>
        public virtual void VisitVariable(IVariableSymbol symbol) => DefaultVisit(symbol);
    }
}
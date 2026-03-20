namespace War3Net.CodeAnalysis.Symbols
{
    /// <summary>
    /// Specifies the kind of method.
    /// </summary>
    public enum MethodKind
    {
        /// <summary>
        /// Method is a regular function.
        /// </summary>
        Function,

        /// <summary>
        /// Method is a native function.
        /// </summary>
        Native,

        /// <summary>
        /// Method is a constructor.
        /// </summary>
        Constructor,

        /// <summary>
        /// Method is a built-in operator.
        /// </summary>
        BuiltInOperator,
    }
}
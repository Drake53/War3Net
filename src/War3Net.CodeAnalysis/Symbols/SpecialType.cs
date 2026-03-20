namespace War3Net.CodeAnalysis.Symbols
{
    /// <summary>
    /// Identifies special built-in types.
    /// </summary>
    public enum SpecialType
    {
        /// <summary>
        /// Not a special type.
        /// </summary>
        None,

        /// <summary>
        /// The integer type.
        /// </summary>
        Integer,

        /// <summary>
        /// The real (floating-point) type.
        /// </summary>
        Real,

        /// <summary>
        /// The boolean type.
        /// </summary>
        Boolean,

        /// <summary>
        /// The string type.
        /// </summary>
        String,

        /// <summary>
        /// The handle type (base type for game objects).
        /// </summary>
        Handle,

        /// <summary>
        /// The code type (function pointer).
        /// </summary>
        Code,

        /// <summary>
        /// The nothing type (void return type).
        /// </summary>
        Nothing,
    }
}
namespace War3Net.CodeAnalysis
{
    /// <summary>
    /// Represents type information for an expression.
    /// </summary>
    public readonly struct TypeInfo : IEquatable<TypeInfo>
    {
        /// <summary>
        /// Represents no type information.
        /// </summary>
        public static readonly TypeInfo None = new(null);

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeInfo"/> struct.
        /// </summary>
        /// <param name="type">The type of the expression.</param>
        public TypeInfo(ITypeSymbol? type)
        {
            Type = type;
        }

        /// <summary>
        /// Gets the type of the expression, or <see langword="null"/> if the type could not be determined.
        /// </summary>
        public ITypeSymbol? Type { get; }

        public static bool operator ==(TypeInfo left, TypeInfo right) => left.Equals(right);

        public static bool operator !=(TypeInfo left, TypeInfo right) => !left.Equals(right);

        /// <inheritdoc/>
        public bool Equals(TypeInfo other) => Equals(Type, other.Type);

        /// <inheritdoc/>
        public override bool Equals(object? obj) => obj is TypeInfo other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => Type?.GetHashCode() ?? 0;
    }
}
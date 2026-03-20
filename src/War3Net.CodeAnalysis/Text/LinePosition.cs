namespace War3Net.CodeAnalysis.Text
{
    /// <summary>
    /// Represents a position in a source document as a line number and character offset.
    /// </summary>
    public readonly struct LinePosition : IEquatable<LinePosition>, IComparable<LinePosition>
    {
        /// <summary>
        /// A <see cref="LinePosition"/> representing position (0, 0).
        /// </summary>
        public static readonly LinePosition Zero = new(0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="LinePosition"/> struct.
        /// </summary>
        /// <param name="line">The zero-based line number.</param>
        /// <param name="character">The zero-based character offset within the line.</param>
        public LinePosition(int line, int character)
        {
            if (line < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(line), "Line must be non-negative.");
            }

            if (character < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(character), "Character must be non-negative.");
            }

            Line = line;
            Character = character;
        }

        /// <summary>
        /// Gets the zero-based line number.
        /// </summary>
        public int Line { get; }

        /// <summary>
        /// Gets the zero-based character offset within the line.
        /// </summary>
        public int Character { get; }

        public static bool operator ==(LinePosition left, LinePosition right) => left.Equals(right);

        public static bool operator !=(LinePosition left, LinePosition right) => !left.Equals(right);

        public static bool operator <(LinePosition left, LinePosition right) => left.CompareTo(right) < 0;

        public static bool operator <=(LinePosition left, LinePosition right) => left.CompareTo(right) <= 0;

        public static bool operator >(LinePosition left, LinePosition right) => left.CompareTo(right) > 0;

        public static bool operator >=(LinePosition left, LinePosition right) => left.CompareTo(right) >= 0;

        /// <inheritdoc/>
        public int CompareTo(LinePosition other)
        {
            var lineComparison = Line.CompareTo(other.Line);
            return lineComparison != 0 ? lineComparison : Character.CompareTo(other.Character);
        }

        /// <inheritdoc/>
        public bool Equals(LinePosition other)
        {
            return Line == other.Line && Character == other.Character;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is LinePosition position && Equals(position);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Line, Character);
        }

        /// <inheritdoc/>
        public override string ToString() => $"({Line + 1},{Character + 1})";
    }
}
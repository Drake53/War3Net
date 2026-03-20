namespace War3Net.CodeAnalysis.Text
{
    /// <summary>
    /// Represents an immutable span of text in a source document.
    /// </summary>
    public readonly struct TextSpan : IEquatable<TextSpan>, IComparable<TextSpan>
    {
        /// <summary>
        /// Represents an empty text span at position 0.
        /// </summary>
        public static readonly TextSpan Empty = new(0, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="TextSpan"/> struct.
        /// </summary>
        /// <param name="start">The start position of the span.</param>
        /// <param name="length">The length of the span.</param>
        public TextSpan(int start, int length)
        {
            if (start < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start), "Start must be non-negative.");
            }

            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be non-negative.");
            }

            Start = start;
            Length = length;
        }

        /// <summary>
        /// Gets the start position of the span.
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// Gets the length of the span.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Gets the end position of the span (exclusive).
        /// </summary>
        public int End => Start + Length;

        /// <summary>
        /// Gets a value indicating whether this span is empty.
        /// </summary>
        public bool IsEmpty => Length == 0;

        public static bool operator ==(TextSpan left, TextSpan right) => left.Equals(right);

        public static bool operator !=(TextSpan left, TextSpan right) => !left.Equals(right);

        public static bool operator <(TextSpan left, TextSpan right) => left.CompareTo(right) < 0;

        public static bool operator <=(TextSpan left, TextSpan right) => left.CompareTo(right) <= 0;

        public static bool operator >(TextSpan left, TextSpan right) => left.CompareTo(right) > 0;

        public static bool operator >=(TextSpan left, TextSpan right) => left.CompareTo(right) >= 0;

        /// <summary>
        /// Creates a <see cref="TextSpan"/> from start and end positions.
        /// </summary>
        /// <param name="start">The start position.</param>
        /// <param name="end">The end position (exclusive).</param>
        /// <returns>A new <see cref="TextSpan"/>.</returns>
        public static TextSpan FromBounds(int start, int end)
        {
            if (start < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(start), "Start must be non-negative.");
            }

            if (end < start)
            {
                throw new ArgumentOutOfRangeException(nameof(end), "End must be greater than or equal to start.");
            }

            return new TextSpan(start, end - start);
        }

        /// <summary>
        /// Determines whether this span contains the specified position.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns><see langword="true"/> if the <paramref name="position"/> is within this span; otherwise, <see langword="false"/>.</returns>
        public bool Contains(int position)
        {
            return position >= Start && position < End;
        }

        /// <summary>
        /// Determines whether this span completely contains the specified span.
        /// </summary>
        /// <param name="span">The span to check.</param>
        /// <returns><see langword="true"/> if the <paramref name="span"/> is contained within this span; otherwise, <see langword="false"/>.</returns>
        public bool Contains(TextSpan span)
        {
            return span.Start >= Start && span.End <= End;
        }

        /// <summary>
        /// Determines whether this span overlaps with the specified span.
        /// </summary>
        /// <param name="span">The span to check.</param>
        /// <returns><see langword="true"/> if the spans overlap; otherwise, <see langword="false"/>.</returns>
        public bool OverlapsWith(TextSpan span)
        {
            return Start < span.End && End > span.Start;
        }

        /// <summary>
        /// Gets the overlap between this span and the specified span.
        /// </summary>
        /// <param name="span">The span to get the overlap with.</param>
        /// <returns>The overlapping span, or <see langword="null"/> if there is no overlap.</returns>
        public TextSpan? Overlap(TextSpan span)
        {
            var overlapStart = Math.Max(Start, span.Start);
            var overlapEnd = Math.Min(End, span.End);

            return overlapStart < overlapEnd
                ? FromBounds(overlapStart, overlapEnd)
                : null;
        }

        /// <summary>
        /// Determines whether this span intersects with the specified span.
        /// </summary>
        /// <param name="span">The span to check for intersection.</param>
        /// <returns><see langword="true"/> if the spans intersect; otherwise, <see langword="false"/>.</returns>
        public bool IntersectsWith(TextSpan span)
        {
            return Start <= span.End && End >= span.Start;
        }

        /// <summary>
        /// Gets the intersection between this span and the specified span.
        /// </summary>
        /// <param name="span">The span to get the intersection with.</param>
        /// <returns>The intersecting span, or <see langword="null"/> if there is no intersection.</returns>
        public TextSpan? Intersection(TextSpan span)
        {
            var intersectionStart = Math.Max(Start, span.Start);
            var intersectionEnd = Math.Min(End, span.End);

            return intersectionStart <= intersectionEnd
                ? FromBounds(intersectionStart, intersectionEnd)
                : null;
        }

        /// <inheritdoc/>
        public int CompareTo(TextSpan other)
        {
            var comparison = Start.CompareTo(other.Start);
            return comparison != 0 ? comparison : Length.CompareTo(other.Length);
        }

        /// <inheritdoc/>
        public bool Equals(TextSpan other)
        {
            return Start == other.Start && Length == other.Length;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is TextSpan span && Equals(span);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Start, Length);
        }

        /// <inheritdoc/>
        public override string ToString() => $"[{Start}..{End})";
    }
}
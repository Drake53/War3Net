// ------------------------------------------------------------------------------
// <copyright file="LinePositionSpan.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Text
{
    /// <summary>
    /// Represents a span of text in a source document as a range of line positions.
    /// </summary>
    public readonly struct LinePositionSpan : IEquatable<LinePositionSpan>
    {
        /// <summary>
        /// A span at position (0,0) with zero length.
        /// </summary>
        public static readonly LinePositionSpan Default = new(LinePosition.Zero, LinePosition.Zero);

        /// <summary>
        /// Initializes a new instance of the <see cref="LinePositionSpan"/> struct.
        /// </summary>
        /// <param name="start">The start position of the span.</param>
        /// <param name="end">The end position of the span.</param>
        public LinePositionSpan(LinePosition start, LinePosition end)
        {
            if (end < start)
            {
                throw new ArgumentException("End position must be greater than or equal to start position.", nameof(end));
            }

            Start = start;
            End = end;
        }

        /// <summary>
        /// Gets the start position of the span.
        /// </summary>
        public LinePosition Start { get; }

        /// <summary>
        /// Gets the end position of the span.
        /// </summary>
        public LinePosition End { get; }

        public static bool operator ==(LinePositionSpan left, LinePositionSpan right) => left.Equals(right);

        public static bool operator !=(LinePositionSpan left, LinePositionSpan right) => !left.Equals(right);

        /// <inheritdoc/>
        public bool Equals(LinePositionSpan other)
        {
            return Start.Equals(other.Start) && End.Equals(other.End);
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is LinePositionSpan span && Equals(span);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Start, End);
        }

        /// <inheritdoc/>
        public override string ToString() => $"{Start}-{End}";
    }
}
// ------------------------------------------------------------------------------
// <copyright file="Location.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.CodeAnalysis.Text;

namespace War3Net.CodeAnalysis.Diagnostics
{
    /// <summary>
    /// Represents a location in source code.
    /// </summary>
    public sealed class Location : IEquatable<Location>
    {
        /// <summary>
        /// A location with no information.
        /// </summary>
        public static readonly Location None = new(TextSpan.Empty, LinePositionSpan.Default, null);

        private Location(TextSpan sourceSpan, LinePositionSpan lineSpan, string? filePath)
        {
            SourceSpan = sourceSpan;
            LineSpan = lineSpan;
            FilePath = filePath;
        }

        /// <summary>
        /// Gets the source text span of this location.
        /// </summary>
        public TextSpan SourceSpan { get; }

        /// <summary>
        /// Gets the line span of this location.
        /// </summary>
        public LinePositionSpan LineSpan { get; }

        /// <summary>
        /// Gets the file path, or <see langword="null"/> if unknown.
        /// </summary>
        public string? FilePath { get; }

        /// <summary>
        /// Creates a location with a source span and line positions.
        /// </summary>
        /// <param name="sourceSpan">The source text span.</param>
        /// <param name="lineSpan">The line position span.</param>
        /// <param name="filePath">The optional file path.</param>
        /// <returns>A new <see cref="Location"/>.</returns>
        public static Location Create(TextSpan sourceSpan, LinePositionSpan lineSpan, string? filePath = null)
        {
            return new Location(sourceSpan, lineSpan, filePath);
        }

        /// <summary>
        /// Creates a location from just a text span (line positions unknown).
        /// </summary>
        /// <param name="sourceSpan">The source text span.</param>
        /// <param name="filePath">The optional file path.</param>
        /// <returns>A new <see cref="Location"/>.</returns>
        public static Location Create(TextSpan sourceSpan, string? filePath = null)
        {
            return new Location(sourceSpan, LinePositionSpan.Default, filePath);
        }

        /// <summary>
        /// Creates a location from Pidgin error position information.
        /// </summary>
        /// <param name="errorOffset">The absolute offset in the input.</param>
        /// <param name="line">The 1-based line number from Pidgin.</param>
        /// <param name="col">The 1-based column number from Pidgin.</param>
        /// <param name="filePath">The optional file path.</param>
        /// <returns>A new <see cref="Location"/>.</returns>
        public static Location FromPidginError(int errorOffset, int line, int col, string? filePath = null)
        {
            // Pidgin uses 1-based lines/cols; convert to 0-based
            var linePosition = new LinePosition(Math.Max(0, line - 1), Math.Max(0, col - 1));
            var lineSpan = new LinePositionSpan(linePosition, linePosition);
            var sourceSpan = new TextSpan(errorOffset, 0); // Point location (zero length)
            return new Location(sourceSpan, lineSpan, filePath);
        }

        /// <inheritdoc/>
        public bool Equals(Location? other)
        {
            return other is not null
                && SourceSpan.Equals(other.SourceSpan)
                && LineSpan.Equals(other.LineSpan)
                && FilePath == other.FilePath;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is Location location && Equals(location);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(
                SourceSpan.GetHashCode(),
                LineSpan.GetHashCode(),
                FilePath?.GetHashCode(StringComparison.Ordinal) ?? 0);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            // Display as 1-based line/column for user-friendly output
            var line = LineSpan.Start.Line + 1;
            var col = LineSpan.Start.Character + 1;

            return FilePath is not null
                ? $"{FilePath}({line},{col})"
                : $"({line},{col})";
        }
    }
}
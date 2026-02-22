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
            return $"{FilePath ?? "?"}{LineSpan.Start}";
        }
    }
}
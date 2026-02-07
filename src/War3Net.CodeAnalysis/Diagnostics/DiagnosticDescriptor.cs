// ------------------------------------------------------------------------------
// <copyright file="DiagnosticDescriptor.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Diagnostics
{
    /// <summary>
    /// Provides a description about a <see cref="Diagnostic"/>.
    /// </summary>
    public sealed class DiagnosticDescriptor : IEquatable<DiagnosticDescriptor>
    {
        private DiagnosticDescriptor(
            string id,
            string title,
            string messageFormat,
            string category,
            DiagnosticSeverity defaultSeverity,
            bool isEnabledByDefault,
            bool isNotConfigurable,
            string description,
            string helpLinkUri)
        {
            Id = id;
            Title = title;
            MessageFormat = messageFormat;
            Category = category;
            DefaultSeverity = defaultSeverity;
            IsEnabledByDefault = isEnabledByDefault;
            IsNotConfigurable = isNotConfigurable;
            Description = description;
            HelpLinkUri = helpLinkUri;
        }

        /// <summary>
        /// Gets a unique identifier for the diagnostic.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets a short title describing the diagnostic.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Gets an optional longer description of the diagnostic.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets an optional hyperlink that provides more detailed information.
        /// </summary>
        public string HelpLinkUri { get; }

        /// <summary>
        /// Gets a format message string, which may contain placeholders.
        /// </summary>
        public string MessageFormat { get; }

        /// <summary>
        /// Gets the category of the diagnostic (e.g., "Syntax", "Semantic").
        /// </summary>
        public string Category { get; }

        /// <summary>
        /// Gets the default severity of the diagnostic.
        /// </summary>
        public DiagnosticSeverity DefaultSeverity { get; }

        /// <summary>
        /// Gets a value indicating whether the diagnostic is enabled by default.
        /// </summary>
        public bool IsEnabledByDefault { get; }

        /// <summary>
        /// Gets a value indicating whether the diagnostic's severity can be configured.
        /// </summary>
        public bool IsNotConfigurable { get; }

        /// <summary>
        /// Creates a new <see cref="DiagnosticDescriptor"/> that is not configurable.
        /// The severity is always <see cref="DiagnosticSeverity.Error"/> and the diagnostic is always enabled.
        /// </summary>
        /// <param name="id">A unique identifier for the diagnostic.</param>
        /// <param name="title">A short title describing the diagnostic.</param>
        /// <param name="messageFormat">A format message string, which may contain placeholders.</param>
        /// <param name="category">The category of the diagnostic (e.g., "Syntax", "Semantic").</param>
        /// <param name="description">An optional longer description of the diagnostic.</param>
        /// <param name="helpLinkUri">An optional hyperlink that provides more detailed information.</param>
        /// <returns>A new <see cref="DiagnosticDescriptor"/> instance with <see cref="IsNotConfigurable"/> set to <see langword="true"/>.</returns>
        public static DiagnosticDescriptor Create(
            string id,
            string title,
            string messageFormat,
            string category,
            string? description = null,
            string? helpLinkUri = null)
        {
            return new DiagnosticDescriptor(
                id ?? throw new ArgumentNullException(nameof(id)),
                title ?? throw new ArgumentNullException(nameof(title)),
                messageFormat ?? throw new ArgumentNullException(nameof(messageFormat)),
                category ?? throw new ArgumentNullException(nameof(category)),
                DiagnosticSeverity.Error,
                isEnabledByDefault: true,
                isNotConfigurable: true,
                description ?? string.Empty,
                helpLinkUri ?? string.Empty);
        }

        /// <summary>
        /// Creates a new <see cref="DiagnosticDescriptor"/> with a configurable severity.
        /// </summary>
        /// <param name="id">A unique identifier for the diagnostic.</param>
        /// <param name="title">A short title describing the diagnostic.</param>
        /// <param name="messageFormat">A format message string, which may contain placeholders.</param>
        /// <param name="category">The category of the diagnostic (e.g., "Syntax", "Semantic").</param>
        /// <param name="defaultSeverity">The default severity of the diagnostic.</param>
        /// <param name="isEnabledByDefault">Whether the diagnostic is enabled by default.</param>
        /// <param name="description">An optional longer description of the diagnostic.</param>
        /// <param name="helpLinkUri">An optional hyperlink that provides more detailed information.</param>
        /// <returns>A new <see cref="DiagnosticDescriptor"/> instance.</returns>
        public static DiagnosticDescriptor Create(
            string id,
            string title,
            string messageFormat,
            string category,
            DiagnosticSeverity defaultSeverity,
            bool isEnabledByDefault = true,
            string? description = null,
            string? helpLinkUri = null)
        {
            return new DiagnosticDescriptor(
                id ?? throw new ArgumentNullException(nameof(id)),
                title ?? throw new ArgumentNullException(nameof(title)),
                messageFormat ?? throw new ArgumentNullException(nameof(messageFormat)),
                category ?? throw new ArgumentNullException(nameof(category)),
                defaultSeverity,
                isEnabledByDefault,
                isNotConfigurable: false,
                description ?? string.Empty,
                helpLinkUri ?? string.Empty);
        }

        /// <inheritdoc/>
        public bool Equals(DiagnosticDescriptor? other)
        {
            return other is not null && Id == other.Id;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is DiagnosticDescriptor descriptor && Equals(descriptor);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
namespace War3Net.CodeAnalysis.Diagnostics
{
    /// <summary>
    /// Represents a diagnostic, such as a compiler error or a warning, along with the location where it occurred.
    /// </summary>
    public sealed class Diagnostic : IEquatable<Diagnostic>
    {
        private readonly object?[] _messageArgs;

        private Diagnostic(
            DiagnosticDescriptor descriptor,
            Location location,
            DiagnosticSeverity severity,
            object?[] messageArgs)
        {
            Descriptor = descriptor;
            Location = location;
            Severity = severity;
            _messageArgs = messageArgs;
        }

        /// <summary>
        /// Gets the diagnostic descriptor.
        /// </summary>
        public DiagnosticDescriptor Descriptor { get; }

        /// <summary>
        /// Gets the diagnostic identifier.
        /// </summary>
        public string Id => Descriptor.Id;

        /// <summary>
        /// Gets the category of the diagnostic.
        /// </summary>
        public string Category => Descriptor.Category;

        /// <summary>
        /// Gets the location of the diagnostic.
        /// </summary>
        public Location Location { get; }

        /// <summary>
        /// Gets the effective severity of the diagnostic.
        /// </summary>
        public DiagnosticSeverity Severity { get; }

        /// <summary>
        /// Creates a new <see cref="Diagnostic"/>.
        /// </summary>
        /// <param name="descriptor">The diagnostic descriptor.</param>
        /// <param name="location">The location of the diagnostic.</param>
        /// <param name="messageArgs">Optional message format arguments.</param>
        /// <returns>A new <see cref="Diagnostic"/>.</returns>
        public static Diagnostic Create(
            DiagnosticDescriptor descriptor,
            Location location,
            params object?[] messageArgs)
        {
            if (descriptor is null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            if (location is null)
            {
                throw new ArgumentNullException(nameof(location));
            }

            return new Diagnostic(
                descriptor,
                location,
                descriptor.DefaultSeverity,
                messageArgs ?? Array.Empty<object>());
        }

        /// <summary>
        /// Creates a new <see cref="Diagnostic"/> with a specific severity.
        /// </summary>
        /// <param name="descriptor">The diagnostic descriptor.</param>
        /// <param name="location">The location of the diagnostic.</param>
        /// <param name="effectiveSeverity">The effective severity to use.</param>
        /// <param name="messageArgs">Optional message format arguments.</param>
        /// <returns>A new <see cref="Diagnostic"/>.</returns>
        public static Diagnostic Create(
            DiagnosticDescriptor descriptor,
            Location location,
            DiagnosticSeverity effectiveSeverity,
            params object?[] messageArgs)
        {
            if (descriptor is null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            if (location is null)
            {
                throw new ArgumentNullException(nameof(location));
            }

            return new Diagnostic(
                descriptor,
                location,
                effectiveSeverity,
                messageArgs ?? Array.Empty<object>());
        }

        /// <summary>
        /// Gets the formatted message for the diagnostic.
        /// </summary>
        /// <returns>The formatted message.</returns>
        public string GetMessage()
        {
            return _messageArgs.Length == 0
                ? Descriptor.MessageFormat
                : string.Format(CultureInfo.InvariantCulture, Descriptor.MessageFormat, _messageArgs);
        }

        /// <inheritdoc/>
        public bool Equals(Diagnostic? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Id == other.Id
                && Location.Equals(other.Location)
                && Severity == other.Severity;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return obj is Diagnostic diagnostic && Equals(diagnostic);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(
                Id.GetHashCode(StringComparison.Ordinal),
                Location.GetHashCode(),
                Severity);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var severityText = Severity switch
            {
                DiagnosticSeverity.Error => "error",
                DiagnosticSeverity.Warning => "warning",
                DiagnosticSeverity.Info => "info",
                _ => "hidden",
            };

            return $"{Location}: {severityText} {Id}: {GetMessage()}";
        }
    }
}
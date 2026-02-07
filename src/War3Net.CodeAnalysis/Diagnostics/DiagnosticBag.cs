// ------------------------------------------------------------------------------
// <copyright file="DiagnosticBag.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace War3Net.CodeAnalysis.Diagnostics
{
    /// <summary>
    /// A mutable bag of diagnostics. Used internally during parsing/analysis.
    /// </summary>
    public sealed class DiagnosticBag : IEnumerable<Diagnostic>
    {
        private List<Diagnostic>? _diagnostics;

        /// <summary>
        /// Gets the number of diagnostics in the bag.
        /// </summary>
        public int Count => _diagnostics?.Count ?? 0;

        /// <summary>
        /// Gets a value indicating whether there are any errors.
        /// </summary>
        public bool HasErrors
        {
            get
            {
                if (_diagnostics is null)
                {
                    return false;
                }

                foreach (var diagnostic in _diagnostics)
                {
                    if (diagnostic.Severity == DiagnosticSeverity.Error)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Adds a diagnostic to the bag.
        /// </summary>
        /// <param name="diagnostic">The diagnostic to add.</param>
        public void Add(Diagnostic diagnostic)
        {
            if (diagnostic is null)
            {
                throw new ArgumentNullException(nameof(diagnostic));
            }

            _diagnostics ??= new List<Diagnostic>();
            _diagnostics.Add(diagnostic);
        }

        /// <summary>
        /// Adds a range of diagnostics to the bag.
        /// </summary>
        /// <param name="diagnostics">The diagnostics to add.</param>
        public void AddRange(IEnumerable<Diagnostic> diagnostics)
        {
            if (diagnostics is null)
            {
                throw new ArgumentNullException(nameof(diagnostics));
            }

            foreach (var diagnostic in diagnostics)
            {
                Add(diagnostic);
            }
        }

        /// <summary>
        /// Reports a diagnostic using the specified descriptor.
        /// </summary>
        /// <param name="descriptor">The diagnostic descriptor.</param>
        /// <param name="location">The location of the diagnostic.</param>
        /// <param name="messageArgs">Optional message format arguments.</param>
        public void Report(DiagnosticDescriptor descriptor, Location location, params object?[] messageArgs)
        {
            Add(Diagnostic.Create(descriptor, location, messageArgs));
        }

        /// <summary>
        /// Converts the bag to an immutable array.
        /// </summary>
        /// <returns>An immutable array of diagnostics.</returns>
        public ImmutableArray<Diagnostic> ToImmutableArray()
        {
            return _diagnostics is null
                ? ImmutableArray<Diagnostic>.Empty
                : _diagnostics.ToImmutableArray();
        }

        /// <inheritdoc/>
        public IEnumerator<Diagnostic> GetEnumerator()
        {
            return (_diagnostics ?? (IEnumerable<Diagnostic>)ImmutableArray<Diagnostic>.Empty).GetEnumerator();
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="ISemanticModel.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Immutable;

using War3Net.CodeAnalysis.Diagnostics;

namespace War3Net.CodeAnalysis
{
    /// <summary>
    /// Provides semantic information about a compilation unit.
    /// </summary>
    public interface ISemanticModel
    {
        /// <summary>
        /// Gets the diagnostics produced during semantic analysis.
        /// </summary>
        /// <returns>An immutable array of diagnostics.</returns>
        ImmutableArray<Diagnostic> GetDiagnostics();
    }
}
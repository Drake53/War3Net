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
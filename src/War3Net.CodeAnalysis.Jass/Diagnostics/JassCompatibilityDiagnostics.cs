namespace War3Net.CodeAnalysis.Jass.Diagnostics
{
    /// <summary>
    /// Contains diagnostic descriptors for JASS compatibility checks.
    /// </summary>
    public static class JassCompatibilityDiagnostics
    {
        private const string CompatibilityCategory = "Compatibility";
        private const string HelpLinkBase = "https://github.com/Drake53/War3Net/tree/master/docs/jass-diagnostics/";

        /// <summary>
        /// <c>JCA1507</c>: <c>ExecuteFunc</c> is called with a string that does not match any declared function name.
        /// </summary>
        public static readonly DiagnosticDescriptor ExecuteFuncOnNonexistent = DiagnosticDescriptor.Create(
            id: "JCA1507",
            title: "ExecuteFunc on nonexistent function",
            messageFormat: "No function named '{0}' found",
            category: CompatibilityCategory,
            defaultSeverity: DiagnosticSeverity.Warning,
            description: "ExecuteFunc is called with a string that does not match any declared function name.",
            helpLinkUri: HelpLinkBase + "JCA1507.md");

        /// <summary>
        /// <c>JCA2234</c>: An audio file path uses a format that is not supported by Warcraft III.
        /// </summary>
        public static readonly DiagnosticDescriptor UnsupportedAudioFormat = DiagnosticDescriptor.Create(
            id: "JCA2234",
            title: "Unsupported audio format",
            messageFormat: "Audio file argument may use unsupported format '{0}'",
            category: CompatibilityCategory,
            defaultSeverity: DiagnosticSeverity.Warning,
            description: "An audio file path uses a format that is not supported by Warcraft III.",
            helpLinkUri: HelpLinkBase + "JCA2234.md");
    }
}
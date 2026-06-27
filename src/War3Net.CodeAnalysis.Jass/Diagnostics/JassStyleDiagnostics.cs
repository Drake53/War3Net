namespace War3Net.CodeAnalysis.Jass.Diagnostics
{
    /// <summary>
    /// Contains diagnostic descriptors for JASS style warnings.
    /// </summary>
    public static class JassStyleDiagnostics
    {
        private const string StyleCategory = "Style";
        private const string HelpLinkBase = "https://github.com/Drake53/War3Net/tree/master/docs/jass-diagnostics/";

        /// <summary>
        /// <c>JCA1027</c>: Indentation does not match the expected indentation style.
        /// </summary>
        public static readonly DiagnosticDescriptor InconsistentIndentation = DiagnosticDescriptor.Create(
            id: "JCA1027",
            title: "Inconsistent indentation",
            messageFormat: "Inconsistent indentation; expected '{0}' but found '{1}'",
            category: StyleCategory,
            defaultSeverity: DiagnosticSeverity.Warning,
            description: "Indentation does not match the expected indentation style.",
            helpLinkUri: HelpLinkBase + "JCA1027.md");

        /// <summary>
        /// <c>JCA1028</c>: Line has trailing whitespace.
        /// </summary>
        public static readonly DiagnosticDescriptor TrailingWhitespace = DiagnosticDescriptor.Create(
            id: "JCA1028",
            title: "Trailing whitespace",
            messageFormat: "Trailing whitespace",
            category: StyleCategory,
            defaultSeverity: DiagnosticSeverity.Warning,
            description: "Line has trailing whitespace.",
            helpLinkUri: HelpLinkBase + "JCA1028.md");
    }
}
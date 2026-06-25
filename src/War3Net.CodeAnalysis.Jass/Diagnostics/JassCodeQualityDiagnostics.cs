namespace War3Net.CodeAnalysis.Jass.Diagnostics
{
    /// <summary>
    /// Contains diagnostic descriptors for JASS code quality warnings.
    /// </summary>
    public static class JassCodeQualityDiagnostics
    {
        private const string CodeQualityCategory = "CodeQuality";
        private const string HelpLinkBase = "https://github.com/Drake53/War3Net/tree/master/docs/jass-diagnostics/";

        /// <summary>
        /// <c>JCA0020</c>: A constant expression evaluating to zero is used as a divisor.
        /// </summary>
        public static readonly DiagnosticDescriptor DivisionByZero = DiagnosticDescriptor.Create(
            id: "JCA0020",
            title: "Division by constant zero",
            messageFormat: "Division by constant zero",
            category: CodeQualityCategory,
            defaultSeverity: DiagnosticSeverity.Warning,
            description: "A constant expression evaluating to zero is used as a divisor.",
            helpLinkUri: HelpLinkBase + "JCA0020.md");

        /// <summary>
        /// <c>JCA0162</c>: Code is unreachable due to unconditional exit, exhaustive branching, or constant conditions.
        /// </summary>
        public static readonly DiagnosticDescriptor UnreachableCode = DiagnosticDescriptor.Create(
            id: "JCA0162",
            title: "Unreachable code detected",
            messageFormat: "Unreachable code detected",
            category: CodeQualityCategory,
            defaultSeverity: DiagnosticSeverity.Warning,
            description: "Code is unreachable due to unconditional exit, exhaustive branching, or constant conditions.",
            helpLinkUri: HelpLinkBase + "JCA0162.md");

        /// <summary>
        /// <c>JCA2000</c>: A handle was created but never destroyed or removed within the function.
        /// </summary>
        public static readonly DiagnosticDescriptor HandleLeak = DiagnosticDescriptor.Create(
            id: "JCA2000",
            title: "Handle leak",
            messageFormat: "{0} may leak; call {1} when done",
            category: CodeQualityCategory,
            defaultSeverity: DiagnosticSeverity.Warning,
            description: "A handle was created but never destroyed or removed within the function.",
            helpLinkUri: HelpLinkBase + "JCA2000.md");
    }
}
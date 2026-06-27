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
        /// <c>JCA1000</c>: A constant function returns <c>nothing</c>.
        /// </summary>
        public static readonly DiagnosticDescriptor ConstantFunctionReturnsNothing = DiagnosticDescriptor.Create(
            id: "JCA1000",
            title: "Constant function returns nothing",
            messageFormat: "Constant function should not return nothing",
            category: CodeQualityCategory,
            defaultSeverity: DiagnosticSeverity.Warning,
            description: "A constant function returns 'nothing', so it produces no value and can have no useful effect.",
            helpLinkUri: HelpLinkBase + "JCA1000.md");

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

        /// <summary>
        /// <c>JCA2190</c>: A loop has no <c>exitwhen</c> or <c>return</c> statement.
        /// </summary>
        public static readonly DiagnosticDescriptor InfiniteLoop = DiagnosticDescriptor.Create(
            id: "JCA2190",
            title: "Infinite loop",
            messageFormat: "Loop may be infinite; no exit condition found",
            category: CodeQualityCategory,
            defaultSeverity: DiagnosticSeverity.Warning,
            description: "A loop has no 'exitwhen' or 'return' statement.",
            helpLinkUri: HelpLinkBase + "JCA2190.md");
    }
}
namespace War3Net.CodeAnalysis.Jass.Diagnostics
{
    /// <summary>
    /// Contains predefined diagnostic descriptors for JASS parsing and analysis.
    /// </summary>
    public static class JassSyntaxDiagnostics
    {
        private const string SyntaxCategory = "Syntax";
        private const string HelpLinkBase = "https://github.com/Drake53/War3Net/tree/master/docs/jass-diagnostics/";

        /// <summary>
        /// <c>JASS0201</c>: Statement expected but not found.
        /// </summary>
        public static readonly DiagnosticDescriptor MissingStatement = DiagnosticDescriptor.Create(
            id: "JASS0201",
            title: "Missing statement",
            messageFormat: "Statement expected",
            category: SyntaxCategory,
            description: "Statement expected but not found.",
            helpLinkUri: HelpLinkBase + "JASS0201.md");

        /// <summary>
        /// <c>JASS0623</c>: Array declarations cannot have an initializer.
        /// </summary>
        public static readonly DiagnosticDescriptor ArrayInitializerNotAllowed = DiagnosticDescriptor.Create(
            id: "JASS0623",
            title: "Array initializer not allowed",
            messageFormat: "Array declarations cannot have an initializer",
            category: SyntaxCategory,
            description: "Array declarations cannot have an initializer.",
            helpLinkUri: HelpLinkBase + "JASS0623.md");

        /// <summary>
        /// <c>JASS1001</c>: An identifier was expected but not found.
        /// </summary>
        public static readonly DiagnosticDescriptor IdentifierExpected = DiagnosticDescriptor.Create(
            id: "JASS1001",
            title: "Identifier expected",
            messageFormat: "Identifier expected",
            category: SyntaxCategory,
            description: "An identifier was expected but not found.",
            helpLinkUri: HelpLinkBase + "JASS1001.md");

        /// <summary>
        /// <c>JASS1003</c>: A specific symbol or keyword was expected but not found.
        /// </summary>
        public static readonly DiagnosticDescriptor SyntaxError = DiagnosticDescriptor.Create(
            id: "JASS1003",
            title: "Syntax error",
            messageFormat: "Syntax error, {0} expected",
            category: SyntaxCategory,
            description: "A specific symbol or keyword was expected but not found.",
            helpLinkUri: HelpLinkBase + "JASS1003.md");

        /// <summary>
        /// <c>JASS1009</c>: Unrecognized escape sequence in string, character, or FourCC literal.
        /// </summary>
        public static readonly DiagnosticDescriptor InvalidEscapeSequence = DiagnosticDescriptor.Create(
            id: "JASS1009",
            title: "Invalid escape sequence",
            messageFormat: "Invalid escape sequence '{0}'",
            category: SyntaxCategory,
            description: "Unrecognized escape sequence in string, character, or FourCC literal.",
            helpLinkUri: HelpLinkBase + "JASS1009.md");

        /// <summary>
        /// <c>JASS1010</c>: Single-quoted literal is missing closing quote.
        /// </summary>
        public static readonly DiagnosticDescriptor UnterminatedSingleQuotedLiteral = DiagnosticDescriptor.Create(
            id: "JASS1010",
            title: "Unterminated single-quoted literal",
            messageFormat: "Unterminated single-quoted literal",
            category: SyntaxCategory,
            description: "Single-quoted literal is missing closing quote.",
            helpLinkUri: HelpLinkBase + "JASS1010.md");

        /// <summary>
        /// <c>JASS1011</c>: Single-quoted literal contains no characters.
        /// </summary>
        public static readonly DiagnosticDescriptor EmptySingleQuotedLiteral = DiagnosticDescriptor.Create(
            id: "JASS1011",
            title: "Empty single-quoted literal",
            messageFormat: "Empty single-quoted literal",
            category: SyntaxCategory,
            description: "Single-quoted literal contains no characters. Must contain exactly 1 (character) or 4 (FourCC) characters.",
            helpLinkUri: HelpLinkBase + "JASS1011.md");

        /// <summary>
        /// <c>JASS1012</c>: Single-quoted literal has an invalid number of characters.
        /// </summary>
        public static readonly DiagnosticDescriptor InvalidSingleQuotedStringLength = DiagnosticDescriptor.Create(
            id: "JASS1012",
            title: "Invalid single-quoted literal length",
            messageFormat: "Single-quoted literal '{0}' must contain exactly 1 or 4 characters",
            category: SyntaxCategory,
            description: "Single-quoted literal has an invalid number of characters. Must be exactly 1 (character) or 4 (FourCC).",
            helpLinkUri: HelpLinkBase + "JASS1012.md");

        /// <summary>
        /// <c>JASS1013</c>: A numeric literal is malformed or contains invalid digits.
        /// </summary>
        public static readonly DiagnosticDescriptor InvalidNumber = DiagnosticDescriptor.Create(
            id: "JASS1013",
            title: "Invalid number",
            messageFormat: "Invalid number '{0}'",
            category: SyntaxCategory,
            description: "A numeric literal is malformed or contains invalid digits.",
            helpLinkUri: HelpLinkBase + "JASS1013.md");

        /// <summary>
        /// <c>JASS1022</c>: Declaration expected but not found.
        /// </summary>
        public static readonly DiagnosticDescriptor MissingDeclaration = DiagnosticDescriptor.Create(
            id: "JASS1022",
            title: "Missing declaration",
            messageFormat: "Declaration expected",
            category: SyntaxCategory,
            description: "Declaration expected but not found.",
            helpLinkUri: HelpLinkBase + "JASS1022.md");

        /// <summary>
        /// <c>JASS1025</c>: Single-line comment or end-of-line expected after a construct.
        /// </summary>
        public static readonly DiagnosticDescriptor EndOfLineExpected = DiagnosticDescriptor.Create(
            id: "JASS1025",
            title: "End-of-line expected",
            messageFormat: "Single-line comment or end-of-line expected",
            category: SyntaxCategory,
            description: "Single-line comment or end-of-line expected after a construct.",
            helpLinkUri: HelpLinkBase + "JASS1025.md");

        /// <summary>
        /// <c>JASS1039</c>: String literal is missing closing quote.
        /// </summary>
        public static readonly DiagnosticDescriptor UnterminatedString = DiagnosticDescriptor.Create(
            id: "JASS1039",
            title: "Unterminated string",
            messageFormat: "Unterminated string literal",
            category: SyntaxCategory,
            description: "String literal is missing closing quote.",
            helpLinkUri: HelpLinkBase + "JASS1039.md");

        /// <summary>
        /// <c>JASS1040</c>: Construct must appear as the first token on a line.
        /// </summary>
        public static readonly DiagnosticDescriptor ConstructMustAppearOnOwnLine = DiagnosticDescriptor.Create(
            id: "JASS1040",
            title: "Construct must appear on its own line",
            messageFormat: "'{0}' must appear as the first token on a line",
            category: SyntaxCategory,
            description: "Construct must appear as the first token on a line.",
            helpLinkUri: HelpLinkBase + "JASS1040.md");

        /// <summary>
        /// <c>JASS1041</c>: An identifier was expected but a keyword was found.
        /// </summary>
        public static readonly DiagnosticDescriptor IdentifierExpectedKeyword = DiagnosticDescriptor.Create(
            id: "JASS1041",
            title: "Identifier expected; keyword found",
            messageFormat: "Identifier expected; '{0}' is a keyword",
            category: SyntaxCategory,
            description: "An identifier was expected but a keyword was found instead.",
            helpLinkUri: HelpLinkBase + "JASS1041.md");

        /// <summary>
        /// <c>JASS1056</c>: An invalid character was encountered.
        /// </summary>
        public static readonly DiagnosticDescriptor InvalidCharacter = DiagnosticDescriptor.Create(
            id: "JASS1056",
            title: "Invalid character",
            messageFormat: "Invalid character '{0}'",
            category: SyntaxCategory,
            description: "An invalid character was encountered that is not valid in JASS.",
            helpLinkUri: HelpLinkBase + "JASS1056.md");

        /// <summary>
        /// <c>JASS1073</c>: Unexpected token encountered.
        /// </summary>
        public static readonly DiagnosticDescriptor UnexpectedToken = DiagnosticDescriptor.Create(
            id: "JASS1073",
            title: "Unexpected token",
            messageFormat: "Unexpected token '{0}'",
            category: SyntaxCategory,
            description: "An unexpected token was encountered that does not belong in this context.",
            helpLinkUri: HelpLinkBase + "JASS1073.md");

        /// <summary>
        /// <c>JASS1513</c>: A block is missing its closing keyword (<c>endfunction</c>, <c>endglobals</c>, <c>endif</c>, or <c>endloop</c>).
        /// </summary>
        public static readonly DiagnosticDescriptor MissingClosingKeyword = DiagnosticDescriptor.Create(
            id: "JASS1513",
            title: "Missing closing keyword",
            messageFormat: "'{0}' expected",
            category: SyntaxCategory,
            description: "A block is missing its closing keyword (endfunction, endglobals, endif, or endloop).",
            helpLinkUri: HelpLinkBase + "JASS1513.md");

        /// <summary>
        /// <c>JASS1514</c>: If or elseif clause is missing <c>then</c> keyword.
        /// </summary>
        public static readonly DiagnosticDescriptor MissingThen = DiagnosticDescriptor.Create(
            id: "JASS1514",
            title: "Missing then",
            messageFormat: "Missing 'then' after condition",
            category: SyntaxCategory,
            description: "If or elseif clause is missing 'then' keyword.",
            helpLinkUri: HelpLinkBase + "JASS1514.md");

        /// <summary>
        /// <c>JASS1519</c>: Statement appears in invalid location.
        /// </summary>
        public static readonly DiagnosticDescriptor InvalidStatementLocation = DiagnosticDescriptor.Create(
            id: "JASS1519",
            title: "Invalid statement location",
            messageFormat: "Statement not allowed at this location",
            category: SyntaxCategory,
            description: "Statement appears in invalid location.",
            helpLinkUri: HelpLinkBase + "JASS1519.md");

        /// <summary>
        /// <c>JASS1525</c>: Expression expected but not found.
        /// </summary>
        public static readonly DiagnosticDescriptor MissingExpression = DiagnosticDescriptor.Create(
            id: "JASS1525",
            title: "Missing expression",
            messageFormat: "Invalid expression term '{0}'",
            category: SyntaxCategory,
            description: "An expression was expected but an invalid term was found.",
            helpLinkUri: HelpLinkBase + "JASS1525.md");

        /// <summary>
        /// <c>JASS1733</c>: Expression expected at end of input.
        /// </summary>
        public static readonly DiagnosticDescriptor MissingExpressionAtEnd = DiagnosticDescriptor.Create(
            id: "JASS1733",
            title: "Expression expected",
            messageFormat: "Expression expected",
            category: SyntaxCategory,
            description: "An expression was expected but the end of input was reached.",
            helpLinkUri: HelpLinkBase + "JASS1733.md");

        /// <summary>
        /// <c>JASS8641</c>: Else or elseif clause without matching if.
        /// </summary>
        public static readonly DiagnosticDescriptor ElseWithoutIf = DiagnosticDescriptor.Create(
            id: "JASS8641",
            title: "Else without if",
            messageFormat: "'{0}' without matching 'if'",
            category: SyntaxCategory,
            description: "Else or elseif clause without matching if.",
            helpLinkUri: HelpLinkBase + "JASS8641.md");
    }
}
namespace War3Net.CodeAnalysis.Jass.Diagnostics
{
    /// <summary>
    /// Contains diagnostic descriptors for JASS semantic analysis.
    /// </summary>
    public static class JassSemanticDiagnostics
    {
        private const string SemanticCategory = "Semantic";
        private const string HelpLinkBase = "https://github.com/Drake53/War3Net/tree/master/docs/jass-diagnostics/";

        /// <summary>
        /// <c>JASS0019</c>: The binary operator is not valid for the given operand types.
        /// </summary>
        public static readonly DiagnosticDescriptor IncompatibleOperandTypes = DiagnosticDescriptor.Create(
            id: "JASS0019",
            title: "Incompatible operand types",
            messageFormat: "Operator '{0}' cannot be applied to operands of type '{1}' and '{2}'",
            category: SemanticCategory,
            description: "The binary operator is not valid for the given operand types.",
            helpLinkUri: HelpLinkBase + "JASS0019.md");

        /// <summary>
        /// <c>JASS0021</c>: Array subscript was used on a variable that is not an array.
        /// </summary>
        public static readonly DiagnosticDescriptor ArrayAccessOnNonArray = DiagnosticDescriptor.Create(
            id: "JASS0021",
            title: "Array access on non-array",
            messageFormat: "'{0}' is not an array",
            category: SemanticCategory,
            description: "Array subscript was used on a variable that is not an array.",
            helpLinkUri: HelpLinkBase + "JASS0021.md");

        /// <summary>
        /// <c>JASS0022</c>: An array variable was used without a subscript.
        /// </summary>
        public static readonly DiagnosticDescriptor MissingArraySubscript = DiagnosticDescriptor.Create(
            id: "JASS0022",
            title: "Missing array subscript",
            messageFormat: "Array '{0}' must be accessed with subscript",
            category: SemanticCategory,
            description: "An array variable was used without a subscript.",
            helpLinkUri: HelpLinkBase + "JASS0022.md");

        /// <summary>
        /// <c>JASS0023</c>: The unary operator is not valid for the given operand type.
        /// </summary>
        public static readonly DiagnosticDescriptor InvalidUnaryOperand = DiagnosticDescriptor.Create(
            id: "JASS0023",
            title: "Invalid unary operand",
            messageFormat: "Operator '{0}' cannot be applied to operand of type '{1}'",
            category: SemanticCategory,
            description: "The unary operator is not valid for the given operand type.",
            helpLinkUri: HelpLinkBase + "JASS0023.md");

        /// <summary>
        /// <c>JASS0028</c>: An entry point function has the wrong signature.
        /// </summary>
        /// <remarks>
        /// Only reported for <c>war3map.j</c>.
        /// </remarks>
        public static readonly DiagnosticDescriptor EntryPointWrongSignature = DiagnosticDescriptor.Create(
            id: "JASS0028",
            title: "Entry point has wrong signature",
            messageFormat: "Entry point function '{0}' must take nothing and return nothing",
            category: SemanticCategory,
            description: "The map script entry point functions 'main' and 'config' must take nothing and return nothing.",
            helpLinkUri: HelpLinkBase + "JASS0028.md");

        /// <summary>
        /// <c>JASS0029</c>: The expression type does not match the expected type.
        /// </summary>
        public static readonly DiagnosticDescriptor TypeMismatch = DiagnosticDescriptor.Create(
            id: "JASS0029",
            title: "Type mismatch",
            messageFormat: "Cannot implicitly convert type '{0}' to '{1}'",
            category: SemanticCategory,
            description: "The expression type does not match the expected type.",
            helpLinkUri: HelpLinkBase + "JASS0029.md");

        /// <summary>
        /// <c>JASS0100</c>: A function has multiple parameters with the same name.
        /// </summary>
        public static readonly DiagnosticDescriptor DuplicateParameterName = DiagnosticDescriptor.Create(
            id: "JASS0100",
            title: "Duplicate parameter name",
            messageFormat: "Parameter '{0}' is already defined",
            category: SemanticCategory,
            description: "A function has multiple parameters with the same name.",
            helpLinkUri: HelpLinkBase + "JASS0100.md");

        /// <summary>
        /// <c>JASS0101</c>: A symbol with the same name has already been declared.
        /// </summary>
        public static readonly DiagnosticDescriptor DuplicateDeclaration = DiagnosticDescriptor.Create(
            id: "JASS0101",
            title: "Duplicate declaration",
            messageFormat: "'{0}' is already declared",
            category: SemanticCategory,
            description: "A symbol with the same name has already been declared.",
            helpLinkUri: HelpLinkBase + "JASS0101.md");

        /// <summary>
        /// <c>JASS0103</c>: A name was referenced that has not been declared.
        /// </summary>
        public static readonly DiagnosticDescriptor UndefinedName = DiagnosticDescriptor.Create(
            id: "JASS0103",
            title: "Undefined name",
            messageFormat: "The name '{0}' does not exist in the current context",
            category: SemanticCategory,
            description: "A name was referenced that has not been declared.",
            helpLinkUri: HelpLinkBase + "JASS0103.md");

        /// <summary>
        /// <c>JASS0118</c>: A symbol is used as a different kind than what it was declared as.
        /// </summary>
        public static readonly DiagnosticDescriptor WrongSymbolKind = DiagnosticDescriptor.Create(
            id: "JASS0118",
            title: "Wrong symbol kind",
            messageFormat: "'{0}' is a {1} but is used like a {2}",
            category: SemanticCategory,
            description: "A symbol is used as a different kind than what it was declared as.",
            helpLinkUri: HelpLinkBase + "JASS0118.md");

        /// <summary>
        /// <c>JASS0127</c>: A return statement with a value was found in a function that returns nothing.
        /// </summary>
        public static readonly DiagnosticDescriptor UnexpectedReturn = DiagnosticDescriptor.Create(
            id: "JASS0127",
            title: "Unexpected return",
            messageFormat: "Cannot return a value from function returning 'nothing'",
            category: SemanticCategory,
            description: "A return statement with a value was found in a function that returns nothing.",
            helpLinkUri: HelpLinkBase + "JASS0127.md");

        /// <summary>
        /// <c>JASS0128</c>: A local variable with the same name has already been declared in this function.
        /// </summary>
        public static readonly DiagnosticDescriptor DuplicateLocalDeclaration = DiagnosticDescriptor.Create(
            id: "JASS0128",
            title: "Duplicate local declaration",
            messageFormat: "Local variable '{0}' is already declared in this function",
            category: SemanticCategory,
            description: "A local variable with the same name has already been declared in this function.",
            helpLinkUri: HelpLinkBase + "JASS0128.md");

        /// <summary>
        /// <c>JASS0131</c>: Constants cannot be reassigned after initialization.
        /// </summary>
        public static readonly DiagnosticDescriptor CannotAssignToConstant = DiagnosticDescriptor.Create(
            id: "JASS0131",
            title: "Assignment to constant",
            messageFormat: "Cannot assign to constant '{0}'",
            category: SemanticCategory,
            description: "Constants cannot be reassigned after initialization.",
            helpLinkUri: HelpLinkBase + "JASS0131.md");

        /// <summary>
        /// <c>JASS0133</c>: A constant variable must be initialized with a compile-time constant expression.
        /// </summary>
        public static readonly DiagnosticDescriptor ConstantInitializerNotConstant = DiagnosticDescriptor.Create(
            id: "JASS0133",
            title: "Constant initializer not constant",
            messageFormat: "Constant initializer must be a constant expression",
            category: SemanticCategory,
            description: "A constant variable must be initialized with a compile-time constant expression.",
            helpLinkUri: HelpLinkBase + "JASS0133.md");

        /// <summary>
        /// <c>JASS0139</c>: The exitwhen statement can only be used inside a loop.
        /// </summary>
        public static readonly DiagnosticDescriptor ExitWhenOutsideLoop = DiagnosticDescriptor.Create(
            id: "JASS0139",
            title: "ExitWhen outside loop",
            messageFormat: "'exitwhen' must be inside a 'loop' statement",
            category: SemanticCategory,
            description: "The exitwhen statement can only be used inside a loop.",
            helpLinkUri: HelpLinkBase + "JASS0139.md");

        /// <summary>
        /// <c>JASS0146</c>: A circular type extension chain was detected.
        /// </summary>
        public static readonly DiagnosticDescriptor CircularTypeExtension = DiagnosticDescriptor.Create(
            id: "JASS0146",
            title: "Circular type extension",
            messageFormat: "Circular base type dependency involving '{0}' and '{1}'",
            category: SemanticCategory,
            description: "A circular type extension chain was detected.",
            helpLinkUri: HelpLinkBase + "JASS0146.md");

        /// <summary>
        /// <c>JASS0161</c>: A function that returns a value must have a return statement on all code paths.
        /// </summary>
        public static readonly DiagnosticDescriptor MissingReturn = DiagnosticDescriptor.Create(
            id: "JASS0161",
            title: "Missing return",
            messageFormat: "Not all code paths return a value in function '{0}'",
            category: SemanticCategory,
            description: "A function that returns a value must have a return statement on all code paths.",
            helpLinkUri: HelpLinkBase + "JASS0161.md");

        /// <summary>
        /// <c>JASS0163</c>: Code after a root-level return statement is unreachable.
        /// </summary>
        public static readonly DiagnosticDescriptor UnreachableCodeAfterReturn = DiagnosticDescriptor.Create(
            id: "JASS0163",
            title: "Unreachable code after return statement",
            messageFormat: "Unreachable code detected",
            category: SemanticCategory,
            DiagnosticSeverity.Error,
            description: "Code after a root-level return statement is unreachable.",
            helpLinkUri: HelpLinkBase + "JASS0163.md");

        /// <summary>
        /// <c>JASS0246</c>: A type was referenced that has not been declared.
        /// </summary>
        public static readonly DiagnosticDescriptor UndefinedType = DiagnosticDescriptor.Create(
            id: "JASS0246",
            title: "Undefined type",
            messageFormat: "Undefined type '{0}'",
            category: SemanticCategory,
            description: "A type was referenced that has not been declared.",
            helpLinkUri: HelpLinkBase + "JASS0246.md");

        /// <summary>
        /// <c>JASS0509</c>: A type declaration attempted to extend a primitive type that cannot be extended.
        /// </summary>
        public static readonly DiagnosticDescriptor CannotExtendPrimitiveType = DiagnosticDescriptor.Create(
            id: "JASS0509",
            title: "Cannot extend primitive type",
            messageFormat: "'{0}' cannot extend primitive type '{1}'",
            category: SemanticCategory,
            description: "A type declaration attempted to extend a primitive type that cannot be extended.",
            helpLinkUri: HelpLinkBase + "JASS0509.md");

        /// <summary>
        /// <c>JASS0645</c>: An identifier does not conform to JASS naming rules.
        /// </summary>
        public static readonly DiagnosticDescriptor InvalidIdentifier = DiagnosticDescriptor.Create(
            id: "JASS0645",
            title: "Invalid identifier",
            messageFormat: "'{0}' is not a valid identifier",
            category: SemanticCategory,
            description: "An identifier does not conform to JASS naming rules.",
            helpLinkUri: HelpLinkBase + "JASS0645.md");

        /// <summary>
        /// <c>JASS0841</c>: A symbol was referenced before it was declared.
        /// </summary>
        public static readonly DiagnosticDescriptor ForwardReference = DiagnosticDescriptor.Create(
            id: "JASS0841",
            title: "Forward reference",
            messageFormat: "'{0}' must be declared before it is used",
            category: SemanticCategory,
            description: "A symbol was referenced before it was declared.",
            helpLinkUri: HelpLinkBase + "JASS0841.md");

        /// <summary>
        /// <c>JASS1501</c>: The number of arguments does not match the function's parameter count.
        /// </summary>
        public static readonly DiagnosticDescriptor WrongArgumentCount = DiagnosticDescriptor.Create(
            id: "JASS1501",
            title: "Wrong argument count",
            messageFormat: "Function '{0}' expects {1} argument(s), but {2} were provided",
            category: SemanticCategory,
            description: "The number of arguments does not match the function's parameter count.",
            helpLinkUri: HelpLinkBase + "JASS1501.md");

        /// <summary>
        /// <c>JASS1503</c>: A function argument type does not match the expected parameter type.
        /// </summary>
        public static readonly DiagnosticDescriptor ArgumentTypeMismatch = DiagnosticDescriptor.Create(
            id: "JASS1503",
            title: "Argument type mismatch",
            messageFormat: "Argument {0}: cannot convert from '{1}' to '{2}'",
            category: SemanticCategory,
            description: "A function argument type does not match the expected parameter type.",
            helpLinkUri: HelpLinkBase + "JASS1503.md");

        /// <summary>
        /// <c>JASS1547</c>: Keyword <c>nothing</c> cannot be used in this context.
        /// </summary>
        public static readonly DiagnosticDescriptor NothingNotAllowed = DiagnosticDescriptor.Create(
            id: "JASS1547",
            title: "Keyword 'nothing' cannot be used in this context",
            messageFormat: "Keyword 'nothing' cannot be used in this context",
            category: SemanticCategory,
            description: "Keyword 'nothing' cannot be used in this context.",
            helpLinkUri: HelpLinkBase + "JASS1547.md");

        /// <summary>
        /// <c>JASS1558</c>: A symbol named like an entry point exists but is not suitable.
        /// </summary>
        /// <remarks>
        /// Only reported for <c>war3map.j</c>.
        /// </remarks>
        public static readonly DiagnosticDescriptor UnsuitableEntryPoint = DiagnosticDescriptor.Create(
            id: "JASS1558",
            title: "Unsuitable entry point function",
            messageFormat: "'{0}' is not a suitable entry point function",
            category: SemanticCategory,
            description: "The map script entry point functions 'main' and 'config' must be ordinary (non-native, non-constant) functions.",
            helpLinkUri: HelpLinkBase + "JASS1558.md");

        /// <summary>
        /// <c>JASS1958</c>: The code type cannot be used as the element type of an array.
        /// </summary>
        public static readonly DiagnosticDescriptor CodeTypeNotAllowed = DiagnosticDescriptor.Create(
            id: "JASS1958",
            title: "Code type not allowed",
            messageFormat: "Type 'code' cannot be used for arrays",
            category: SemanticCategory,
            description: "The code type cannot be used as the element type of an array.",
            helpLinkUri: HelpLinkBase + "JASS1958.md");

        /// <summary>
        /// <c>JASS2901</c>: A constant function cannot modify global variables.
        /// </summary>
        public static readonly DiagnosticDescriptor ConstantFunctionModifiesGlobal = DiagnosticDescriptor.Create(
            id: "JASS2901",
            title: "Constant function cannot modify global variable",
            messageFormat: "Constant function cannot modify global variable '{0}'",
            category: SemanticCategory,
            description: "A constant function cannot modify global variables.",
            helpLinkUri: HelpLinkBase + "JASS2901.md");

        /// <summary>
        /// <c>JASS2902</c>: A constant function can only call other constant functions.
        /// </summary>
        public static readonly DiagnosticDescriptor ConstantFunctionCallsNonConstant = DiagnosticDescriptor.Create(
            id: "JASS2902",
            title: "Constant function calls non-constant function",
            messageFormat: "Constant function cannot call non-constant function '{0}'",
            category: SemanticCategory,
            description: "A constant function can only call other constant functions.",
            helpLinkUri: HelpLinkBase + "JASS2902.md");

        /// <summary>
        /// <c>JASS5001</c>: The map script is missing a required entry point function.
        /// </summary>
        /// <remarks>
        /// Only reported for <c>war3map.j</c>.
        /// </remarks>
        public static readonly DiagnosticDescriptor MissingEntryPoint = DiagnosticDescriptor.Create(
            id: "JASS5001",
            title: "Missing entry point function",
            messageFormat: "Map script is missing entry point function '{0}'",
            category: SemanticCategory,
            description: "A Warcraft III map script must contain both 'main' and 'config' entry point functions.",
            helpLinkUri: HelpLinkBase + "JASS5001.md");

        /// <summary>
        /// <c>JASS8185</c>: A local variable declaration must appear at the start of the function body.
        /// </summary>
        public static readonly DiagnosticDescriptor LocalDeclarationMustAppearFirst = DiagnosticDescriptor.Create(
            id: "JASS8185",
            title: "Local declaration must appear first",
            messageFormat: "Local variable declaration must appear at the start of the function body",
            category: SemanticCategory,
            description: "JASS requires all local variable declarations to appear at the beginning of a function body, before any other statement type.",
            helpLinkUri: HelpLinkBase + "JASS8185.md");

        /// <summary>
        /// <c>JASS8803</c>: A top-level declaration appears after a declaration kind that must come later.
        /// </summary>
        public static readonly DiagnosticDescriptor DeclarationOrderViolation = DiagnosticDescriptor.Create(
            id: "JASS8803",
            title: "Declaration order violation",
            messageFormat: "A {0} may not follow a {1}",
            category: SemanticCategory,
            description: "Top-level declarations must follow the order: type, globals, native, function.",
            helpLinkUri: HelpLinkBase + "JASS8803.md");
    }
}
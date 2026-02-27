// ------------------------------------------------------------------------------
// <copyright file="JassSemanticDiagnostics.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Diagnostics;

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
        /// <c>JASS1958</c>: The code type cannot be used as the element type of an array.
        /// </summary>
        public static readonly DiagnosticDescriptor CodeTypeNotAllowed = DiagnosticDescriptor.Create(
            id: "JASS1958",
            title: "Code type not allowed",
            messageFormat: "Type 'code' cannot be used for arrays",
            category: SemanticCategory,
            description: "The code type cannot be used as the element type of an array.",
            helpLinkUri: HelpLinkBase + "JASS1958.md");
    }
}
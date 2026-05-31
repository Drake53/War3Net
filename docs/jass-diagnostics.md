# War3Net.CodeAnalysis.Jass Diagnostic Specification

## Overview

This document lists all implemented and planned diagnostics. Diagnostic IDs match their C# equivalent where possible.

### Categories

| Category          | Severity | Description                                                        |
|-------------------|----------|--------------------------------------------------------------------|
| `"Syntax"`        | Error    | Parse and lexical errors                                           |
| `"Semantic"`      | Error    | Type and symbol errors                                             |
| `"CodeQuality"`   | Warning  | Dead code, unused symbols, null safety, complexity, resource leaks |
| `"Style"`         | Warning  | Formatting and naming                                              |
| `"BestPractice"`  | Info     | Idiomatic and efficient code suggestions                           |
| `"Compatibility"` | Info     | Cross-version and desync concerns                                  |

## Error Diagnostics

### Syntax Errors

| ID | Description | Message | C# Equivalent |
|----|-------------|---------|---------------|
| JASS1001 | An identifier was expected but not found | `Identifier expected` | CS1001 `Identifier expected` |
| JASS1003 | A specific symbol or keyword was expected but not found | `Syntax error, {0} expected` | CS1003 `Syntax error, '{0}' expected` |
| JASS1009 | Unrecognized escape sequence in string/single-quoted literal | `Invalid escape sequence '{0}'` | CS1009 `Unrecognized escape sequence` |
| JASS1010 | Single-quoted literal is missing closing quote | `Unterminated single-quoted literal` | CS1010 `Newline in constant` |
| JASS1012 | Single-quoted literal has an invalid number of characters | `Single-quoted literal '{0}' must contain exactly 1 or 4 characters` | CS1012 `Too many characters in character literal` |
| JASS1013 | A numeric literal is malformed or contains invalid digits | `Invalid number '{0}'` | CS1013 `Invalid number` |
| JASS1025 | Single-line comment or end-of-line expected after a construct | `Single-line comment or end-of-line expected` | CS1025 `Single-line comment or end-of-line expected` |
| JASS1039 | String literal is missing closing quote | `Unterminated string literal` | CS1039 `Unterminated string literal` |
| JASS1040 | Construct must appear as the first token on a line | `'{0}' must appear as the first token on a line` | CS1040 `Preprocessor directives must appear as the first non-whitespace character on a line` |
| JASS1041 | An identifier was expected but a keyword was found | `Identifier expected; '{0}' is a keyword` | CS1041 `Identifier expected; '{0}' is a keyword` |
| JASS1056 | An invalid character was encountered | `Invalid character '{0}'` | CS1056 `Unexpected character '{0}'` |
| JASS1073 | Unexpected token encountered | `Unexpected token '{0}'` | CS1073 `Unexpected token '{0}'` |
| JASS1513 | A block is missing its closing keyword | `'{0}' expected` | CS1513 `} expected` |
| JASS1514 | If/elseif clause is missing `then` keyword | `Missing 'then' after condition` | CS1514 `{ expected` |
| JASS1525 | Invalid expression term | `Invalid expression term '{0}'` | CS1525 `Invalid expression term '{0}'` |
| JASS8641 | Else or elseif clause without matching if | `'{0}' without matching 'if'` | CS8641 `'else' cannot start a statement.` |

### Semantic Errors

| ID | Description | Message | C# Equivalent |
|----|-------------|---------|---------------|
| JASS0019 | Binary operator used with incompatible types | `Operator '{0}' cannot be applied to operands of type '{1}' and '{2}'` | CS0019 `Operator '{0}' cannot be applied to operands of type '{1}' and '{2}'` |
| JASS0021 | Attempted to index a non-array variable | `'{0}' is not an array` | CS0021 `Cannot apply indexing with [] to an expression of type '{0}'` |
| JASS0022 | Expected array index | `Array '{0}' must be accessed with subscript` | — |
| JASS0023 | Unary operator used with incompatible type | `Operator '{0}' cannot be applied to operand of type '{1}'` | CS0023 `Operator '{0}' cannot be applied to operand of type '{1}'` |
| JASS0028 | Entry point function `main`/`config` declared with wrong signature | `Entry point function '{0}' must take nothing and return nothing` | CS0028 `'{0}' has the wrong signature to be an entry point` |
| JASS0029 | Expression type doesn't match expected type | `Cannot implicitly convert type '{0}' to '{1}'` | CS0029 `Cannot implicitly convert type '{0}' to '{1}'` |
| JASS0100 | Function has multiple parameters with same name | `Parameter '{0}' is already defined` | CS0100 `The parameter name '{0}' is a duplicate` |
| JASS0101 | Symbol with same name already declared | `'{0}' is already declared` | CS0101 `The namespace '{1}' already contains a definition for '{0}'` |
| JASS0103 | Reference to a name that is not declared | `The name '{0}' does not exist in the current context` | CS0103 `The name '{0}' does not exist in the current context` |
| JASS0118 | Symbol used as wrong kind | `'{0}' is a {1} but is used like a {2}` | CS0118 `'{0}' is a {1} but is used like a {2}` |
| JASS0127 | Return with value in function returning nothing | `Cannot return a value from function returning 'nothing'` | CS0127 `Since '{0}' returns void, a return keyword must not be followed by an object expression` |
| JASS0128 | Local variable with same name already declared | `Local variable '{0}' is already declared in this function` | CS0128 `A local variable or function named '{0}' is already defined in this scope` |
| JASS0131 | Attempting to assign to a constant variable | `Cannot assign to constant '{0}'` | CS0131 `The left-hand side of an assignment must be a variable, property or indexer` |
| JASS0133 | Constant initialized with non-constant expression | `Constant initializer must be a constant expression` | CS0133 `The expression being assigned to '{0}' must be constant` |
| JASS0139 | `exitwhen` statement used outside of a loop | `'exitwhen' must be inside a 'loop' statement` | CS0139 `No enclosing loop out of which to break or continue` |
| JASS0146 | Circular type extension | `Circular base type dependency involving '{0}' and '{1}'` | CS0146 `Circular base type dependency involving '{0}' and '{1}'` |
| JASS0161 | Non-void function path doesn't return a value | `Not all code paths return a value in function '{0}'` | CS0161 `'{0}': not all code paths return a value` |
| JASS0163 | Code after a root-level return statement is unreachable | `Unreachable code detected` | — |
| JASS0246 | Reference to type that is not declared | `Undefined type '{0}'` | CS0246 `The type or namespace name '{0}' could not be found` |
| JASS0509 | Type extends a primitive type that cannot be extended | `'{0}' cannot extend primitive type '{1}'` | CS0509 `'{0}': cannot derive from sealed type '{1}'` |
| JASS0645 | Identifier does not conform to JASS naming rules | `'{0}' is not a valid identifier` | — |
| JASS0841 | Symbol referenced before it is declared | `'{0}' must be declared before it is used` | CS0841 `Cannot use local variable '{0}' before it is declared` |
| JASS1501 | Function called with incorrect number of arguments | `Function '{0}' expects {1} argument(s), but {2} were provided` | CS1501 `No overload for method '{0}' takes {1} arguments` |
| JASS1503 | Function argument type doesn't match parameter type | `Argument {0}: cannot convert from '{1}' to '{2}'` | CS1503 `Argument {0}: cannot convert from '{1}' to '{2}'` |
| JASS1547 | Keyword `nothing` cannot be used in this context | `Keyword 'nothing' cannot be used in this context` | CS1547 `Keyword 'void' cannot be used in this context` |
| JASS1558 | A symbol named `main`/`config` exists but is unsuitable as an entry point | `'{0}' is not a suitable entry point function` | CS1558 `'{0}' does not have a suitable static 'Main' method` |
| JASS1958 | `code` type used in array declaration | `Type 'code' cannot be used for arrays` | — |
| JASS2901 | Constant function cannot modify global variable | `Constant function cannot modify global variable '{0}'` | PUR001 `Method mutates a field` |
| JASS2902 | Constant function calls non-constant function | `Constant function cannot call non-constant function '{0}'` | PUR002 `Method calls a non-pure method` |
| JASS5001 | Map script is missing a required entry point function | `Map script is missing entry point function '{0}'` | CS5001 `Program does not contain a static 'Main' method suitable for an entry point` |
| JASS8185 | Local variable declaration must appear at the start of the function body | `Local variable declaration must appear at the start of the function body` | CS8185 `A declaration is not allowed in this context.` |
| JASS8803 | Top-level declaration order violation | `A {0} may not follow a {1}` | CS8803 `Top-level statements must precede namespace and type declarations.` |

## Warning Diagnostics

### Code Quality Warnings

| ID | Description | Message | C# Equivalent |
|----|-------------|---------|---------------|
| JASS0020 | Constant expression evaluating to zero used as divisor | `Division by constant zero` | CS0020 `Division by constant zero` |
| JASS0162 | Code is unreachable due to unconditional exit, exhaustive branching, or constant conditions | `Unreachable code detected` | CS0162 `Unreachable code detected` |
| JASS2000 | Handle created but never destroyed/removed | `{0} may leak; call {1} when done` | CA2000 `Dispose objects before losing scope` |

### Style Warnings

| ID | Description | Message | C# Equivalent |
|----|-------------|---------|---------------|

## Info Diagnostics

### Best Practice Suggestions

| ID | Description | Message | C# Equivalent |
|----|-------------|---------|---------------|

### Compatibility Suggestions

| ID | Description | Message | C# Equivalent |
|----|-------------|---------|---------------|

## Individual Diagnostic Documentation

For detailed documentation on each diagnostic including examples and fixes, see the [jass-diagnostics/](jass-diagnostics/) directory.

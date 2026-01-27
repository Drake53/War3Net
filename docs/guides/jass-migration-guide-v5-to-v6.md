# War3Net.CodeAnalysis.Jass v5 -> v6 migration guide

## Overview

War3Net.CodeAnalysis.Jass v6 introduced major breaking changes to make the library better suited for certain use cases.

New classes have been added for tokens and trivia, making it possible to model a JASS script with 100% accuracy.

This guide contains mapping tables for migrating your code from v5 to v6.

## Syntax changes

<details><summary>New types</summary>

New syntax node classes to hold tokens:

| Class name                            | Token(s)              |
|---------------------------------------|-----------------------|
| `JassElementAccessClauseSyntax`       | `[` and `]`           |
| `JassElseIfClauseDeclaratorSyntax`    | `elseif` and `then`   |
| `JassEmptyParameterListSyntax`        | `takes` and `nothing` |
| `JassGlobalConstantDeclarationSyntax` | `constant`            |
| `JassIfClauseDeclaratorSyntax`        | `if` and `then`       |
| `JassReturnClauseSyntax`              | `returns`             |

Other new syntax node classes:

| Class name                                    | Purpose                                                                                    |
|-----------------------------------------------|--------------------------------------------------------------------------------------------|
| `JassIfClauseSyntax`                          | Holds `JassIfClauseDeclaratorSyntax` and statements                                        |
| `JassParameterListOrEmptyParameterListSyntax` | Abstract base class for empty (`takes nothing`) and not-empty (`takes <parameters>`) lists |
| `JassSyntaxNode`                              | Abstract base class for all new and existing syntax node classes                           |

Other new types:

| Type name               | Purpose                                                                        |
|-------------------------|--------------------------------------------------------------------------------|
| `JassSyntaxKind`        | Mainly used to tell the difference between literals and operators              |
| `JassSyntaxNodeOrToken` | Can be used to model "custom script actions" (one full line of the map script) |
| `JassSyntaxToken`       | Holds a token's text and trivia                                                |
| `JassSyntaxTrivia`      | Contains trivia as text (whitespace, newlines, single line comments)           |
| `JassSyntaxTriviaList`  | A collection of leading or trailing trivia for a token                         |

</details>

<details><summary>Renamed types</summary>

The following interfaces have been changed to abstract classes:

| Old type name                | New type name                         |
|------------------------------|---------------------------------------|
| `IExpressionSyntax`          | `JassExpressionSyntax`                |
| `IGlobalDeclarationSyntax`   | `JassGlobalDeclarationSyntax`         |
| `IStatementSyntax`           | `JassStatementSyntax`                 |
| `ITopLevelDeclarationSyntax` | `JassTopLevelDeclarationSyntax`       |
| `IVariableDeclaratorSyntax`  | `JassVariableOrArrayDeclaratorSyntax` |

The following classes have been renamed:

| Old class name                       | New class name                        |
|--------------------------------------|---------------------------------------|
| `JassArrayReferenceExpressionSyntax` | `JassElementAccessExpressionSyntax`   |
| `JassGlobalDeclarationListSyntax`    | `JassGlobalsDeclarationSyntax`        |
| `JassGlobalDeclarationSyntax`        | `JassGlobalVariableDeclarationSyntax` |

</details>

<details><summary>Replaced types</summary>

Specific literal expression classes have been removed, you can now use the generic JassLiteralExpressionSyntax class.

The new JassLiteralExpressionSyntax contains a token (with string text) property instead of the actual type of the expression.

| Old type                                 | New JassSyntaxKind                            |
|------------------------------------------|-----------------------------------------------|
| `JassBooleanLiteralExpressionSyntax`     | `JassSyntaxKind.BooleanLiteralExpression`     |
| `JassCharacterLiteralExpressionSyntax`   | `JassSyntaxKind.CharacterLiteralExpression`   |
| `JassDecimalLiteralExpressionSyntax`     | `JassSyntaxKind.DecimalLiteralExpression`     |
| `JassFourCCLiteralExpressionSyntax`      | `JassSyntaxKind.FourCCLiteralExpression`      |
| `JassHexadecimalLiteralExpressionSyntax` | `JassSyntaxKind.HexadecimalLiteralExpression` |
| `JassNullLiteralExpressionSyntax`        | `JassSyntaxKind.NullLiteralExpression`        |
| `JassOctalLiteralExpressionSyntax`       | `JassSyntaxKind.OctalLiteralExpression`       |
| `JassRealLiteralExpressionSyntax`        | `JassSyntaxKind.RealLiteralExpression`        |
| `JassStringLiteralExpressionSyntax`      | `JassSyntaxKind.StringLiteralExpression`      |

Script line interfaces have been removed, the new `JassSyntaxNodeOrToken` serves the same purpose:

| Removed interface        |
|--------------------------|
| `IDeclarationLineSyntax` |
| `IGlobalLineSyntax`      |
| `IStatementLineSyntax`   |

Custom script action classes have been removed, these have been replaced by new syntax node classes or the `JassSyntaxToken` class:

| Removed class                       | Replacement class or token kind     |
|-------------------------------------|-------------------------------------|
| `JassDebugCustomScriptAction`       | NO REPLACEMENT YET                  |
| `JassElseCustomScriptAction`        | `JassSyntaxKind.ElseKeyword`        |
| `JassElseIfCustomScriptAction`      | `JassElseIfClauseDeclaratorSyntax`  |
| `JassEndFunctionCustomScriptAction` | `JassSyntaxKind.EndFunctionKeyword` |
| `JassEndGlobalsCustomScriptAction`  | `JassSyntaxKind.EndGlobalsKeyword`  |
| `JassEndIfCustomScriptAction`       | `JassSyntaxKind.EndIfKeyword`       |
| `JassEndLoopCustomScriptAction`     | `JassSyntaxKind.EndLoopKeyword`     |
| `JassFunctionCustomScriptAction`    | `JassFunctionDeclaratorSyntax`      |
| `JassGlobalsCustomScriptAction`     | `JassSyntaxKind.GlobalsKeyword`     |
| `JassIfCustomScriptAction`          | `JassIfClauseDeclaratorSyntax`      |
| `JassLoopCustomScriptAction`        | `JassSyntaxKind.LoopKeyword`        |

The `BinaryOperatorType` and `UnaryOperatorType` enums have been removed, the type of the operator can now be determined by the syntax kind of the operator token:

| Old member name                     | New token kind                          |
|-------------------------------------|-----------------------------------------|
| `BinaryOperatorType.Add`            | `JassSyntaxKind.PlusToken`              |
| `BinaryOperatorType.Subtract`       | `JassSyntaxKind.MinusToken`             |
| `BinaryOperatorType.Multiplication` | `JassSyntaxKind.AsteriskToken`          |
| `BinaryOperatorType.Division`       | `JassSyntaxKind.SlashToken`             |
| `BinaryOperatorType.GreaterThan`    | `JassSyntaxKind.GreaterThanToken`       |
| `BinaryOperatorType.LessThan`       | `JassSyntaxKind.LessThanToken`          |
| `BinaryOperatorType.Equals`         | `JassSyntaxKind.EqualsEqualsToken`      |
| `BinaryOperatorType.NotEquals`      | `JassSyntaxKind.ExclamationEqualsToken` |
| `BinaryOperatorType.GreaterOrEqual` | `JassSyntaxKind.GreaterThanEqualsToken` |
| `BinaryOperatorType.LessOrEqual`    | `JassSyntaxKind.LessThanEqualsToken`    |
| `BinaryOperatorType.And`            | `JassSyntaxKind.AndKeyword`             |
| `BinaryOperatorType.Or`             | `JassSyntaxKind.OrKeyword`              |
| `UnaryOperatorType.Plus`            | `JassSyntaxKind.PlusToken`              |
| `UnaryOperatorType.Minus`           | `JassSyntaxKind.MinusToken`             |
| `UnaryOperatorType.Not`             | `JassSyntaxKind.NotKeyword`             |

Comments and empty lines are now handled by trivia:

| Removed class       |
| --------------------|
| `JassCommentSyntax` |
| `JassEmptySyntax`   |

</details>

<details><summary>Removed types</summary>

The following types have been removed:

| Removed type                  | Alternative    |
|-------------------------------|----------------|
| `IInvocationSyntax`           | NO ALTERNATIVE |
| `JassDebugCustomScriptAction` | NO ALTERNATIVE |

The following types are no longer relevant:

| Removed type                            | Alternative                                        |
|-----------------------------------------|----------------------------------------------------|
| `JassStatementListSyntax`               | Use `ImmutableArray<JassStatementSyntax>` directly |
| `JassVariableReferenceExpressionSyntax` | Use `JassIdentifierNameSyntax` directly            |

The following classes were unused and are no longer relevant:

| Removed class                    |
|----------------------------------|
| `IMemberDeclarationSyntax`       |
| `IScopedDeclarationSyntax`       |
| `IScopedGlobalDeclarationSyntax` |

</details>

<details><summary>Renamed members</summary>

The following methods have been renamed in syntax node classes:

| Old method name | New method name  |
|-----------------|------------------|
| `Equals`        | `IsEquivalentTo` |

The following properties have been renamed.

If the containing type has been changed or renamed, the new type name is also listed in the new column.

| Old property name                            | New property name                                       |
|----------------------------------------------|---------------------------------------------------------|
| `JassArgumentListSyntax.Arguments`           | `ArgumentList`                                          |
| `JassArrayReferenceExpressionSyntax.Indexer` | `JassElementAccessExpressionSyntax.ElementAccessClause` |
| `JassCallStatementSyntax.Arguments`          | `ArgumentList`                                          |
| `JassGlobalDeclarationListSyntax.Globals`    | `JassGlobalsDeclarationSyntax.GlobalDeclarations`       |
| `JassInvocationExpressionSyntax.Arguments`   | `ArgumentList`                                          |
| `JassParameterListSyntax.Empty`              | `JassEmptyParameterListSyntax.Value`                    |
| `JassParameterListSyntax.Parameters`         | `ParameterList`                                         |
| `JassTypeSyntax.Boolean`                     | `JassPredefinedTypeSyntax.Boolean`                      |
| `JassTypeSyntax.Code`                        | `JassPredefinedTypeSyntax.Code`                         |
| `JassTypeSyntax.Handle`                      | `JassPredefinedTypeSyntax.Handle`                       |
| `JassTypeSyntax.Integer`                     | `JassPredefinedTypeSyntax.Integer`                      |
| `JassTypeSyntax.Nothing`                     | `JassPredefinedTypeSyntax.Nothing`                      |
| `JassTypeSyntax.Real`                        | `JassPredefinedTypeSyntax.Real`                         |
| `JassTypeSyntax.String`                      | `JassPredefinedTypeSyntax.String`                       |

</details>

<details><summary>Replaced members</summary>

The following properties have been replaced:

| Old property name                     | New property type and name                          | Old type                   |
|---------------------------------------|-----------------------------------------------------|----------------------------|
| `JassBinaryExpressionSyntax.Operator` | `JassSyntaxToken OperatorToken`                     | `BinaryOperatorType`       |
| `JassElseClauseSyntax.Body`           | `ImmutableArray<JassStatementSyntax> Statements`    | `JassStatementListSyntax`  |
| `JassIdentifierNameSyntax.Name`       | `JassSyntaxToken Token`                             | `string`                   |
| `JassSetStatementSyntax.Indexer`      | `JassElementAccessClauseSyntax ElementAccessClause` | `IExpressionSyntax`        |
| `JassTypeSyntax.TypeName`             | `JassSyntaxToken Token`                             | `JassIdentifierNameSyntax` |
| `JassUnaryExpressionSyntax.Operator`  | `JassSyntaxToken OperatorToken`                     | `UnaryOperatorType`        |

Other replacements:

| Old property name                                        | Alternative                                          |
|----------------------------------------------------------|------------------------------------------------------|
| `JassBooleanLiteralExpressionSyntax.False`               | `JassSyntaxFactory.Literal(false)`                   |
| `JassBooleanLiteralExpressionSyntax.True`                | `JassSyntaxFactory.Literal(true)`                    |
| `JassNativeFunctionDeclarationSyntax.FunctionDeclarator` | All declarator properties are now available directly |
| `JassNullLiteralExpressionSyntax.Value`                  | `JassSyntaxFactory.Literal(null)`                    |

</details>

<details><summary>Other (breaking) changes</summary>

- JassTypeSyntax is now abstract and inherits JassExpressionSyntax
- JassIdentifierNameSyntax now inherits JassTypeSyntax -> JassExpressionSyntax
- Properties that used to be of type `JassParameterListSyntax` are now `JassParameterListOrEmptyParameterListSyntax`
- The constructor of syntax node classes is now internal
- Properties have been changed from `{ get; init; }` to `{ get; }`
- `JassSymbol` now contains both `char` and `string` constants, existing `char` constants got the `Char` suffix
- `JassSyntaxFacts.IsWhitespaceCharacter(char)` now only considers spaces and tabs to be whitespace (previously used `char.IsWhitespace` and excluded \r and \n)

</details>

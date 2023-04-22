// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxKind.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass
{
    public enum JassSyntaxKind
    {
        None = 0,

        /// <summary>Represents <c>*</c> token.</summary>
        /// <seealso cref="JassSymbol.Asterisk"/>
        AsteriskToken = 8199,

        /// <summary>Represents <c>(</c> token.</summary>
        /// <seealso cref="JassSymbol.OpenParen"/>
        OpenParenToken = 8200,

        /// <summary>Represents <c>)</c> token.</summary>
        /// <seealso cref="JassSymbol.CloseParen"/>
        CloseParenToken = 8201,

        /// <summary>Represents <c>-</c> token.</summary>
        /// <seealso cref="JassSymbol.Minus"/>
        MinusToken = 8202,

        /// <summary>Represents <c>+</c> token.</summary>
        /// <seealso cref="JassSymbol.Plus"/>
        PlusToken = 8203,

        /// <summary>Represents <c>=</c> token.</summary>
        /// <seealso cref="JassSymbol.Equals"/>
        EqualsToken = 8204,

        /// <summary>Represents <c>[</c> token.</summary>
        /// <seealso cref="JassSymbol.OpenBracket"/>
        OpenBracketToken = 8207,

        /// <summary>Represents <c>]</c> token.</summary>
        /// <seealso cref="JassSymbol.CloseBracket"/>
        CloseBracketToken = 8208,

        /// <summary>Represents <c>&lt;</c> token.</summary>
        /// <seealso cref="JassSymbol.LessThan"/>
        LessThanToken = 8215,

        /// <summary>Represents <c>,</c> token.</summary>
        /// <seealso cref="JassSymbol.Comma"/>
        CommaToken = 8216,

        /// <summary>Represents <c>&gt;</c> token.</summary>
        /// <seealso cref="JassSymbol.GreaterThan"/>
        GreaterThanToken = 8217,

        /// <summary>Represents <c>/</c> token.</summary>
        /// <seealso cref="JassSymbol.Slash"/>
        SlashToken = 8221,

        /// <summary>Represents <c>!=</c> token.</summary>
        /// <seealso cref="JassSymbol.ExclamationEquals"/>
        ExclamationEqualsToken = 8267,

        /// <summary>Represents <c>==</c> token.</summary>
        /// <seealso cref="JassSymbol.EqualsEquals"/>
        EqualsEqualsToken = 8268,

        /// <summary>Represents <c>&lt;=</c> token.</summary>
        /// <seealso cref="JassSymbol.LessThanEquals"/>
        LessThanEqualsToken = 8270,

        /// <summary>Represents <c>&gt;=</c> token.</summary>
        /// <seealso cref="JassSymbol.GreaterThanEquals"/>
        GreaterThanEqualsToken = 8273,

        /// <summary>Represents <c>boolean</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Boolean"/>
        BooleanKeyword = 8304,

        /// <summary>Represents <c>integer</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Integer"/>
        IntegerKeyword = 8309,

        /// <summary>Represents <c>real</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Real"/>
        RealKeyword = 8314,

        /// <summary>Represents <c>string</c> keyword.</summary>
        /// <seealso cref="JassKeyword.String"/>
        StringKeyword = 8316,

        /// <summary>Represents <c>nothing</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Nothing"/>
        NothingKeyword = 8318,

        /// <summary>Represents <c>handle</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Handle"/>
        HandleKeyword = 8319,

        /// <summary>Represents <c>null</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Null"/>
        NullKeyword = 8322,

        /// <summary>Represents <c>true</c> keyword.</summary>
        /// <seealso cref="JassKeyword.True"/>
        TrueKeyword = 8323,

        /// <summary>Represents <c>false</c> keyword.</summary>
        /// <seealso cref="JassKeyword.False"/>
        FalseKeyword = 8324,

        /// <summary>Represents <c>if</c> keyword.</summary>
        /// <seealso cref="JassKeyword.If"/>
        IfKeyword = 8325,

        /// <summary>Represents <c>elseif</c> keyword.</summary>
        /// <seealso cref="JassKeyword.ElseIf"/>
        ElseIfKeyword = 8326,

        /// <summary>Represents <c>then</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Then"/>
        ThenKeyword = 8327,

        /// <summary>Represents <c>else</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Else"/>
        ElseKeyword = 8328,

        /// <summary>Represents <c>endif</c> keyword.</summary>
        /// <seealso cref="JassKeyword.EndIf"/>
        EndIfKeyword = 8329,

        /// <summary>Represents <c>loop</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Loop"/>
        LoopKeyword = 8330,

        /// <summary>Represents <c>exitwhen</c> keyword.</summary>
        /// <seealso cref="JassKeyword.ExitWhen"/>
        ExitWhenKeyword = 8339,

        /// <summary>Represents <c>endloop</c> keyword.</summary>
        /// <seealso cref="JassKeyword.EndLoop"/>
        EndLoopKeyword = 8340,

        /// <summary>Represents <c>return</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Return"/>
        ReturnKeyword = 8341,

        /// <summary>Represents <c>call</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Call"/>
        CallKeyword = 8342,

        /// <summary>Represents <c>set</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Set"/>
        SetKeyword = 8343,

        /// <summary>Represents <c>local</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Local"/>
        LocalKeyword = 8344,

        /// <summary>Represents <c>debug</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Debug"/>
        DebugKeyword = 8345,

        /// <summary>Represents <c>constant</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Constant"/>
        ConstantKeyword = 8350,

        /// <summary>Represents <c>function</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Function"/>
        FunctionKeyword = 8351,

        /// <summary>Represents <c>takes</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Takes"/>
        TakesKeyword = 8352,

        /// <summary>Represents <c>returns</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Returns"/>
        ReturnsKeyword = 8353,

        /// <summary>Represents <c>endfunction</c> keyword.</summary>
        /// <seealso cref="JassKeyword.EndFunction"/>
        EndFunctionKeyword = 8354,

        /// <summary>Represents <c>native</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Native"/>
        NativeKeyword = 8359,

        /// <summary>Represents <c>extends</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Extends"/>
        ExtendsKeyword = 8371,

        /// <summary>Represents <c>code</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Code"/>
        CodeKeyword = 8378,

        /// <summary>Represents <c>alias</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Alias"/>
        AliasKeyword = 8407,

        /// <summary>Represents <c>array</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Array"/>
        ArrayKeyword = 8408,

        /// <summary>Represents <c>globals</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Globals"/>
        GlobalsKeyword = 8409,

        /// <summary>Represents <c>endglobals</c> keyword.</summary>
        /// <seealso cref="JassKeyword.EndGlobals"/>
        EndGlobalsKeyword = 8410,

        /// <summary>Represents <c>type</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Type"/>
        TypeKeyword = 8411,

        /// <summary>Represents <c>or</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Or"/>
        OrKeyword = 8438,

        /// <summary>Represents <c>and</c> keyword.</summary>
        /// <seealso cref="JassKeyword.And"/>
        AndKeyword = 8439,

        /// <summary>Represents <c>not</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Not"/>
        NotKeyword = 8440,

        /// <summary>Represents the end of a file.</summary>
        EndOfFileToken = 8496,

        IdentifierToken = 8508,

        RealLiteralToken = 8509,
        CharacterLiteralToken = 8510,
        StringLiteralToken = 8511,
        DecimalLiteralToken = 8512,
        HexadecimalLiteralToken = 8513,
        OctalLiteralToken = 8514,
        FourCCLiteralToken = 8515,

        NewlineTrivia = 8539,
        WhitespaceTrivia = 8540,
        SingleLineCommentTrivia = 8541,

        /// <summary>Represents <see cref="JassIdentifierNameSyntax"/>.</summary>
        IdentifierName = 8616,

        /// <summary>Represents <see cref="JassPredefinedTypeSyntax"/>.</summary>
        PredefinedType = 8621,

        /// <summary>Represents <see cref="JassParenthesizedExpressionSyntax"/>.</summary>
        ParenthesizedExpression = 8632,

        /// <summary>Represents <see cref="JassFunctionReferenceExpressionSyntax"/>.</summary>
        FunctionReferenceExpression = 8633,

        /// <summary>Represents <see cref="JassInvocationExpressionSyntax"/>.</summary>
        InvocationExpression = 8634,

        /// <summary>Represents <see cref="JassElementAccessExpressionSyntax"/>.</summary>
        ElementAccessExpression = 8635,

        /// <summary>Represents <see cref="JassArgumentListSyntax"/>.</summary>
        ArgumentList = 8636,

        /// <summary>Represents <see cref="JassElementAccessClauseSyntax"/>.</summary>
        ElementAccessClause = 8637,

        /// <summary>Represents <see cref="JassBinaryExpressionSyntax"/> with <see cref="PlusToken"/> as operator token kind.</summary>
        AddExpression = 8668,

        /// <summary>Represents <see cref="JassBinaryExpressionSyntax"/> with <see cref="MinusToken"/> as operator token kind.</summary>
        SubtractExpression = 8669,

        /// <summary>Represents <see cref="JassBinaryExpressionSyntax"/> with <see cref="AsteriskToken"/> as operator token kind.</summary>
        MultiplyExpression = 8670,

        /// <summary>Represents <see cref="JassBinaryExpressionSyntax"/> with <see cref="SlashToken"/> as operator token kind.</summary>
        DivideExpression = 8671,

        /// <summary>Represents <see cref="JassBinaryExpressionSyntax"/> with <see cref="OrKeyword"/> as operator token kind.</summary>
        LogicalOrExpression = 8675,

        /// <summary>Represents <see cref="JassBinaryExpressionSyntax"/> with <see cref="AndKeyword"/> as operator token kind.</summary>
        LogicalAndExpression = 8676,

        /// <summary>Represents <see cref="JassBinaryExpressionSyntax"/> with <see cref="EqualsEqualsToken"/> as operator token kind.</summary>
        EqualsExpression = 8680,

        /// <summary>Represents <see cref="JassBinaryExpressionSyntax"/> with <see cref="ExclamationEqualsToken"/> as operator token kind.</summary>
        NotEqualsExpression = 8681,

        /// <summary>Represents <see cref="JassBinaryExpressionSyntax"/> with <see cref="LessThanToken"/> as operator token kind.</summary>
        LessThanExpression = 8682,

        /// <summary>Represents <see cref="JassBinaryExpressionSyntax"/> with <see cref="LessThanEqualsToken"/> as operator token kind.</summary>
        LessThanOrEqualExpression = 8683,

        /// <summary>Represents <see cref="JassBinaryExpressionSyntax"/> with <see cref="GreaterThanExpression"/> as operator token kind.</summary>
        GreaterThanExpression = 8684,

        /// <summary>Represents <see cref="JassBinaryExpressionSyntax"/> with <see cref="GreaterThanEqualsToken"/> as operator token kind.</summary>
        GreaterThanOrEqualExpression = 8685,

        /// <summary>Represents <see cref="JassSetStatementSyntax"/>.</summary>
        SetStatement = 8714,

        /// <summary>Represents <see cref="JassUnaryExpressionSyntax"/> with <see cref="PlusToken"/> as operator token kind.</summary>
        UnaryPlusExpression = 8730,

        /// <summary>Represents <see cref="JassUnaryExpressionSyntax"/> with <see cref="MinusToken"/> as operator token kind.</summary>
        UnaryMinusExpression = 8731,

        /// <summary>Represents <see cref="JassUnaryExpressionSyntax"/> with <see cref="NotKeyword"/> as operator token kind.</summary>
        LogicalNotExpression = 8733,

        /// <summary>Represents <see cref="JassLiteralExpressionSyntax"/> with <see cref="RealLiteralToken"/> as token kind.</summary>
        RealLiteralExpression = 8749,

        /// <summary>Represents <see cref="JassLiteralExpressionSyntax"/> with <see cref="StringLiteralToken"/> as token kind.</summary>
        StringLiteralExpression = 8750,

        /// <summary>Represents <see cref="JassLiteralExpressionSyntax"/> with <see cref="CharacterLiteralToken"/> as token kind.</summary>
        CharacterLiteralExpression = 8751,

        /// <summary>Represents <see cref="JassLiteralExpressionSyntax"/> with <see cref="TrueKeyword"/> as token kind.</summary>
        TrueLiteralExpression = 8752,

        /// <summary>Represents <see cref="JassLiteralExpressionSyntax"/> with <see cref="FalseKeyword"/> as token kind.</summary>
        FalseLiteralExpression = 8753,

        /// <summary>Represents <see cref="JassLiteralExpressionSyntax"/> with <see cref="NullKeyword"/> as token kind.</summary>
        NullLiteralExpression = 8754,

        /// <summary>Represents <see cref="JassLiteralExpressionSyntax"/> with <see cref="DecimalLiteralToken"/> as token kind.</summary>
        DecimalLiteralExpression = 8755,

        /// <summary>Represents <see cref="JassLiteralExpressionSyntax"/> with <see cref="HexadecimalLiteralToken"/> as token kind.</summary>
        HexadecimalLiteralExpression = 8756,

        /// <summary>Represents <see cref="JassLiteralExpressionSyntax"/> with <see cref="OctalLiteralToken"/> as token kind.</summary>
        OctalLiteralExpression = 8757,

        /// <summary>Represents <see cref="JassLiteralExpressionSyntax"/> with <see cref="FourCCLiteralToken"/> as token kind.</summary>
        FourCCLiteralExpression = 8758,

        /// <summary>Represents <see cref="JassLocalVariableDeclarationStatementSyntax"/> with <see cref="VariableDeclarator"/> as declarator kind.</summary>
        LocalVariableDeclarationStatement = 8793,

        /// <summary>Represents <see cref="JassLocalVariableDeclarationStatementSyntax"/> with <see cref="ArrayDeclarator"/> as declarator kind.</summary>
        LocalArrayDeclarationStatement = 8794,

        /// <summary>Represents <see cref="JassVariableDeclaratorSyntax"/>.</summary>
        VariableDeclarator = 8795,

        /// <summary>Represents <see cref="JassEqualsValueClauseSyntax"/>.</summary>
        EqualsValueClause = 8796,

        /// <summary>Represents <see cref="JassArrayDeclaratorSyntax"/>.</summary>
        ArrayDeclarator = 8797,

        /// <summary>Represents <see cref="JassExitStatementSyntax"/>.</summary>
        ExitStatement = 8803,

        /// <summary>Represents <see cref="JassReturnStatementSyntax"/>.</summary>
        ReturnStatement = 8805,

        /// <summary>Represents <see cref="JassCallStatementSyntax"/>.</summary>
        CallStatement = 8808,

        /// <summary>Represents <see cref="JassLoopStatementSyntax"/>.</summary>
        LoopStatement = 8809,

        /// <summary>Represents <see cref="JassIfStatementSyntax"/>.</summary>
        IfStatement = 8819,

        /// <summary>Represents <see cref="JassIfClauseSyntax"/>.</summary>
        IfClause = 8820,

        /// <summary>Represents <see cref="JassIfClauseDeclaratorSyntax"/>.</summary>
        IfClauseDeclarator = 8821,

        /// <summary>Represents <see cref="JassElseIfClauseSyntax"/>.</summary>
        ElseIfClause = 8822,

        /// <summary>Represents <see cref="JassElseIfClauseDeclaratorSyntax"/>.</summary>
        ElseIfClauseDeclarator = 8823,

        /// <summary>Represents <see cref="JassElseClauseSyntax"/>.</summary>
        ElseClause = 8824,

        /// <summary>Represents <see cref="JassDebugStatementSyntax"/> with <see cref="SetStatement"/> as statement kind.</summary>
        DebugSetStatement = 8825,

        /// <summary>Represents <see cref="JassDebugStatementSyntax"/> with <see cref="CallStatement"/> as statement kind.</summary>
        DebugCallStatement = 8826,

        /// <summary>Represents <see cref="JassDebugStatementSyntax"/> with <see cref="LoopStatement"/> as statement kind.</summary>
        DebugLoopStatement = 8827,

        /// <summary>Represents <see cref="JassDebugStatementSyntax"/> with <see cref="IfStatement"/> as statement kind.</summary>
        DebugIfStatement = 8828,

        /// <summary>Represents <see cref="JassCompilationUnitSyntax"/>.</summary>
        CompilationUnit = 8840,

        /// <summary>Represents <see cref="JassTypeDeclarationSyntax"/>.</summary>
        TypeDeclaration = 8855,

        /// <summary>Represents <see cref="JassNativeFunctionDeclarationSyntax"/>.</summary>
        NativeFunctionDeclaration = 8859,

        /// <summary>Represents <see cref="JassFunctionDeclarationSyntax"/>.</summary>
        FunctionDeclaration = 8875,

        /// <summary>Represents <see cref="JassFunctionDeclaratorSyntax"/>.</summary>
        FunctionDeclarator = 8876,

        /// <summary>Represents <see cref="JassParameterListSyntax"/>.</summary>
        ParameterList = 8906,

        /// <summary>Represents <see cref="JassEmptyParameterListSyntax"/>.</summary>
        EmptyParameterList = 8907,

        /// <summary>Represents <see cref="JassParameterSyntax"/>.</summary>
        Parameter = 8908,

        /// <summary>Represents <see cref="JassReturnClauseSyntax"/>.</summary>
        ReturnClause = 8909,

        /// <summary>Represents <see cref="JassGlobalsDeclarationSyntax"/>.</summary>
        GlobalsDeclaration = 8911,

        /// <summary>Represents <see cref="JassGlobalConstantDeclarationSyntax"/>.</summary>
        GlobalConstantDeclaration = 8912,

        /// <summary>Represents <see cref="JassGlobalVariableDeclarationSyntax"/> with <see cref="VariableDeclarator"/> as declarator kind.</summary>
        GlobalVariableDeclaration = 8913,

        /// <summary>Represents <see cref="JassGlobalVariableDeclarationSyntax"/> with <see cref="ArrayDeclarator"/> as declarator kind.</summary>
        GlobalArrayDeclaration = 8914,
    }
}
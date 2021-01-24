// ------------------------------------------------------------------------------
// <copyright file="SyntaxTokenType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Jass
{
    [Obsolete]
    public enum SyntaxTokenType
    {
        Undefined,
        EndOfFile,

        // Keywords
        AliasKeyword,
        ArrayKeyword,
        BooleanKeyword,
        CallKeyword,
        CodeKeyword,
        ConstantKeyword,
        DebugKeyword,
        ElseKeyword,
        ElseifKeyword,
        EndfunctionKeyword,
        EndglobalsKeyword,
        EndifKeyword,
        EndloopKeyword,
        ExitwhenKeyword,
        ExtendsKeyword,
        FalseKeyword,
        FunctionKeyword,
        GlobalsKeyword,
        HandleKeyword,
        IfKeyword,
        IntegerKeyword,
        LocalKeyword,
        LoopKeyword,
        NativeKeyword,
        NothingKeyword,
        NullKeyword,
        RealKeyword,
        ReturnKeyword,
        ReturnsKeyword,
        SetKeyword,
        StringKeyword,
        TakesKeyword,
        ThenKeyword,
        TrueKeyword,
        TypeKeyword,

        // Operators
        PlusOperator,
        MinusOperator,
        MultiplicationOperator,
        DivisionOperator,
        GreaterThanOperator,
        LessThanOperator,
        EqualityOperator,
        UnequalityOperator,
        GreaterOrEqualOperator,
        LessOrEqualOperator,
        AndOperator,
        OrOperator,
        NotOperator,

        // Other Symbols
        ParenthesisOpenSymbol,
        ParenthesisCloseSymbol,
        SquareBracketOpenSymbol,
        SquareBracketCloseSymbol,
        Assignment,
        NewlineSymbol,
        SingleQuote,
        DoubleQuotes,
        Comma,
        DoubleForwardSlash,

        // Alphanumericals
        AlphanumericIdentifier,
        DecimalNumber,
        OctalNumber,
        HexadecimalNumber,
        Character,
        FourCCNumber,
        RealNumber,
        String,
        Comment,
    }
}
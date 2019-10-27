// ------------------------------------------------------------------------------
// <copyright file="SyntaxTokenType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.CodeAnalysis.Jass
{
    public enum SyntaxTokenType
    {
        Undefined,
        EndOfFile,

        // Keywords
        TypeKeyword,
        ExtendsKeyword,
        HandleKeyword,
        GlobalsKeyword,
        EndglobalsKeyword,
        ConstantKeyword,
        NativeKeyword,
        TakesKeyword,
        NothingKeyword,
        ReturnsKeyword,
        FunctionKeyword,
        EndfunctionKeyword,
        LocalKeyword,
        ArrayKeyword,
        SetKeyword,
        CallKeyword,
        IfKeyword,
        ThenKeyword,
        EndifKeyword,
        ElseKeyword,
        ElseifKeyword,
        LoopKeyword,
        EndloopKeyword,
        ExitwhenKeyword,
        ReturnKeyword,
        DebugKeyword,
        NullKeyword,
        TrueKeyword,
        FalseKeyword,
        CodeKeyword,
        IntegerKeyword,
        RealKeyword,
        BooleanKeyword,
        StringKeyword,

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
        FourCCNumber,
        RealNumber,
        String,
        Comment,
    }
}
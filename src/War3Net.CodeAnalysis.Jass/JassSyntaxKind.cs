// ------------------------------------------------------------------------------
// <copyright file="JassSyntaxKind.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

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

        /// <summary>Represents <c>bool</c> keyword.</summary>
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

        /// <summary>Represents <c>else</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Else"/>
        ElseKeyword = 8326,

        /// <summary>Represents <c>loop</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Loop"/>
        LoopKeyword = 8327,

        /// <summary>Represents <c>endloop</c> keyword.</summary>
        /// <seealso cref="JassKeyword.EndLoop"/>
        EndLoopKeyword = 8330,

        /// <summary>Represents <c>exitwhen</c> keyword.</summary>
        /// <seealso cref="JassKeyword.ExitWhen"/>
        ExitWhenKeyword = 8339,

        /// <summary>Represents <c>return</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Return"/>
        ReturnKeyword = 8341,

        /// <summary>Represents <c>constant</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Constant"/>
        ConstantKeyword = 8350,

        /// <summary>Represents <c>native</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Native"/>
        NativeKeyword = 8359,

        /// <summary>Represents <c>alias</c> keyword.</summary>
        /// <seealso cref="JassKeyword.Alias"/>
        AliasKeyword = 8407,

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

        /// <summary>Represents <c>elseif</c> keyword.</summary>
        /// <seealso cref="JassKeyword.ElseIf"/>
        ElseIfKeyword = 8467,

        /// <summary>Represents <c>endif</c> keyword.</summary>
        /// <seealso cref="JassKeyword.EndIf"/>
        EndIfKeyword = 8468,

        /// <summary>Represents the end of a file.</summary>
        EndOfFileToken = 8496,
    }
}
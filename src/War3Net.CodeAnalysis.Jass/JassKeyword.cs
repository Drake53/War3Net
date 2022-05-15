﻿// ------------------------------------------------------------------------------
// <copyright file="JassKeyword.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1720, SA1600

using System;
using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass
{
    public static class JassKeyword
    {
        public const string Alias = "alias";
        public const string And = "and";
        public const string Array = "array";
        public const string Boolean = "boolean";
        public const string Call = "call";
        public const string Code = "code";
        public const string Constant = "constant";
        public const string Debug = "debug";
        public const string Else = "else";
        public const string ElseIf = "elseif";
        public const string EndFunction = "endfunction";
        public const string EndGlobals = "endglobals";
        public const string EndIf = "endif";
        public const string EndLoop = "endloop";
        public const string ExitWhen = "exitwhen";
        public const string Extends = "extends";
        public const string False = "false";
        public const string Function = "function";
        public const string Globals = "globals";
        public const string Handle = "handle";
        public const string If = "if";
        public const string Integer = "integer";
        public const string Local = "local";
        public const string Loop = "loop";
        public const string Native = "native";
        public const string Not = "not";
        public const string Nothing = "nothing";
        public const string Null = "null";
        public const string Or = "or";
        public const string Real = "real";
        public const string Return = "return";
        public const string Returns = "returns";
        public const string Set = "set";
        public const string String = "string";
        public const string Takes = "takes";
        public const string Then = "then";
        public const string True = "true";
        public const string Type = "type";

        private static readonly HashSet<string> _keywords = new(StringComparer.Ordinal)
        {
            Alias,
            And,
            Array,
            Boolean,
            Call,
            Code,
            Constant,
            Debug,
            Else,
            ElseIf,
            EndFunction,
            EndGlobals,
            EndIf,
            EndLoop,
            ExitWhen,
            Extends,
            False,
            Function,
            Globals,
            Handle,
            If,
            Integer,
            Local,
            Loop,
            Native,
            Not,
            Nothing,
            Null,
            Or,
            Real,
            Return,
            Returns,
            Set,
            String,
            Takes,
            Then,
            True,
            Type,
        };

        public static bool IsKeyword(string value) => _keywords.Contains(value);
    }
}
// ------------------------------------------------------------------------------
// <copyright file="EnumMemberIdentifierExpressionAttribute.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.CSharp.Attributes
{
    /// <summary>
    /// For use with enum members, where the enum type has <see cref="EnumCastMethodAttribute"/>.
    /// This attribute defines a global constant, that should be defined in lua code, that this enum member transpiles to.
    /// </summary>
    /// <example>
    /// In lua, the constant PLAYER_COLOR_RED is defined as ConvertPlayerColor(0).
    /// With that, instead of transpiling enum member 'playercolor.red = 0' to ConvertPlayerColor(0), it becomes PLAYER_COLOR_RED.
    /// </example>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class EnumMemberIdentifierExpressionAttribute : Attribute
    {
        private readonly string _transpilesTo;

        public EnumMemberIdentifierExpressionAttribute(string transpilesTo)
        {
            _transpilesTo = transpilesTo;
        }

        public string TranspilesTo => _transpilesTo;
    }
}
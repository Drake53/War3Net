// ------------------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.ComponentModel;

namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class TypeExtensions
    {
        public static SyntaxTokenType ToJassTypeKeyword(this Type type)
        {
            var typeCode = Type.GetTypeCode(type);
            return typeCode switch
            {
                TypeCode.Empty => SyntaxTokenType.NullKeyword,
                TypeCode.Object => SyntaxTokenType.HandleKeyword,
                TypeCode.Boolean => SyntaxTokenType.BooleanKeyword,
                TypeCode.Char => SyntaxTokenType.IntegerKeyword,
                TypeCode.SByte => SyntaxTokenType.IntegerKeyword,
                TypeCode.Byte => SyntaxTokenType.IntegerKeyword,
                TypeCode.Int16 => SyntaxTokenType.IntegerKeyword,
                TypeCode.UInt16 => SyntaxTokenType.IntegerKeyword,
                TypeCode.Int32 => SyntaxTokenType.IntegerKeyword,
                TypeCode.UInt32 => SyntaxTokenType.IntegerKeyword,
                TypeCode.Int64 => SyntaxTokenType.IntegerKeyword,
                TypeCode.UInt64 => SyntaxTokenType.IntegerKeyword,
                TypeCode.Single => SyntaxTokenType.RealKeyword,
                TypeCode.Double => SyntaxTokenType.RealKeyword,
                TypeCode.Decimal => SyntaxTokenType.RealKeyword,
                TypeCode.String => SyntaxTokenType.StringKeyword,

                _ => throw new InvalidEnumArgumentException(nameof(typeCode), (int)typeCode, typeof(TypeCode)),
            };
        }
    }
}
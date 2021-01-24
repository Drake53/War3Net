// ------------------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.ComponentModel;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Jass.Extensions
{
    public static class TypeExtensions
    {
        public static JassTypeSyntax ToJassType(this Type type)
        {
            var typeCode = Type.GetTypeCode(type);
            return typeCode switch
            {
                TypeCode.Empty => JassTypeSyntax.Nothing,
                TypeCode.Object => JassTypeSyntax.Handle,
                TypeCode.Boolean => JassTypeSyntax.Boolean,
                TypeCode.Char => JassTypeSyntax.Integer,
                TypeCode.SByte => JassTypeSyntax.Integer,
                TypeCode.Byte => JassTypeSyntax.Integer,
                TypeCode.Int16 => JassTypeSyntax.Integer,
                TypeCode.UInt16 => JassTypeSyntax.Integer,
                TypeCode.Int32 => JassTypeSyntax.Integer,
                TypeCode.UInt32 => JassTypeSyntax.Integer,
                TypeCode.Int64 => JassTypeSyntax.Integer,
                TypeCode.UInt64 => JassTypeSyntax.Integer,
                TypeCode.Single => JassTypeSyntax.Real,
                TypeCode.Double => JassTypeSyntax.Real,
                TypeCode.Decimal => JassTypeSyntax.Real,
                TypeCode.String => JassTypeSyntax.String,

                _ => throw new InvalidEnumArgumentException(nameof(typeCode), (int)typeCode, typeof(TypeCode)),
            };
        }
    }
}
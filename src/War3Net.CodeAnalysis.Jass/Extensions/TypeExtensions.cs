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
                TypeCode.Empty => JassPredefinedTypeSyntax.Nothing,
                TypeCode.Object => JassPredefinedTypeSyntax.Handle,
                TypeCode.Boolean => JassPredefinedTypeSyntax.Boolean,
                TypeCode.Char => JassPredefinedTypeSyntax.Integer,
                TypeCode.SByte => JassPredefinedTypeSyntax.Integer,
                TypeCode.Byte => JassPredefinedTypeSyntax.Integer,
                TypeCode.Int16 => JassPredefinedTypeSyntax.Integer,
                TypeCode.UInt16 => JassPredefinedTypeSyntax.Integer,
                TypeCode.Int32 => JassPredefinedTypeSyntax.Integer,
                TypeCode.UInt32 => JassPredefinedTypeSyntax.Integer,
                TypeCode.Int64 => JassPredefinedTypeSyntax.Integer,
                TypeCode.UInt64 => JassPredefinedTypeSyntax.Integer,
                TypeCode.Single => JassPredefinedTypeSyntax.Real,
                TypeCode.Double => JassPredefinedTypeSyntax.Real,
                TypeCode.Decimal => JassPredefinedTypeSyntax.Real,
                TypeCode.String => JassPredefinedTypeSyntax.String,

                _ => throw new InvalidEnumArgumentException(nameof(typeCode), (int)typeCode, typeof(TypeCode)),
            };
        }
    }
}
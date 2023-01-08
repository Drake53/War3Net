// ------------------------------------------------------------------------------
// <copyright file="EnumConvert.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Runtime.CompilerServices;

using War3Net.Common.Extensions;

namespace War3Net.Common
{
    public static class EnumConvert<TEnum>
        where TEnum : struct, Enum
    {
        public static TEnum FromByte(byte value, bool allowNoFlags = true)
        {
            if (Enum.GetUnderlyingType(typeof(TEnum)) != typeof(byte))
            {
                throw new InvalidOperationException($"FromByte requires that enum of type {typeof(TEnum).Name} uses byte as its underlying type.");
            }

            var result = Unsafe.As<byte, TEnum>(ref value);
            if (!result.IsDefined(allowNoFlags))
            {
                var displayValue = Attribute.GetCustomAttribute(typeof(TEnum), typeof(FlagsAttribute)) is null
                    ? value.ToString(CultureInfo.InvariantCulture)
                    : $"0b{Convert.ToString(value, 2).PadLeft(8, '0')}";

                throw new ArgumentException($"Value '{displayValue}' is not defined for enum of type {typeof(TEnum).Name}.");
            }

            return result;
        }

        public static TEnum FromSByte(sbyte value, bool allowNoFlags = true)
        {
            if (Enum.GetUnderlyingType(typeof(TEnum)) != typeof(sbyte))
            {
                throw new InvalidOperationException($"FromSByte requires that enum of type {typeof(TEnum).Name} uses sbyte as its underlying type.");
            }

            var result = Unsafe.As<sbyte, TEnum>(ref value);
            if (!result.IsDefined(allowNoFlags))
            {
                var displayValue = Attribute.GetCustomAttribute(typeof(TEnum), typeof(FlagsAttribute)) is null
                    ? value.ToString(CultureInfo.InvariantCulture)
                    : $"0b{Convert.ToString((byte)value, 2).PadLeft(8, '0')}";

                throw new ArgumentException($"Value '{displayValue}' is not defined for enum of type {typeof(TEnum).Name}.");
            }

            return result;
        }

        public static TEnum FromInt16(short value, bool allowNoFlags = true)
        {
            if (Enum.GetUnderlyingType(typeof(TEnum)) != typeof(short))
            {
                throw new InvalidOperationException($"FromInt16 requires that enum of type {typeof(TEnum).Name} uses short as its underlying type.");
            }

            var result = Unsafe.As<short, TEnum>(ref value);
            if (!result.IsDefined(allowNoFlags))
            {
                var displayValue = Attribute.GetCustomAttribute(typeof(TEnum), typeof(FlagsAttribute)) is null
                    ? value.ToString(CultureInfo.InvariantCulture)
                    : $"0b{Convert.ToString(value, 2).PadLeft(16, '0')}";

                throw new ArgumentException($"Value '{displayValue}' is not defined for enum of type {typeof(TEnum).Name}.");
            }

            return result;
        }

        public static TEnum FromUInt16(ushort value, bool allowNoFlags = true)
        {
            if (Enum.GetUnderlyingType(typeof(TEnum)) != typeof(ushort))
            {
                throw new InvalidOperationException($"FromUInt16 requires that enum of type {typeof(TEnum).Name} uses ushort as its underlying type.");
            }

            var result = Unsafe.As<ushort, TEnum>(ref value);
            if (!result.IsDefined(allowNoFlags))
            {
                var displayValue = Attribute.GetCustomAttribute(typeof(TEnum), typeof(FlagsAttribute)) is null
                    ? value.ToString(CultureInfo.InvariantCulture)
                    : $"0b{Convert.ToString((short)value, 2).PadLeft(16, '0')}";

                throw new ArgumentException($"Value '{displayValue}' is not defined for enum of type {typeof(TEnum).Name}.");
            }

            return result;
        }

        public static TEnum FromInt32(int value, bool allowNoFlags = true)
        {
            if (Enum.GetUnderlyingType(typeof(TEnum)) != typeof(int))
            {
                throw new InvalidOperationException($"FromInt32 requires that enum of type {typeof(TEnum).Name} uses int as its underlying type.");
            }

            var result = Unsafe.As<int, TEnum>(ref value);
            if (!result.IsDefined(allowNoFlags))
            {
                var displayValue = Attribute.GetCustomAttribute(typeof(TEnum), typeof(FlagsAttribute)) is null
                    ? value.ToString(CultureInfo.InvariantCulture)
                    : $"0b{Convert.ToString(value, 2).PadLeft(32, '0')}";

                throw new ArgumentException($"Value '{displayValue}' is not defined for enum of type {typeof(TEnum).Name}.");
            }

            return result;
        }

        public static TEnum FromUInt32(uint value, bool allowNoFlags = true)
        {
            if (Enum.GetUnderlyingType(typeof(TEnum)) != typeof(uint))
            {
                throw new InvalidOperationException($"FromUInt32 requires that enum of type {typeof(TEnum).Name} uses uint as its underlying type.");
            }

            var result = Unsafe.As<uint, TEnum>(ref value);
            if (!result.IsDefined(allowNoFlags))
            {
                var displayValue = Attribute.GetCustomAttribute(typeof(TEnum), typeof(FlagsAttribute)) is null
                    ? value.ToString(CultureInfo.InvariantCulture)
                    : $"0b{Convert.ToString((int)value, 2).PadLeft(32, '0')}";

                throw new ArgumentException($"Value '{displayValue}' is not defined for enum of type {typeof(TEnum).Name}.");
            }

            return result;
        }

        public static TEnum FromInt64(long value, bool allowNoFlags = true)
        {
            if (Enum.GetUnderlyingType(typeof(TEnum)) != typeof(long))
            {
                throw new InvalidOperationException($"FromInt64 requires that enum of type {typeof(TEnum).Name} uses long as its underlying type.");
            }

            var result = Unsafe.As<long, TEnum>(ref value);
            if (!result.IsDefined(allowNoFlags))
            {
                var displayValue = Attribute.GetCustomAttribute(typeof(TEnum), typeof(FlagsAttribute)) is null
                    ? value.ToString(CultureInfo.InvariantCulture)
                    : $"0b{Convert.ToString(value, 2).PadLeft(64, '0')}";

                throw new ArgumentException($"Value '{displayValue}' is not defined for enum of type {typeof(TEnum).Name}.");
            }

            return result;
        }

        public static TEnum FromUInt64(ulong value, bool allowNoFlags = true)
        {
            if (Enum.GetUnderlyingType(typeof(TEnum)) != typeof(ulong))
            {
                throw new InvalidOperationException($"FromUInt64 requires that enum of type {typeof(TEnum).Name} uses ulong as its underlying type.");
            }

            var result = Unsafe.As<ulong, TEnum>(ref value);
            if (!result.IsDefined(allowNoFlags))
            {
                var displayValue = Attribute.GetCustomAttribute(typeof(TEnum), typeof(FlagsAttribute)) is null
                    ? value.ToString(CultureInfo.InvariantCulture)
                    : $"0b{Convert.ToString((long)value, 2).PadLeft(64, '0')}";

                throw new ArgumentException($"Value '{displayValue}' is not defined for enum of type {typeof(TEnum).Name}.");
            }

            return result;
        }
    }
}
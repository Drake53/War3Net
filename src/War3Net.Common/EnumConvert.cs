// ------------------------------------------------------------------------------
// <copyright file="EnumConvert.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Globalization;

using War3Net.Common.Extensions;

namespace War3Net.Common
{
    public static class EnumConvert<TEnum>
        where TEnum : struct, Enum
    {
        public static TEnum FromByte(byte value, bool allowNoFlags = true)
        {
            var result = (TEnum)(object)value;
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
            var result = (TEnum)(object)value;
            if (!result.IsDefined(allowNoFlags))
            {
                var displayValue = Attribute.GetCustomAttribute(typeof(TEnum), typeof(FlagsAttribute)) is null
                    ? value.ToString(CultureInfo.InvariantCulture)
                    : $"0b{Convert.ToString(value, 2).PadLeft(8, '0')}";

                throw new ArgumentException($"Value '{displayValue}' is not defined for enum of type {typeof(TEnum).Name}.");
            }

            return result;
        }

        public static TEnum FromInt16(short value, bool allowNoFlags = true)
        {
            var result = (TEnum)(object)value;
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
            var result = (TEnum)(object)value;
            if (!result.IsDefined(allowNoFlags))
            {
                var displayValue = Attribute.GetCustomAttribute(typeof(TEnum), typeof(FlagsAttribute)) is null
                    ? value.ToString(CultureInfo.InvariantCulture)
                    : $"0b{Convert.ToString(value, 2).PadLeft(16, '0')}";

                throw new ArgumentException($"Value '{displayValue}' is not defined for enum of type {typeof(TEnum).Name}.");
            }

            return result;
        }

        public static TEnum FromInt32(int value, bool allowNoFlags = true)
        {
            var result = (TEnum)(object)value;
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
            var result = (TEnum)(object)value;
            if (!result.IsDefined(allowNoFlags))
            {
                var displayValue = Attribute.GetCustomAttribute(typeof(TEnum), typeof(FlagsAttribute)) is null
                    ? value.ToString(CultureInfo.InvariantCulture)
                    : $"0b{Convert.ToString(value, 2).PadLeft(32, '0')}";

                throw new ArgumentException($"Value '{displayValue}' is not defined for enum of type {typeof(TEnum).Name}.");
            }

            return result;
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="EnumExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Common.Extensions
{
    public static class EnumExtensions
    {
        /// <param name="allowNoFlags">If <see langword="true"/>, an integral value of zero will be considered valid for <paramref name="enum"/>, assuming <typeparamref name="TEnum"/> has <see cref="FlagsAttribute"/>.</param>
        public static bool IsDefined<TEnum>(this TEnum @enum, bool allowNoFlags = true)
            where TEnum : struct, Enum
        {
            if (Enum.IsDefined(@enum))
            {
                return true;
            }

            if (Attribute.GetCustomAttribute(typeof(TEnum), typeof(FlagsAttribute)) is null)
            {
                return false;
            }

            var enumString = @enum.ToString();
            if (allowNoFlags && string.Equals(enumString, "0", StringComparison.Ordinal))
            {
                return true;
            }

            var firstChar = enumString[0];
            return !char.IsDigit(firstChar) && firstChar != '-';
        }
    }
}
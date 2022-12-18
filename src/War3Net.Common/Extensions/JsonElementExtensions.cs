// ------------------------------------------------------------------------------
// <copyright file="JsonElementExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text.Json;

namespace War3Net.Common.Extensions
{
    public static class JsonElementExtensions
    {
        public static TEnum GetByte<TEnum>(this JsonElement jsonElement)
            where TEnum : struct, Enum
        {
            return jsonElement.ValueKind == JsonValueKind.Number
                ? EnumConvert<TEnum>.FromByte(jsonElement.GetByte())
                : Enum.Parse<TEnum>(jsonElement.GetString());
        }

        public static TEnum GetInt32<TEnum>(this JsonElement jsonElement)
            where TEnum : struct, Enum
        {
            return jsonElement.ValueKind == JsonValueKind.Number
                ? EnumConvert<TEnum>.FromInt32(jsonElement.GetInt32())
                : Enum.Parse<TEnum>(jsonElement.GetString());
        }

        public static TEnum GetByte<TEnum>(this JsonElement jsonElement, ReadOnlySpan<char> propertyName)
            where TEnum : struct, Enum
        {
            return jsonElement.GetProperty(propertyName).GetByte<TEnum>();
        }

        public static TEnum GetInt32<TEnum>(this JsonElement jsonElement, ReadOnlySpan<char> propertyName)
            where TEnum : struct, Enum
        {
            return jsonElement.GetProperty(propertyName).GetInt32<TEnum>();
        }

        public static JsonElement.ArrayEnumerator EnumerateArray(this JsonElement jsonElement, ReadOnlySpan<char> propertyName) => jsonElement.GetProperty(propertyName).EnumerateArray();

        public static byte GetByte(this JsonElement jsonElement, ReadOnlySpan<char> propertyName) => jsonElement.GetProperty(propertyName).GetByte();

        public static int GetInt32(this JsonElement jsonElement, ReadOnlySpan<char> propertyName) => jsonElement.GetProperty(propertyName).GetInt32();

        public static float GetSingle(this JsonElement jsonElement, ReadOnlySpan<char> propertyName) => jsonElement.GetProperty(propertyName).GetSingle();

        public static string? GetString(this JsonElement jsonElement, ReadOnlySpan<char> propertyName) => jsonElement.GetProperty(propertyName).GetString();
    }
}
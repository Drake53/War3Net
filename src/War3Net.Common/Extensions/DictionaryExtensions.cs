// ------------------------------------------------------------------------------
// <copyright file="DictionaryExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace War3Net.Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static void SetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
            where TKey : notnull
        {
            if (dict is null)
            {
                throw new ArgumentNullException(nameof(dict));
            }

            if (!dict.TryAdd(key, value))
            {
                dict[key] = value;
            }
        }

#if NETSTANDARD2_0
        /// <summary>Tries to add the specified <paramref name="key"/> and <paramref name="value"/> to the <paramref name="dictionary"/>.</summary>
        /// <typeparam name="TKey">The type of the keys in the <paramref name="dictionary"/>.</typeparam>
        /// <typeparam name="TValue">The type of the values in the <paramref name="dictionary"/>.</typeparam>
        /// <param name="dictionary">A dictionary with keys of type <typeparamref name="TKey"/> and values of type <typeparamref name="TValue"/>.</param>
        /// <param name="key">The key of the value to add.</param>
        /// <param name="value">The value to add.</param>
        /// <returns>
        /// <para><see langword="true"/> when the <paramref name="key"/> and <paramref name="value"/> are successfully added to the <paramref name="dictionary"/>;</para>
        /// <para><see langword="false"/> when the <paramref name="dictionary"/> already contains the specified <paramref name="key"/>, in which case nothing gets added.</para>
        /// </returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="dictionary"/> is <see langword="null"/>.</exception>
        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
            where TKey : notnull
        {
            if (dictionary is null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
                return true;
            }

            return false;
        }
#endif
    }
}
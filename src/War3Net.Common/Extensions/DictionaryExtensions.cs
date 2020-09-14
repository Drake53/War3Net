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
        public static void SetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
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
    }
}
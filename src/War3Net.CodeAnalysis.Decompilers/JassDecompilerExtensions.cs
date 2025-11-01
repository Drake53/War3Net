// ------------------------------------------------------------------------------
// <copyright file="DecompilationContext.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.CodeAnalysis.Decompilers
{
    public static class JassDecompilerExtensions
    {
        public static T2 OneOf<T1, T2>(this IEnumerable<T1> tokens, params Func<IEnumerable<T1>, T2>[] mapping) where T2 : class
        {
            foreach (var map in mapping)
            {
                try
                {
                    var result = map(tokens);
                    if (result != null)
                    {
                        return result;
                    }
                }
                catch
                {
                    //swallow exceptions
                }
            }

            return default;
        }

        public static Nullable_Class<T2> OneOf_Struct<T1, T2>(this IEnumerable<T1> tokens, params Func<IEnumerable<T1>, Nullable_Class<T2>>[] mapping) where T2 : struct
        {
            return OneOf(tokens, mapping);
        }

        public static T2 SafeMapFirst<T1, T2>(this IEnumerable<T1> tokens, Func<T1, T2> mapping) where T2 : class
        {
            return tokens.Select(x => {
                try
                {
                    return mapping(x);
                }
                catch
                {
                    //swallow exceptions
                }

                return default;
            }).Where(x => x != null).FirstOrDefault();
        }

        public static Nullable_Class<T2> SafeMapFirst_Struct<T1, T2>(this IEnumerable<T1> tokens, Func<T1, Nullable_Class<T2>> mapping) where T2 : struct
        {
            return SafeMapFirst(tokens, mapping);
        }
    }
}
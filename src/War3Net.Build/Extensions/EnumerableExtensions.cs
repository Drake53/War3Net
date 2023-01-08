// ------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace War3Net.Build.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<(T Obj, int Id)> IncludeId<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Select((obj, id) => (obj, id));
        }
    }
}
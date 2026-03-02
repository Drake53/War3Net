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
// ------------------------------------------------------------------------------
// <copyright file="FilterFuncApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.Runtime.Core;

namespace War3Net.Runtime.Api.Common.Core
{
    public static class FilterFuncApi
    {
        public static FilterFunc Filter(Func<bool>? func) => new FilterFunc(func);

        public static void DestroyFilter(FilterFunc? f) => f.Dispose();
    }
}
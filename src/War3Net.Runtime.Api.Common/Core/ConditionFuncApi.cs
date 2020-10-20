// ------------------------------------------------------------------------------
// <copyright file="ConditionFuncApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.Runtime.Core;

namespace War3Net.Runtime.Api.Common.Core
{
    public static class ConditionFuncApi
    {
        public static ConditionFunc Condition(Func<bool>? func) => new ConditionFunc(func);

        public static void DestroyCondition(ConditionFunc? c) => c.Dispose();
    }
}
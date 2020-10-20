// ------------------------------------------------------------------------------
// <copyright file="ConditionFunc.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Runtime.Core
{
    public sealed class ConditionFunc : BoolExpr
    {
        public ConditionFunc(Func<bool> func)
            : base(func)
        {
        }
    }
}
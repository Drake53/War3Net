// ------------------------------------------------------------------------------
// <copyright file="BoolExpr.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Runtime.Core
{
    public abstract class BoolExpr : Agent
    {
        private readonly Func<bool> _func;

        public BoolExpr(Func<bool> func)
        {
            _func = func;
        }

        public override void Dispose()
        {
            // TODO
        }
    }
}
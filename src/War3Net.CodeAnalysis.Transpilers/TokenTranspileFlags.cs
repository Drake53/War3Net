// ------------------------------------------------------------------------------
// <copyright file="TokenTranspileFlags.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.CodeAnalysis.Transpilers
{
    [Flags]
    public enum TokenTranspileFlags
    {
        ReturnArray = 1 << 0,
        ReturnBoolFunc = 1 << 1,
    }
}
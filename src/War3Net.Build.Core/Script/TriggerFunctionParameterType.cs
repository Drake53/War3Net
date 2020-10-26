// ------------------------------------------------------------------------------
// <copyright file="TriggerFunctionParameterType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Script
{
    public enum TriggerFunctionParameterType
    {
        Preset = 0,
        Variable = 1,
        Function = 2,
        String = 3,

        Undefined = -1,
    }
}
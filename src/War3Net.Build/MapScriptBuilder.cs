// ------------------------------------------------------------------------------
// <copyright file="MapScriptBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        public MapScriptBuilder()
        {
            MaxPlayerSlots = 24;
            UseWeatherEffectVariable = true;
        }

        public int MaxPlayerSlots { get; set; }

        public bool UseWeatherEffectVariable { get; set; }
    }
}
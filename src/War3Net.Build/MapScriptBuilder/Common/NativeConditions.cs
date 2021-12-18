// ------------------------------------------------------------------------------
// <copyright file="NativeConditions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.Build.Info;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual bool SetDayNightModelsCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return true;
        }

        protected internal virtual bool SetTerrainFogExCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null
                && map.Info.MapFlags.HasFlag(MapFlags.HasTerrainFog);
        }

        protected internal virtual bool SetWaterBaseColorCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null
                && map.Info.MapFlags.HasFlag(MapFlags.HasWaterTintingColor);
        }

        protected internal virtual bool EnableGlobalWeatherEffectCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null
                && map.Info.GlobalWeather != WeatherType.None;
        }

        protected internal virtual bool NewSoundEnvironmentCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null
                && map.Info.FormatVersion > MapInfoFormatVersion.v15;
        }

        protected internal virtual bool SetAmbientSoundCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return true;
        }

        protected internal virtual bool SetMapMusicCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return true;
        }

        protected internal virtual bool InitBlizzardCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return true;
        }
    }
}
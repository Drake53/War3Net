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
        protected virtual bool SetDayNightModelsCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return true;
        }

        protected virtual bool SetTerrainFogExCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info.MapFlags.HasFlag(MapFlags.HasTerrainFog);
        }

        protected virtual bool SetWaterBaseColorCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info.MapFlags.HasFlag(MapFlags.HasWaterTintingColor);
        }

        protected virtual bool EnableGlobalWeatherEffectCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info.GlobalWeather != WeatherType.None;
        }

        protected virtual bool NewSoundEnvironmentCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info.FormatVersion > MapInfoFormatVersion.v15;
        }

        protected virtual bool SetAmbientSoundCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return true;
        }

        protected virtual bool SetMapMusicCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return true;
        }

        protected virtual bool InitBlizzardCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return true;
        }
    }
}
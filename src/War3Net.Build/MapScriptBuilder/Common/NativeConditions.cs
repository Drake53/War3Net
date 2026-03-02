namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual bool ShouldCallSetDayNightModels(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null
                && map.Info.FormatVersion >= MapInfoFormatVersion.v15;
        }

        protected internal virtual bool ShouldCallSetTerrainFogEx(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null
                && map.Info.MapFlags.HasFlag(MapFlags.HasTerrainFog);
        }

        protected internal virtual bool ShouldCallSetWaterBaseColor(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null
                && map.Info.MapFlags.HasFlag(MapFlags.HasWaterTintingColor);
        }

        protected internal virtual bool ShouldCallEnableGlobalWeatherEffect(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null
                && map.Info.GlobalWeather != WeatherType.None;
        }

        protected internal virtual bool ShouldCallNewSoundEnvironment(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null
                && map.Info.FormatVersion > MapInfoFormatVersion.v15;
        }

        protected internal virtual bool ShouldCallSetAmbientSound(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null
                && map.Info.FormatVersion >= MapInfoFormatVersion.v15;
        }

        protected internal virtual bool ShouldCallSetMapMusic(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null
                && map.Info.FormatVersion >= MapInfoFormatVersion.v15;
        }

        protected internal virtual bool ShouldCallInitBlizzard(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return true;
        }
    }
}
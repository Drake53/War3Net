using System;
using War3Net.Build.Common;
using War3Net.Build.Environment;
using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateCreateRegions(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var mapRegions = map.Regions;
            if (mapRegions is null)
            {
                throw new ArgumentException($"Function '{GeneratedFunctionName.CreateRegions}' cannot be generated without {nameof(MapRegions)}.", nameof(map));
            }

            writer.WriteFunction(GeneratedFunctionName.CreateRegions);

            if (UseWeatherEffectVariable)
            {
                writer.WriteLocal(TypeName.WeatherEffect, VariableName.WeatherEffect);
                writer.WriteLine();
            }

            foreach (var region in mapRegions.Regions)
            {
                var regionName = region.GetVariableName();

                writer.WriteSet(
                    regionName,
                    JassExpression.InvokeSpaced(
                        NativeName.Rect,
                        JassLiteral.Real(region.Left),
                        JassLiteral.Real(region.Bottom),
                        JassLiteral.Real(region.Right),
                        JassLiteral.Real(region.Top)));

                if (region.WeatherType != WeatherType.None)
                {
                    if (UseWeatherEffectVariable)
                    {
                        writer.WriteSet(
                            VariableName.WeatherEffect,
                            JassExpression.InvokeSpaced(
                                NativeName.AddWeatherEffect,
                                regionName,
                                JassLiteral.FourCC((int)region.WeatherType)));

                        writer.WriteCall(
                            NativeName.EnableWeatherEffect,
                            VariableName.WeatherEffect,
                            JassKeyword.True);
                    }
                    else
                    {
                        writer.WriteCall(
                            NativeName.EnableWeatherEffect,
                            JassExpression.Invoke(
                                NativeName.AddWeatherEffect,
                                regionName,
                                JassLiteral.FourCC((int)region.WeatherType)),
                            JassKeyword.True);
                    }
                }

                if (!string.IsNullOrEmpty(region.AmbientSound))
                {
                    writer.WriteCall(
                        NativeName.SetSoundPosition,
                        region.AmbientSound,
                        JassLiteral.Real(region.CenterX),
                        JassLiteral.Real(region.CenterY),
                        "0.0");

                    writer.WriteCall(
                        NativeName.RegisterStackedSound,
                        region.AmbientSound,
                        JassKeyword.True,
                        JassLiteral.Real(region.Width),
                        JassLiteral.Real(region.Height));
                }
            }

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateCreateRegions(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (map.Info is not null && map.Info.FormatVersion == MapInfoFormatVersion.v8)
            {
                return true;
            }

            return map.Regions is not null
                && map.Regions.Regions.Count > 0;
        }
    }
}
namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateMain(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var mapEnvironment = map.Environment;
            if (mapEnvironment is null)
            {
                throw new ArgumentException($"Function '{GeneratedFunctionName.Main}' cannot be generated without {nameof(MapEnvironment)}.", nameof(map));
            }

            var mapInfo = map.Info;
            if (mapInfo is null)
            {
                throw new ArgumentException($"Function '{GeneratedFunctionName.Main}' cannot be generated without {nameof(MapInfo)}.", nameof(map));
            }

            writer.WriteFunction(GeneratedFunctionName.Main);

            if (UseWeatherEffectVariable && ShouldCallEnableGlobalWeatherEffect(map))
            {
                writer.WriteLocal(TypeName.WeatherEffect, VariableName.WeatherEffect);
            }

            if (mapInfo.CameraBoundsComplements is null)
            {
                writer.WriteCall(
                    NativeName.SetCameraBounds,
                    JassLiteral.Real(mapInfo.CameraBounds.BottomLeft.X),
                    JassLiteral.Real(mapInfo.CameraBounds.BottomLeft.Y),
                    JassLiteral.Real(mapInfo.CameraBounds.TopRight.X),
                    JassLiteral.Real(mapInfo.CameraBounds.TopRight.Y),
                    JassLiteral.Real(mapInfo.CameraBounds.TopLeft.X),
                    JassLiteral.Real(mapInfo.CameraBounds.TopLeft.Y),
                    JassLiteral.Real(mapInfo.CameraBounds.BottomRight.X),
                    JassLiteral.Real(mapInfo.CameraBounds.BottomRight.Y));
            }
            else
            {
                var left = JassLiteral.Real(mapEnvironment.Left + (128 * mapInfo.CameraBoundsComplements.Left));
                var bottom = JassLiteral.Real(mapEnvironment.Bottom + (128 * mapInfo.CameraBoundsComplements.Bottom));
                var right = JassLiteral.Real(mapEnvironment.Right - (128 * mapInfo.CameraBoundsComplements.Right));
                var top = JassLiteral.Real(mapEnvironment.Top - (128 * mapInfo.CameraBoundsComplements.Top));

                var marginLeft = JassExpression.Invoke(NativeName.GetCameraMargin, CameraMarginName.Left);
                var marginBottom = JassExpression.Invoke(NativeName.GetCameraMargin, CameraMarginName.Bottom);
                var marginRight = JassExpression.Invoke(NativeName.GetCameraMargin, CameraMarginName.Right);
                var marginTop = JassExpression.Invoke(NativeName.GetCameraMargin, CameraMarginName.Top);

                writer.WriteCall(
                    NativeName.SetCameraBounds,
                    JassExpression.Add(left, marginLeft),
                    JassExpression.Add(bottom, marginBottom),
                    JassExpression.Subtract(right, marginRight),
                    JassExpression.Subtract(top, marginTop),
                    JassExpression.Add(left, marginLeft),
                    JassExpression.Subtract(top, marginTop),
                    JassExpression.Subtract(right, marginRight),
                    JassExpression.Add(bottom, marginBottom));
            }

            if (ShouldCallSetDayNightModels(map))
            {
                var lightEnvironment = mapInfo.LightEnvironment == Tileset.Unspecified ? mapInfo.Tileset : mapInfo.LightEnvironment;
                writer.WriteCall(
                    NativeName.SetDayNightModels,
                    JassLiteral.String(LightEnvironmentProvider.GetTerrainLightEnvironmentModel(lightEnvironment)),
                    JassLiteral.String(LightEnvironmentProvider.GetUnitLightEnvironmentModel(lightEnvironment)));
            }

            if (ShouldCallSetTerrainFogEx(map))
            {
                var precision = mapInfo.FormatVersion >= MapInfoFormatVersion.v31 ? 3 : 1;

                writer.WriteCall(
                    NativeName.SetTerrainFogEx,
                    JassLiteral.Int((int)mapInfo.FogStyle),
                    JassLiteral.Real(mapInfo.FogStartZ),
                    JassLiteral.Real(mapInfo.FogEndZ),
                    JassLiteral.Real(mapInfo.FogDensity, precision),
                    JassLiteral.Real(mapInfo.FogColor.R / 255f, precision),
                    JassLiteral.Real(mapInfo.FogColor.G / 255f, precision),
                    JassLiteral.Real(mapInfo.FogColor.B / 255f, precision));
            }

            if (ShouldCallSetWaterBaseColor(map))
            {
                writer.WriteCall(
                    NativeName.SetWaterBaseColor,
                    JassLiteral.Int(mapInfo.WaterTintingColor.R),
                    JassLiteral.Int(mapInfo.WaterTintingColor.G),
                    JassLiteral.Int(mapInfo.WaterTintingColor.B),
                    JassLiteral.Int(mapInfo.WaterTintingColor.A));
            }

            if (ShouldCallEnableGlobalWeatherEffect(map))
            {
                var weatherType = JassLiteral.FourCC((int)mapInfo.GlobalWeather);
                var weatherRegion = JassExpression.InvokeCompact(
                    NativeName.Rect,
                    JassLiteral.Real(mapEnvironment.Left),
                    JassLiteral.Real(mapEnvironment.Bottom),
                    JassLiteral.Real(mapEnvironment.Right),
                    JassLiteral.Real(mapEnvironment.Top));

                if (UseWeatherEffectVariable)
                {
                    writer.WriteSet(
                        VariableName.WeatherEffect,
                        JassExpression.InvokeSpaced(
                            NativeName.AddWeatherEffect,
                            weatherRegion,
                            weatherType));

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
                            weatherRegion,
                            weatherType),
                        JassKeyword.True);
                }
            }

            if (ShouldCallNewSoundEnvironment(map))
            {
                writer.WriteCall(
                    NativeName.NewSoundEnvironment,
                    JassLiteral.String(string.IsNullOrEmpty(mapInfo.SoundEnvironment) ? "Default" : mapInfo.SoundEnvironment));
            }

            if (ShouldCallSetAmbientSound(map))
            {
                writer.WriteCall(
                    FunctionName.SetAmbientDaySound,
                    JassLiteral.String(SoundEnvironmentProvider.GetAmbientDaySound(mapInfo.Tileset)));

                writer.WriteCall(
                    FunctionName.SetAmbientNightSound,
                    JassLiteral.String(SoundEnvironmentProvider.GetAmbientNightSound(mapInfo.Tileset)));
            }

            if (ShouldCallSetMapMusic(map))
            {
                writer.WriteCall(
                    NativeName.SetMapMusic,
                    JassLiteral.String("Music"),
                    JassKeyword.True,
                    "0");
            }

            if (ShouldGenerateInitSounds(map))
            {
                writer.WriteCall(GeneratedFunctionName.InitSounds);
            }

            if (ShouldGenerateCreateRegions(map))
            {
                writer.WriteCall(GeneratedFunctionName.CreateRegions);
            }

            if (ShouldGenerateCreateCameras(map))
            {
                writer.WriteCall(GeneratedFunctionName.CreateCameras);
            }

            if (ShouldGenerateInitUpgrades(map))
            {
                writer.WriteCall(GeneratedFunctionName.InitUpgrades);
            }

            if (ShouldGenerateInitTechTree(map))
            {
                writer.WriteCall(GeneratedFunctionName.InitTechTree);
            }

            if (ShouldGenerateCreateAllDestructables(map))
            {
                writer.WriteCall(GeneratedFunctionName.CreateAllDestructables);
            }

            if (ShouldGenerateCreateAllItems(map))
            {
                writer.WriteCall(GeneratedFunctionName.CreateAllItems);
            }

            if (ShouldGenerateInitRandomGroups(map))
            {
                writer.WriteCall(GeneratedFunctionName.InitRandomGroups);
            }

            if (ShouldGenerateCreateAllUnits(map))
            {
                writer.WriteCall(GeneratedFunctionName.CreateAllUnits);
            }
            else
            {
                if (ShouldGenerateCreateNeutralUnits(map))
                {
                    writer.WriteCall(GeneratedFunctionName.CreateNeutralUnits);
                }

                if (ShouldGenerateCreatePlayerUnits(map))
                {
                    writer.WriteCall(GeneratedFunctionName.CreatePlayerUnits);
                }
            }

            if (ShouldCallInitBlizzard(map))
            {
                writer.WriteCall(FunctionName.InitBlizzard);
            }

            if (ShouldGenerateInitGlobals(map))
            {
                writer.WriteCall(GeneratedFunctionName.InitGlobals);
            }

            if (ShouldGenerateInitCustomTriggers(map))
            {
                writer.WriteCall(GeneratedFunctionName.InitCustomTriggers);
            }

            if (ShouldGenerateRunInitializationTriggers(map))
            {
                writer.WriteCall(GeneratedFunctionName.RunInitializationTriggers);
            }

            if (UseCSharpLua)
            {
                writer.WriteCall(LuaSyntaxGenerator.kManifestFuncName);
            }

            writer.WriteLine();

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateMain(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null
                && map.Environment is not null;
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="JassScriptBuilder.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text;

using War3Net.Build.Providers;
using War3Net.CodeAnalysis.Jass.Renderer;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.Build.Script
{
    [System.Obsolete()]
    public sealed class JassScriptBuilder : ScriptBuilder
    {
        public override string Extension => ".j";

        public override void BuildMainFunction(string path, float left, float right, float top, float bottom, Tileset light, SoundEnvironment sound, params string[] initFunctions)
        {
            var fileSyntax = JassSyntaxFactory.File(GetMainFunctionSyntax(left, right, top, bottom, light, sound, initFunctions));
            RenderFunctionToFile(path, fileSyntax);
        }

        public override void BuildConfigFunction(string path, string mapName, string mapDescription, string lobbyMusic, params PlayerSlot[] playerSlots)
        {
            var fileSyntax = JassSyntaxFactory.File(GetConfigFunctionSyntax(mapName, mapDescription, lobbyMusic, playerSlots));
            RenderFunctionToFile(path, fileSyntax);
        }

        private static void RenderFunctionToFile(string path, FileSyntax fileSyntax)
        {
            using (var fileStream = FileProvider.OpenNewWrite(path))
            {
                using (var writer = new StreamWriter(fileStream, new UTF8Encoding(false, true)))
                {
                    var opt = JassRendererOptions.Default;
                    opt.Comments = false;

                    var mainAndConfigRenderer = new JassRenderer(writer);
                    mainAndConfigRenderer.Options = opt;
                    mainAndConfigRenderer.Render(fileSyntax);
                }
            }
        }

        private FunctionSyntax GetMainFunctionSyntax(float left, float right, float top, float bottom, Tileset tileset, SoundEnvironment sound, params string[] initFunctions)
        {
            var statements = new List<NewStatementSyntax>()
            {
                JassSyntaxFactory.CallStatement(
                    "SetCameraBounds",
                    JassSyntaxFactory.ArgumentList(
                        JassSyntaxFactory.VariableExpression("left"),
                        JassSyntaxFactory.VariableExpression("bottom"),
                        JassSyntaxFactory.VariableExpression("right"),
                        JassSyntaxFactory.VariableExpression("top"),
                        JassSyntaxFactory.VariableExpression("left"),
                        JassSyntaxFactory.VariableExpression("top"),
                        JassSyntaxFactory.VariableExpression("right"),
                        JassSyntaxFactory.VariableExpression("bottom"))),
                JassSyntaxFactory.CallStatement(
                    "SetDayNightModels",
                    JassSyntaxFactory.ArgumentList(
                        JassSyntaxFactory.ConstantExpression(LightEnvironmentProvider.GetTerrainLightEnvironmentModel(tileset)),
                        JassSyntaxFactory.ConstantExpression(LightEnvironmentProvider.GetUnitLightEnvironmentModel(tileset)))),
                JassSyntaxFactory.CallStatement(
                    "NewSoundEnvironment",
                    JassSyntaxFactory.ConstantExpression(SoundEnvironmentProvider.GetSoundEnvironment(sound))),
                JassSyntaxFactory.CallStatement(
                    "SetAmbientDaySound",
                    JassSyntaxFactory.ConstantExpression(SoundEnvironmentProvider.GetAmbientDaySound(tileset))),
                JassSyntaxFactory.CallStatement(
                    "SetAmbientNightSound",
                    JassSyntaxFactory.ConstantExpression(SoundEnvironmentProvider.GetAmbientNightSound(tileset))),
                JassSyntaxFactory.CallStatement(
                    "SetMapMusic",
                    JassSyntaxFactory.ArgumentList(
                        JassSyntaxFactory.ConstantExpression("Music"),
                        JassSyntaxFactory.ConstantExpression(true),
                        JassSyntaxFactory.ConstantExpression(0))),
            };

            foreach (var initFunction in initFunctions)
            {
                statements.Add(JassSyntaxFactory.CallStatement(initFunction));
            }

            return JassSyntaxFactory.Function(
                JassSyntaxFactory.FunctionDeclaration("main"),
                JassSyntaxFactory.LocalVariableList(
                    JassSyntaxFactory.VariableDefinition(
                        JassSyntaxFactory.ParseTypeName("real"),
                        "left",
                        JassSyntaxFactory.BinaryAdditionExpression(
                            JassSyntaxFactory.ConstantExpression(left),
                            JassSyntaxFactory.InvocationExpression(
                                "GetCameraMargin",
                                JassSyntaxFactory.ArgumentList(JassSyntaxFactory.VariableExpression("CAMERA_MARGIN_LEFT"))))),
                    JassSyntaxFactory.VariableDefinition(
                        JassSyntaxFactory.ParseTypeName("real"),
                        "right",
                        JassSyntaxFactory.BinarySubtractionExpression(
                            JassSyntaxFactory.ConstantExpression(right),
                            JassSyntaxFactory.InvocationExpression(
                                "GetCameraMargin",
                                JassSyntaxFactory.ArgumentList(JassSyntaxFactory.VariableExpression("CAMERA_MARGIN_RIGHT"))))),
                    JassSyntaxFactory.VariableDefinition(
                        JassSyntaxFactory.ParseTypeName("real"),
                        "top",
                        JassSyntaxFactory.BinarySubtractionExpression(
                            JassSyntaxFactory.ConstantExpression(top),
                            JassSyntaxFactory.InvocationExpression(
                                "GetCameraMargin",
                                JassSyntaxFactory.ArgumentList(JassSyntaxFactory.VariableExpression("CAMERA_MARGIN_TOP"))))),
                    JassSyntaxFactory.VariableDefinition(
                        JassSyntaxFactory.ParseTypeName("real"),
                        "bottom",
                        JassSyntaxFactory.BinaryAdditionExpression(
                            JassSyntaxFactory.ConstantExpression(bottom),
                            JassSyntaxFactory.InvocationExpression(
                                "GetCameraMargin",
                                JassSyntaxFactory.ArgumentList(JassSyntaxFactory.VariableExpression("CAMERA_MARGIN_BOTTOM")))))),
                statements.ToArray());
        }

        private FunctionSyntax GetConfigFunctionSyntax(string mapName, string mapDescription, string lobbyMusic, params PlayerSlot[] playerSlots)
        {
            var statements = new List<NewStatementSyntax>()
            {
                JassSyntaxFactory.CallStatement("SetMapName", JassSyntaxFactory.ConstantExpression(mapName)),
                JassSyntaxFactory.CallStatement("SetMapDescription", JassSyntaxFactory.ConstantExpression(mapDescription)),
                JassSyntaxFactory.CallStatement("SetPlayers", JassSyntaxFactory.ConstantExpression(playerSlots.Length)),
                JassSyntaxFactory.CallStatement("SetTeams", JassSyntaxFactory.ConstantExpression(playerSlots.Length)),
                JassSyntaxFactory.CallStatement("SetGamePlacement", JassSyntaxFactory.VariableExpression("MAP_PLACEMENT_TEAMS_TOGETHER")),
            };

            if (lobbyMusic != null)
            {
                statements.Add(JassSyntaxFactory.CallStatement("PlayMusic", JassSyntaxFactory.ConstantExpression(lobbyMusic)));
            }

            for (var i = 0; i < playerSlots.Length; i++)
            {
                var playerSlot = playerSlots[i];
                statements.Add(JassSyntaxFactory.CallStatement(
                    "DefineStartLocation",
                    JassSyntaxFactory.ArgumentList(
                        JassSyntaxFactory.ConstantExpression(i),
                        JassSyntaxFactory.ConstantExpression(playerSlot.StartLocationX),
                        JassSyntaxFactory.ConstantExpression(playerSlot.StartLocationY))));
                statements.Add(JassSyntaxFactory.CallStatement(
                    "SetPlayerStartLocation",
                    JassSyntaxFactory.ArgumentList(
                        JassSyntaxFactory.InvocationExpression("Player", JassSyntaxFactory.ConstantExpression(i)),
                        JassSyntaxFactory.ConstantExpression(i))));
                statements.Add(JassSyntaxFactory.CallStatement(
                    "ForcePlayerStartLocation",
                    JassSyntaxFactory.ArgumentList(
                        JassSyntaxFactory.InvocationExpression("Player", JassSyntaxFactory.ConstantExpression(i)),
                        JassSyntaxFactory.ConstantExpression(i))));
                statements.Add(JassSyntaxFactory.CallStatement(
                    "SetPlayerColor",
                    JassSyntaxFactory.ArgumentList(
                        JassSyntaxFactory.InvocationExpression("Player", JassSyntaxFactory.ConstantExpression(i)),
                        JassSyntaxFactory.InvocationExpression("ConvertPlayerColor", JassSyntaxFactory.ConstantExpression(i)))));
                statements.Add(JassSyntaxFactory.CallStatement(
                    "SetPlayerRacePreference",
                    JassSyntaxFactory.ArgumentList(
                        JassSyntaxFactory.InvocationExpression("Player", JassSyntaxFactory.ConstantExpression(i)),
                        JassSyntaxFactory.VariableExpression(playerSlot.RacePreference))));
                statements.Add(JassSyntaxFactory.CallStatement(
                    "SetPlayerRaceSelectable",
                    JassSyntaxFactory.ArgumentList(
                        JassSyntaxFactory.InvocationExpression("Player", JassSyntaxFactory.ConstantExpression(i)),
                        JassSyntaxFactory.ConstantExpression(playerSlot.RaceSelectable))));
                statements.Add(JassSyntaxFactory.CallStatement(
                    "SetPlayerController",
                    JassSyntaxFactory.ArgumentList(
                        JassSyntaxFactory.InvocationExpression("Player", JassSyntaxFactory.ConstantExpression(i)),
                        JassSyntaxFactory.VariableExpression(playerSlot.Controller))));
                statements.Add(JassSyntaxFactory.CallStatement(
                    "SetPlayerTeam",
                    JassSyntaxFactory.ArgumentList(
                        JassSyntaxFactory.InvocationExpression("Player", JassSyntaxFactory.ConstantExpression(i)),
                        JassSyntaxFactory.ConstantExpression(playerSlot.Team))));
                statements.Add(JassSyntaxFactory.CallStatement(
                    "SetStartLocPrioCount",
                    JassSyntaxFactory.ArgumentList(
                        JassSyntaxFactory.ConstantExpression(i),
                        JassSyntaxFactory.ConstantExpression(0))));
            }

            return JassSyntaxFactory.Function(
                JassSyntaxFactory.FunctionDeclaration("config"),
                statements.ToArray());
        }
    }
}
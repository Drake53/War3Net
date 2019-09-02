// ------------------------------------------------------------------------------
// <copyright file="LuaScriptBuilder.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text;

using CSharpLua;
using CSharpLua.LuaAst;

using War3Net.Build.Providers;

namespace War3Net.Build.Script
{
    public sealed class LuaScriptBuilder : ScriptBuilder
    {
        public override void BuildMainFunction(string path, float left, float right, float top, float bottom, LightEnvironment light, SoundEnvironment sound, params string[] initFunctions)
        {
            var fileSyntax = new LuaCompilationUnitSyntax();
            fileSyntax.AddStatement(GetMainFunctionSyntax(left, right, top, bottom, light, sound, initFunctions));
            RenderFunctionToFile(path, fileSyntax);
        }

        public override void BuildConfigFunction(string path, string mapName, string mapDescription, string lobbyMusic, params PlayerSlot[] playerSlots)
        {
            var fileSyntax = new LuaCompilationUnitSyntax();
            fileSyntax.AddStatement(GetConfigFunctionSyntax(mapName, mapDescription, lobbyMusic, playerSlots));
            RenderFunctionToFile(path, fileSyntax);
        }

        private static void RenderFunctionToFile(string path, LuaCompilationUnitSyntax luaCompilationUnit)
        {
            using (var fileStream = File.Create(path))
            {
                using (var writer = new StreamWriter(fileStream, new UTF8Encoding(false, true)))
                {
                    var renderSettings = new LuaSyntaxGenerator.SettingInfo();
                    renderSettings.Indent = 4;

                    var renderer = new LuaRenderer(renderSettings, writer);
                    renderer.RenderCompilationUnit(luaCompilationUnit);
                }
            }
        }

        private LuaVariableListDeclarationSyntax GetMainFunctionSyntax(float left, float right, float top, float bottom, LightEnvironment light, SoundEnvironment sound, params string[] initFunctions)
        {
            var localLeft = new LuaBinaryExpressionSyntax(
                new LuaFloatLiteralExpressionSyntax(left),
                LuaSyntaxNode.Tokens.Plus,
                new LuaInvocationExpressionSyntax("GetCameraMargin", "CAMERA_MARGIN_LEFT"));
            var localRight = new LuaBinaryExpressionSyntax(
                new LuaFloatLiteralExpressionSyntax(right),
                LuaSyntaxNode.Tokens.Sub,
                new LuaInvocationExpressionSyntax("GetCameraMargin", "CAMERA_MARGIN_RIGHT"));
            var localTop = new LuaBinaryExpressionSyntax(
                new LuaFloatLiteralExpressionSyntax(top),
                LuaSyntaxNode.Tokens.Sub,
                new LuaInvocationExpressionSyntax("GetCameraMargin", "CAMERA_MARGIN_TOP"));
            var localBottom = new LuaBinaryExpressionSyntax(
                new LuaFloatLiteralExpressionSyntax(bottom),
                LuaSyntaxNode.Tokens.Plus,
                new LuaInvocationExpressionSyntax("GetCameraMargin", "CAMERA_MARGIN_BOTTOM"));
            var locals = new[]
            {
                localLeft,
                localRight,
                localTop,
                localBottom,
            };

            var statements = new List<LuaStatementSyntax>
            {
                new LuaLocalDeclarationStatementSyntax(new LuaLocalVariablesStatementSyntax(
                    new[]
                    {
                        new LuaSymbolNameSyntax(new LuaIdentifierLiteralExpressionSyntax("left")),
                        new LuaSymbolNameSyntax(new LuaIdentifierLiteralExpressionSyntax("right")),
                        new LuaSymbolNameSyntax(new LuaIdentifierLiteralExpressionSyntax("top")),
                        new LuaSymbolNameSyntax(new LuaIdentifierLiteralExpressionSyntax("bottom")),
                    },
                    locals)),
                new LuaExpressionStatementSyntax(
                    new LuaInvocationExpressionSyntax(
                        "SetCameraBounds",
                        "left",
                        "bottom",
                        "right",
                        "top",
                        "left",
                        "top",
                        "right",
                        "bottom")),
                new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                    "SetDayNightModels",
                    new LuaStringLiteralExpressionSyntax(LightEnvironmentProvider.GetTerrainLightEnvironmentModel(light)),
                    new LuaStringLiteralExpressionSyntax(LightEnvironmentProvider.GetUnitLightEnvironmentModel(light)))),
                new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax("NewSoundEnvironment", new LuaStringLiteralExpressionSyntax("Default"))),
                new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax("SetAmbientDaySound", new LuaStringLiteralExpressionSyntax(SoundEnvironmentProvider.GetAmbientDaySound(sound)))),
                new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax("SetAmbientNightSound", new LuaStringLiteralExpressionSyntax(SoundEnvironmentProvider.GetAmbientNightSound(sound)))),
                new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                    "SetMapMusic",
                    new LuaStringLiteralExpressionSyntax("Music"),
                    LuaIdentifierLiteralExpressionSyntax.False,
                    LuaNumberLiteralExpressionSyntax.Zero)),
            };

            foreach (var initFunction in initFunctions)
            {
                statements.Add(new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(initFunction)));
            }

            var functionSyntax = new LuaFunctionExpressionSyntax();
            functionSyntax.AddStatements(statements);

            var mainFunctionDeclarator = new LuaVariableDeclaratorSyntax("main", functionSyntax);
            mainFunctionDeclarator.IsLocalDeclaration = false;

            var globalFunctionSyntax = new LuaVariableListDeclarationSyntax();
            globalFunctionSyntax.Variables.Add(mainFunctionDeclarator);

            return globalFunctionSyntax;
        }

        private LuaVariableListDeclarationSyntax GetConfigFunctionSyntax(string mapName, string mapDescription, string lobbyMusic, params PlayerSlot[] playerSlots)
        {
            var statements = new List<LuaStatementSyntax>
            {
                new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax("SetMapName", new LuaStringLiteralExpressionSyntax(mapName))),
                new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax("SetMapDescription", new LuaStringLiteralExpressionSyntax(mapDescription))),
                new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax("SetPlayers", playerSlots.Length)),
                new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax("SetTeams", playerSlots.Length)),
                new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax("SetGamePlacement", "MAP_PLACEMENT_TEAMS_TOGETHER")),
            };

            if (lobbyMusic != null)
            {
                statements.Add(new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax("PlayMusic", lobbyMusic)));
            }

            for (var i = 0; i < playerSlots.Length; i++)
            {
                var playerSlot = playerSlots[i];
                statements.Add(new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                    "DefineStartLocation",
                    i,
                    playerSlot.StartLocationX,
                    playerSlot.StartLocationY)));
                statements.Add(new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                    "SetPlayerStartLocation",
                    new LuaInvocationExpressionSyntax("Player", i),
                    i)));
                statements.Add(new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                    "ForcePlayerStartLocation",
                    new LuaInvocationExpressionSyntax("Player", i),
                    i)));
                statements.Add(new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                    "SetPlayerColor",
                    new LuaInvocationExpressionSyntax("Player", i),
                    new LuaInvocationExpressionSyntax("ConvertPlayerColor", i))));
                statements.Add(new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                    "SetPlayerRacePreference",
                    new LuaInvocationExpressionSyntax("Player", i),
                    new LuaIdentifierLiteralExpressionSyntax(playerSlot.RacePreference))));
                statements.Add(new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                    "SetPlayerRaceSelectable",
                    new LuaInvocationExpressionSyntax("Player", i),
                    LuaIdentifierLiteralExpressionSyntax.False)));
                statements.Add(new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                    "SetPlayerController",
                    new LuaInvocationExpressionSyntax("Player", i),
                    new LuaIdentifierLiteralExpressionSyntax(playerSlot.Controller))));
                statements.Add(new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                    "SetPlayerTeam",
                    new LuaInvocationExpressionSyntax("Player", i),
                    playerSlot.Team)));
                statements.Add(new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                    "SetStartLocPrioCount",
                    i,
                    0)));
            }

            var functionSyntax = new LuaFunctionExpressionSyntax();
            functionSyntax.AddStatements(statements);

            var configFunctionDeclarator = new LuaVariableDeclaratorSyntax("config", functionSyntax);
            configFunctionDeclarator.IsLocalDeclaration = false;

            var globalFunctionSyntax = new LuaVariableListDeclarationSyntax();
            globalFunctionSyntax.Variables.Add(configFunctionDeclarator);

            return globalFunctionSyntax;
        }
    }
}
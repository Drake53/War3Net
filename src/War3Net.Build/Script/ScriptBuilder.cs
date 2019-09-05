// ------------------------------------------------------------------------------
// <copyright file="ScriptBuilder.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Script
{
    public abstract class ScriptBuilder
    {
        public abstract string Extension { get; }

        public void BuildMainFunction(string path, ScriptBuilderOptions options)
        {
            BuildMainFunction(
                path,
                options.CameraBoundsLeft,
                options.CameraBoundsRight,
                options.CameraBoundsTop,
                options.CameraBoundsBottom,
                options.Tileset,
                options.SoundEnvironment,
                options.InitializationFunctions.ToArray());
        }

        public void BuildConfigFunction(string path, ScriptBuilderOptions options)
        {
            BuildConfigFunction(
                path,
                options.MapName,
                options.MapDescription,
                options.LobbyMusic,
                options.PlayerSlots.ToArray());
        }

        public abstract void BuildMainFunction(string path, float left, float right, float top, float bottom, Tileset light, SoundEnvironment sound, params string[] initFunctions);

        public abstract void BuildConfigFunction(string path, string mapName, string mapDescription, string lobbyMusic, params PlayerSlot[] playerSlots);
    }
}
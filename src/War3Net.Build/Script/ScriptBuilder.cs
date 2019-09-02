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
        public abstract void BuildMainFunction(string path, float left, float right, float top, float bottom, LightEnvironment light, SoundEnvironment sound, params string[] initFunctions);

        public abstract void BuildConfigFunction(string path, string mapName, string mapDescription, string lobbyMusic, params PlayerSlot[] playerSlots);
    }
}
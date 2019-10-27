// ------------------------------------------------------------------------------
// <copyright file="ScriptBuilderOptions.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.Build.Common;

namespace War3Net.Build.Script
{
    [System.Obsolete]
    public sealed class ScriptBuilderOptions
    {
        public float CameraBoundsLeft { get; set; }

        public float CameraBoundsRight { get; set; }

        public float CameraBoundsTop { get; set; }

        public float CameraBoundsBottom { get; set; }

        public Tileset Tileset { get; set; }

        public SoundEnvironment SoundEnvironment { get; set; }

        public string MapName { get; set; }

        public string MapDescription { get; set; }

        public string LobbyMusic { get; set; }

        public List<string> InitializationFunctions { get; private set; }

        public List<PlayerSlot> PlayerSlots { get; private set; }

        public ScriptBuilderOptions()
        {
            CameraBoundsLeft = -999999f;
            CameraBoundsRight = 999999f;
            CameraBoundsTop = 999999f;
            CameraBoundsBottom = -999999f;

            Tileset = Tileset.LordaeronSummer;
            SoundEnvironment = SoundEnvironment.Mountains;

            MapName = "Just Another Warcraft III Map";
            MapDescription = "Generated with War3Net.Build";

            LobbyMusic = null;

            InitializationFunctions = new List<string>();
            PlayerSlots = new List<PlayerSlot>();
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="GameConfiguration.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Configuration
{
    public sealed partial class GameConfiguration
    {
        public const string FileExtension = ".wgc";

        /// <summary>
        /// Initializes a new instance of the <see cref="GameConfiguration"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public GameConfiguration(GameConfigurationFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        public GameConfigurationFormatVersion FormatVersion { get; set; }

        public GameConfigurationFlags Flags { get; set; }

        public uint GameSpeedMultiplier { get; set; }

        public string MapPath { get; set; }

        public List<GameConfigurationPlayerInfo> PlayerInfo { get; init; } = new();

        public override string ToString() => MapPath;
    }
}
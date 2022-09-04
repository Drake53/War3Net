// ------------------------------------------------------------------------------
// <copyright file="GameConfiguration.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Configuration
{
    public sealed class GameConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameConfiguration"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public GameConfiguration(GameConfigurationFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        internal GameConfiguration(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        public GameConfigurationFormatVersion FormatVersion { get; set; }

        public GameConfigurationFlags Flags { get; set; }

        public uint GameSpeedMultiplier { get; set; }

        public string MapPath { get; set; }

        public List<GameConfigurationPlayerInfo> PlayerInfo { get; init; } = new();

        public override string ToString() => MapPath;

        internal void ReadFrom(BinaryReader reader)
        {
            FormatVersion = reader.ReadInt32<GameConfigurationFormatVersion>();
            Flags = reader.ReadInt32<GameConfigurationFlags>();
            GameSpeedMultiplier = reader.ReadUInt32();
            MapPath = reader.ReadChars();

            nint playerInfoCount = reader.ReadInt32();
            for (nint i = 0; i < playerInfoCount; i++)
            {
                PlayerInfo.Add(reader.ReadGameConfigurationPlayerInfo(FormatVersion));
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write((int)FormatVersion);
            writer.Write((int)Flags);
            writer.Write(GameSpeedMultiplier);
            writer.WriteString(MapPath);

            writer.Write(PlayerInfo.Count);
            foreach (var playerInfo in PlayerInfo)
            {
                writer.Write(playerInfo, FormatVersion);
            }
        }
    }
}
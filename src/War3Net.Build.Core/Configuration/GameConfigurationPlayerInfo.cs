// ------------------------------------------------------------------------------
// <copyright file="GameConfigurationPlayerInfo.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Common;
using War3Net.Common.Extensions;

namespace War3Net.Build.Configuration
{
    public sealed class GameConfigurationPlayerInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameConfigurationPlayerInfo"/> class.
        /// </summary>
        public GameConfigurationPlayerInfo()
        {
        }

        internal GameConfigurationPlayerInfo(BinaryReader reader, GameConfigurationFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        public int PlayerSlotId { get; set; }

        public int ForceId { get; set; }

        public GameConfigurationPlayerRace PlayerRace { get; set; }

        public KnownPlayerColor PlayerColor { get; set; }

        public int Handicap { get; set; }

        public GameConfigurationPlayerInfoFlags PlayerInfoFlags { get; set; }

        public AIDifficulty AIDifficulty { get; set; }

        public string CustomAIFilePath { get; set; }

        public override string ToString() => $"Player slot #{PlayerSlotId + 1}";

        internal void ReadFrom(BinaryReader reader, GameConfigurationFormatVersion formatVersion)
        {
            PlayerSlotId = reader.ReadInt32();
            ForceId = reader.ReadInt32();
            PlayerRace = reader.ReadInt32<GameConfigurationPlayerRace>();
            PlayerColor = reader.ReadInt32<KnownPlayerColor>();
            Handicap = reader.ReadInt32();
            PlayerInfoFlags = reader.ReadInt32<GameConfigurationPlayerInfoFlags>();
            AIDifficulty = reader.ReadInt32<AIDifficulty>();
            CustomAIFilePath = reader.ReadChars();
        }

        internal void WriteTo(BinaryWriter writer, GameConfigurationFormatVersion formatVersion)
        {
            writer.Write(PlayerSlotId);
            writer.Write(ForceId);
            writer.Write((int)PlayerRace);
            writer.Write((int)PlayerColor);
            writer.Write(Handicap);
            writer.Write((int)PlayerInfoFlags);
            writer.Write((int)AIDifficulty);
            writer.WriteString(CustomAIFilePath);
        }
    }
}
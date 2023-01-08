// ------------------------------------------------------------------------------
// <copyright file="GameConfigurationPlayerInfo.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Common;

namespace War3Net.Build.Configuration
{
    public sealed partial class GameConfigurationPlayerInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameConfigurationPlayerInfo"/> class.
        /// </summary>
        public GameConfigurationPlayerInfo()
        {
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
    }
}
// ------------------------------------------------------------------------------
// <copyright file="GamecacheSyncEvent.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Replay.Action
{
    public abstract class GamecacheSyncEvent : ActionBlock
    {
        private readonly string _campaignFileName;
        private readonly string _missionKey;
        private readonly string _key;

        public GamecacheSyncEvent(Stream data)
        {
            using (var reader = new BinaryReader(data, new UTF8Encoding(false, true), true))
            {
                _campaignFileName = reader.ReadChars();
                _missionKey = reader.ReadChars();
                _key = reader.ReadChars();
            }
        }

        /// <summary>
        /// Gets the string that was used to initialize the gamecache.
        /// </summary>
        public string CampaignFileName => _campaignFileName;

        // For syncInt MMD, this is either val:# or chk:#, where # is the 0-indexed message number.
        public string MissionKey => _missionKey;

        // For syncInt MMD, this is the message contents for messages (val), or the index for checksum messages (which should match the number in missionkey).
        public string Key => _key;
    }
}
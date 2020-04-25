// ------------------------------------------------------------------------------
// <copyright file="SlotRecord.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

namespace War3Net.Replay
{
    public sealed class SlotRecord
    {
        public static SlotRecord Parse(Stream stream, bool leaveOpen = false)
        {
            var record = new SlotRecord();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                var playerId = reader.ReadByte();
                var mapDownloadPercent = reader.ReadByte(); // expected to be 0x64 in custom games, and 0xff in ladder
                var slotStatus = reader.ReadByte(); // 0 = empty, 1 = closed, 2 = occupied
                var userFlag = reader.ReadByte(); // 0 = human, 1 = computer
                var teamNumber = reader.ReadByte(); // 12 for observer/referee (should be 24 in newer patches?)
                var playerColor = reader.ReadByte();
                var playerRace = reader.ReadByte(); // 0x01 human, 2,4,8=orc,NE,ud, 0x20 = random, 0x40 = selectable/fixed
                var aiStrength = reader.ReadByte(); // 0, 1, 2 easy normal hard, only exists in v1.03 or higher
                var handicap = reader.ReadByte(); // 0x64 for 100%, only exists in v1.07 or higher
            }

            return record;
        }
    }
}
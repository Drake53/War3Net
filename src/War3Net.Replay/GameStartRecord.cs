// ------------------------------------------------------------------------------
// <copyright file="GameStartRecord.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

namespace War3Net.Replay
{
    public sealed class GameStartRecord
    {
        public static GameStartRecord Parse(Stream stream, bool leaveOpen = false)
        {
            var record = new GameStartRecord();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                if (reader.ReadByte() != 0x19)
                {
                    throw new InvalidDataException();
                }

                var dataBytes = reader.ReadUInt16();
                var slotRecords = reader.ReadByte();

                for (var slot = 0; slot < slotRecords; slot++)
                {
                    SlotRecord.Parse(stream, true);
                }

                var seed = reader.ReadUInt32();
                var selectMode = reader.ReadByte();
                var startPositionCount = reader.ReadByte();
            }

            return record;
        }
    }
}
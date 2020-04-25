// ------------------------------------------------------------------------------
// <copyright file="KeyboardEvent.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

namespace War3Net.Replay.Action
{
    public sealed class KeyboardEventBlock : ActionBlock
    {
        public KeyboardEventBlock(Stream data)
        {
            using var reader = new BinaryReader(data, new UTF8Encoding(false, true), true);

            var unk1 = reader.ReadUInt32();
            var unk2 = reader.ReadUInt32();
            if (unk1 != unk2)
            {
                throw new InvalidDataException();
            }

            var unk3 = reader.ReadUInt32();

            var keyType = reader.ReadUInt32();
            var metaKeyType = reader.ReadUInt32();
        }
    }
}
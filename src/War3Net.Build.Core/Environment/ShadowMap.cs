// ------------------------------------------------------------------------------
// <copyright file="ShadowMap.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace War3Net.Build.Environment
{
    public class ShadowMap
    {
        public const string FileName = "war3map.shd";

        // True = 0xff, false = 0x00
        private readonly List<byte> _cells;

        public ShadowMap()
        {
            _cells = new List<byte>();
        }

        public static ShadowMap Parse(Stream stream, bool leaveOpen = false)
        {
            var shadowMap = new ShadowMap();
            while (true)
            {
                var read = stream.ReadByte();
                if (read == -1)
                {
                    break;
                }

                shadowMap._cells.Add((byte)read);
            }

            if (!leaveOpen)
            {
                stream.Dispose();
            }

            return shadowMap;
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                foreach (var cell in _cells)
                {
                    writer.Write(cell);
                }
            }
        }
    }
}
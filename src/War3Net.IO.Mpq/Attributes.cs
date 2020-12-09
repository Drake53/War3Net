// ------------------------------------------------------------------------------
// <copyright file="Attributes.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

using War3Net.Common.Extensions;

namespace War3Net.IO.Mpq
{
    public sealed class Attributes
    {
        public const string FileName = "(attributes)";

        internal Attributes(MpqArchiveCreateOptions mpqArchiveCreateOptions)
        {
            Unk = 100;
            Flags = mpqArchiveCreateOptions.AttributesFlags;
        }

        internal Attributes(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        public int Unk { get; set; }

        public AttributesFlags Flags { get; set; }

        public List<int> Crc32s { get; init; } = new();

        public List<DateTime> DateTimes { get; init; } = new();

        internal void ReadFrom(BinaryReader reader)
        {
            Unk = reader.ReadInt32();
            if (Unk != 100)
            {
                throw new InvalidDataException();
            }

            Flags = reader.ReadInt32<AttributesFlags>();

            var bytesPerMpqFile = 0;

            var hasCrc32 = Flags.HasFlag(AttributesFlags.Crc32);
            if (hasCrc32)
            {
                bytesPerMpqFile += 4;
            }

            var hasDateTime = Flags.HasFlag(AttributesFlags.DateTime);
            if (hasDateTime)
            {
                bytesPerMpqFile += 8;
            }

            var remainingBytes = reader.BaseStream.Length - reader.BaseStream.Position;
            if (bytesPerMpqFile > 0)
            {
                if ((remainingBytes % bytesPerMpqFile) != 0)
                {
                    throw new InvalidDataException();
                }

                nint fileCount = (int)remainingBytes / bytesPerMpqFile;

                if (hasCrc32)
                {
                    for (nint i = 0; i < fileCount; i++)
                    {
                        Crc32s.Add(reader.ReadInt32());
                    }
                }

                if (hasDateTime)
                {
                    for (nint i = 0; i < fileCount; i++)
                    {
                        DateTimes.Add(new DateTime(reader.ReadInt64(), DateTimeKind.Unspecified));
                    }
                }
            }
            else if (remainingBytes > 0)
            {
                throw new InvalidDataException();
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write(Unk);
            writer.Write((int)Flags);

            if (Flags.HasFlag(AttributesFlags.Crc32))
            {
                foreach (var crc32 in Crc32s)
                {
                    writer.Write(crc32);
                }
            }

            if (Flags.HasFlag(AttributesFlags.DateTime))
            {
                foreach (var dateTime in DateTimes)
                {
                    writer.Write(dateTime.Ticks);
                }
            }
        }
    }
}
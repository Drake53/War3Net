// ------------------------------------------------------------------------------
// <copyright file="Attributes.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace War3Net.IO.Mpq
{
    public static class Attributes
    {
        public const string Key = "(attributes)";

        public static bool VerifyAttributes(this MpqArchive archive)
        {
            if (archive.TryAddFilename(Key))
            {
                using var attributesStream = archive.OpenFile(Key);
                using var reader = new BinaryReader(attributesStream);

                var unk = reader.ReadUInt32();
                if (unk != 100)
                {
                    throw new InvalidDataException();
                }

                var dataFlags = reader.ReadUInt32();
                if (dataFlags <= 0 || dataFlags >= 4)
                {
                    throw new InvalidDataException();
                }

                // CRC32
                if ((dataFlags & 0x1) != 0)
                {
                    foreach (var mpqEntry in archive)
                    {
                        using var mpqEntryStream = archive.OpenFile(mpqEntry);

                        var expected = mpqEntry.Filename == Key ? 0 : new Ionic.Crc.CRC32().GetCrc32(mpqEntryStream);
                        var actual = reader.ReadInt32();
                        if (actual != expected)
                        {
                            return false;
                        }
                    }
                }

                // DateTime
                if ((dataFlags & 0x2) != 0)
                {
                    foreach (var mpqEntry in archive)
                    {
                        var dateTime = new DateTime(reader.ReadInt64(), DateTimeKind.Unspecified);
                    }
                }

                if (attributesStream.Position < attributesStream.Length)
                {
                    throw new InvalidDataException();
                }
            }

            return true;
        }
    }
}
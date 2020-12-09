// ------------------------------------------------------------------------------
// <copyright file="MpqArchiveExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

namespace War3Net.IO.Mpq.Extensions
{
    public static class MpqArchiveExtensions
    {
        /// <exception cref="ArgumentException">Thrown when the <see cref="MpqArchive"/> does not contain an <see cref="Attributes"/> file.</exception>
        public static bool VerifyAttributes(this MpqArchive archive)
        {
            if (!archive.TryAddFilename(Attributes.FileName))
            {
                throw new ArgumentException($"The archive must contain an {Attributes.FileName} file.", nameof(archive));
            }

            using var attributesStream = archive.OpenFile(Attributes.FileName);
            using var reader = new BinaryReader(attributesStream);

            var attributes = reader.ReadAttributes();
            var hasCrc32 = attributes.Flags.HasFlag(AttributesFlags.Crc32);
            var hasDateTime = attributes.Flags.HasFlag(AttributesFlags.DateTime);

            var count = 0;
            foreach (var mpqEntry in archive)
            {
                if (hasCrc32)
                {
                    using var mpqEntryStream = archive.OpenFile(mpqEntry);

                    var crc32 = mpqEntry.Filename == Attributes.FileName ? 0 : new Ionic.Crc.CRC32().GetCrc32(mpqEntryStream);
                    if (crc32 != attributes.Crc32s[count])
                    {
                        return false;
                    }
                }

                count++;
            }

            return (!hasCrc32 || attributes.Crc32s.Count == count)
                && (!hasDateTime || attributes.DateTimes.Count == count);
        }
    }
}
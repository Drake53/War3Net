// ------------------------------------------------------------------------------
// <copyright file="MpqArchiveExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Security.Cryptography;

using Ionic.Crc;

namespace War3Net.IO.Mpq.Extensions
{
    public static class MpqArchiveExtensions
    {
        private static readonly int _signatureCrc32 = new CRC32().GetCrc32(new MemoryStream(new byte[64 + (2 * sizeof(int))]));

        /// <exception cref="ArgumentException">Thrown when the <see cref="MpqArchive"/> does not contain an <see cref="Attributes"/> file.</exception>
        public static bool VerifyAttributes(this MpqArchive archive)
        {
            if (!archive.TryOpenFile(Attributes.FileName, out var attributesStream))
            {
                throw new ArgumentException($"The archive must contain an {Attributes.FileName} file.", nameof(archive));
            }

            archive.AddFileName(Signature.FileName);

            using var reader = new BinaryReader(attributesStream);

            var attributes = reader.ReadAttributes();
            var hasCrc32 = attributes.Flags.HasFlag(AttributesFlags.Crc32);
            var hasDateTime = attributes.Flags.HasFlag(AttributesFlags.DateTime);
            var hasUnk0x04 = attributes.Flags.HasFlag(AttributesFlags.Unk0x04);

            var count = 0;
            foreach (var mpqEntry in archive)
            {
                if (hasCrc32)
                {
                    var actualCrc32 = attributes.Crc32s[count];
                    if (string.Equals(mpqEntry.FileName, Attributes.FileName, StringComparison.OrdinalIgnoreCase))
                    {
                        if (actualCrc32 != 0)
                        {
                            return false;
                        }
                    }
                    else if (string.Equals(mpqEntry.FileName, Signature.FileName, StringComparison.OrdinalIgnoreCase))
                    {
                        if (actualCrc32 != _signatureCrc32)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        using var mpqEntryStream = archive.OpenFile(mpqEntry);
                        if (actualCrc32 != new CRC32().GetCrc32(mpqEntryStream))
                        {
                            return false;
                        }
                    }
                }

                if (hasUnk0x04)
                {
                    if (attributes.Unk0x04s[count].Length != 16)
                    {
                        return false;
                    }

                    for (var i = 0; i < 16; i++)
                    {
                        if (attributes.Unk0x04s[count][i] != 0)
                        {
                            return false;
                        }
                    }
                }

                count++;
            }

            return (!hasCrc32 || attributes.Crc32s.Count == count)
                && (!hasDateTime || attributes.DateTimes.Count == count)
                && (!hasUnk0x04 || attributes.Unk0x04s.Count == count);
        }

        public static bool VerifySignature(this MpqArchive archive, ReadOnlySpan<char> publicKey)
        {
            throw new NotImplementedException();

            using var signatureStream = archive.OpenFile(Signature.FileName);
            using var reader = new BinaryReader(signatureStream);

            var signature = reader.ReadSignature();

            using var memoryStream = new MemoryStream();
            archive.BaseStream.Position = 0;
            archive.BaseStream.CopyTo(memoryStream);

            var streamBytes = memoryStream.ToArray();
            var archiveBytes = new byte[archive.Header.ArchiveSize];
            var overwrite = new byte[signatureStream.Length];

            Array.Copy(overwrite, 0, streamBytes, signatureStream.FilePosition, overwrite.Length);
            Array.Copy(streamBytes, archive.Header.DataPosition, archiveBytes, 0, archiveBytes.Length);

            using var rsa = RSA.Create();
            rsa.ImportFromPem(publicKey);

            return rsa.VerifyData(archiveBytes, signature.SignatureBytes, HashAlgorithmName.MD5, RSASignaturePadding.Pkcs1);
        }
    }
}
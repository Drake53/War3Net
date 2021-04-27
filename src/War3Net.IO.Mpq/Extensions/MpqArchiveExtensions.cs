// ------------------------------------------------------------------------------
// <copyright file="MpqArchiveExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

using Ionic.Crc;

namespace War3Net.IO.Mpq.Extensions
{
    public static class MpqArchiveExtensions
    {
        private static readonly int _signatureCrc32 = new CRC32().GetCrc32(new MemoryStream(new byte[72]));
        private static readonly string[] _defaultPublicKeys = GetKnownBlizzardPublicKeys().ToArray();

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
                    var unk0x04 = attributes.Unk0x04s[count];
                    if (unk0x04.Length != 16)
                    {
                        return false;
                    }

                    for (var i = 0; i < 16; i++)
                    {
                        if (unk0x04[i] != 0)
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

        public static bool VerifySignature(this MpqArchive archive)
        {
            return archive.VerifySignature(_defaultPublicKeys);
        }

        public static bool VerifySignature(this MpqArchive archive, ReadOnlySpan<char> publicKey)
        {
            if (!archive.TryOpenFile(Signature.FileName, out var signatureStream))
            {
                throw new ArgumentException($"The archive must contain a {Signature.FileName} file.", nameof(archive));
            }

            using var reader = new BinaryReader(signatureStream);

            var signature = reader.ReadSignature();

            var archiveBytes = new byte[archive.Header.ArchiveSize];
            archive.BaseStream.Position = archive.HeaderOffset;
            archive.BaseStream.Read(archiveBytes);

            Array.Fill(archiveBytes, (byte)0x00, (int)(signatureStream.FilePosition - archive.HeaderOffset) + 8, 64);

            using var rsa = RSA.Create();
            return rsa.VerifyMpqSignature(archiveBytes, signature.SignatureBytes, publicKey);
        }

        public static bool VerifySignature(this MpqArchive archive, IEnumerable<string> publicKeys)
        {
            if (!publicKeys.Any())
            {
                throw new ArgumentException("Must provide at least one public key.", nameof(publicKeys));
            }

            if (!archive.TryOpenFile(Signature.FileName, out var signatureStream))
            {
                throw new ArgumentException($"The archive must contain a {Signature.FileName} file.", nameof(archive));
            }

            using var reader = new BinaryReader(signatureStream);

            var signature = reader.ReadSignature();

            var archiveBytes = new byte[archive.Header.ArchiveSize];
            archive.BaseStream.Position = archive.HeaderOffset;
            archive.BaseStream.Read(archiveBytes);

            Array.Fill(archiveBytes, (byte)0x00, (int)(signatureStream.FilePosition - archive.HeaderOffset) + 8, 64);

            using var rsa = RSA.Create();
            return publicKeys.Any(publicKey => rsa.VerifyMpqSignature(archiveBytes, signature.SignatureBytes, publicKey));
        }

        private static IEnumerable<string> GetKnownBlizzardPublicKeys()
        {
            yield return Signature.BlizzardWeakPublicKey;
            yield return Signature.BlizzardStrongPublicKey;
            yield return Signature.Warcraft3MapPublicKey;
            yield return Signature.WowPatchPublicKey;
            yield return Signature.WowSurveyPublicKey;
            yield return Signature.Starcraft2MapPublicKey;
        }
    }
}
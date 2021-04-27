// ------------------------------------------------------------------------------
// <copyright file="Signature.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.IO.Mpq
{
    public sealed class Signature
    {
        public const string FileName = "(signature)";

        internal Signature()
        {
            Unk1 = 0;
            Unk2 = 0;
            SignatureBytes = new byte[64];
        }

        internal Signature(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        public int Unk1 { get; set; }

        public int Unk2 { get; set; }

        public byte[] SignatureBytes { get; set; }

        internal void ReadFrom(BinaryReader reader)
        {
            Unk1 = reader.ReadInt32();
            if (Unk1 != 0)
            {
                throw new InvalidDataException();
            }

            Unk2 = reader.ReadInt32();
            if (Unk2 != 0)
            {
                throw new InvalidDataException();
            }

            SignatureBytes = reader.ReadBytes(64);

            var remainingBytes = reader.BaseStream.Length - reader.BaseStream.Position;
            if (remainingBytes > 0)
            {
                throw new InvalidDataException();
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write(Unk1);
            writer.Write(Unk2);
            writer.Write(SignatureBytes);
        }
    }
}
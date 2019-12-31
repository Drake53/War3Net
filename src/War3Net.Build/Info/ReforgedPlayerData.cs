// ------------------------------------------------------------------------------
// <copyright file="ReforgedPlayerData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

namespace War3Net.Build.Info
{
    public sealed class ReforgedPlayerData : PlayerData
    {
        private int _unk0;
        private int _unk1;

        internal ReforgedPlayerData()
        {
        }

        public int Unk0
        {
            get => _unk0;
            set => _unk0 = value;
        }

        public int Unk1
        {
            get => _unk1;
            set => _unk1 = value;
        }

        public static ReforgedPlayerData Parse(Stream stream, bool leaveOpen = false)
        {
            var data = new ReforgedPlayerData();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                ReadFrom(reader, data);
            }

            return data;
        }

        internal static void ReadFrom(BinaryReader reader, ReforgedPlayerData data)
        {
            PlayerData.ReadFrom(reader, data);
            data._unk0 = reader.ReadInt32();
            data._unk1 = reader.ReadInt32();
        }

        public override void WriteTo(BinaryWriter writer)
        {
            base.WriteTo(writer);
            writer.Write(_unk0);
            writer.Write(_unk1);
        }
    }
}
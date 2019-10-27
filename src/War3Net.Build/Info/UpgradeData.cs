// ------------------------------------------------------------------------------
// <copyright file="UpgradeData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

namespace War3Net.Build.Info
{
    public sealed class UpgradeData
    {
        private int _playersMask;
        private char[] _upgradeId;
        private int _upgradeLevel; // 0-indexed
        private UpgradeAvailability _availability;

        public static UpgradeData Parse(Stream stream, bool leaveOpen = false)
        {
            var data = new UpgradeData();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                data._playersMask = reader.ReadInt32();
                data._upgradeId = reader.ReadChars(4);
                data._upgradeLevel = reader.ReadInt32();
                data._availability = (UpgradeAvailability)reader.ReadInt32();
            }

            return data;
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(_playersMask);
            writer.Write(_upgradeId);
            writer.Write(_upgradeLevel);
            writer.Write((int)_availability);
        }

        public bool AppliesToPlayer(int playerIndex)
        {
            return (_playersMask & (1 << playerIndex)) != 0;
        }
    }
}
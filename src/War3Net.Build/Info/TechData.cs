// ------------------------------------------------------------------------------
// <copyright file="TechData.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

namespace War3Net.Build.Info
{
    public sealed class TechData
    {
        private int _playersMask;
        private char[] _techId; // can be item, unit, or ability

        public static TechData Parse(Stream stream, bool leaveOpen = false)
        {
            var data = new TechData();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                data._playersMask = reader.ReadInt32();
                data._techId = reader.ReadChars(4);
            }

            return data;
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(_playersMask);
            writer.Write(_techId);
        }

        public bool AppliesToPlayer(int playerIndex)
        {
            return (_playersMask & (1 << playerIndex)) != 0;
        }
    }
}
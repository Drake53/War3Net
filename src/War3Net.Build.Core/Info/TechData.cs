// ------------------------------------------------------------------------------
// <copyright file="TechData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;

namespace War3Net.Build.Info
{
    public sealed class TechData
    {
        private int _playersMask;
        private char[] _techId;

        private TechData()
        {
        }

        public TechData(string id, int playersMask = -1)
            : this()
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (id.Length != 4)
            {
                throw new ArgumentException("Upgrade id must be 4 characters long.", nameof(id));
            }

            _playersMask = playersMask;
            _techId = new[] { id[0], id[1], id[2], id[3] };
        }

        public string Id => new string(_techId);

        /// <summary>
        /// Gets a value indicating whether the tech is an ability, or it is an item or unit.
        /// </summary>
        public bool IsAbility => _techId[0] == 'A';

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
// ------------------------------------------------------------------------------
// <copyright file="ForceData.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text;

using War3Net.Build.Extensions;

namespace War3Net.Build.Info
{
    public sealed class ForceData
    {
        private ForceFlags _forceFlags;
        private int _playersMask;
        private string _forceName;

        public ForceFlags ForceFlags
        {
            get => _forceFlags;
            set => _forceFlags = value;
        }

        public string ForceName
        {
            get => _forceName;
            set => _forceName = value;
        }

        public static ForceData Parse(Stream stream, bool leaveOpen = false)
        {
            var data = new ForceData();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                data._forceFlags = (ForceFlags)reader.ReadInt32();
                data._playersMask = reader.ReadInt32();
                data._forceName = reader.ReadChars();
            }

            return data;
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write((int)_forceFlags);
            writer.Write(_playersMask);
            writer.WriteString(_forceName);
        }

        public bool ContainsPlayer(int playerIndex)
        {
            // Note: if the player slot is unused, this usually returns true.
            return (_playersMask & (1 << playerIndex)) != 0;
        }

        public IEnumerable<int> GetPlayers()
        {
            const int MaxPlayerSlots = 24;

            for (var index = 0; index < MaxPlayerSlots; index++)
            {
                if (ContainsPlayer(index))
                {
                    yield return index;
                }
            }
        }

        public void SetPlayers(params PlayerData[] players)
        {
            _playersMask = 0;
            foreach (var player in players)
            {
                _playersMask |= (1 << player.PlayerNumber);
            }
        }
    }
}
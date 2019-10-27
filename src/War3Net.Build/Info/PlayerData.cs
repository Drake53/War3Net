// ------------------------------------------------------------------------------
// <copyright file="PlayerData.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

using War3Net.Build.Extensions;

namespace War3Net.Build.Info
{
    public sealed class PlayerData
    {
        private int _playerNumber;
        private PlayerController _playerController;
        private PlayerRace _playerRace;
        private PlayerFlags _playerFlags;
        private string _playerName;
        private PointF _startPosition;
        private int _allyLowPriorityFlags;
        private int _allyHighPriorityFlags;

        public int PlayerNumber
        {
            get => _playerNumber;
            set => _playerNumber = value;
        }

        public PlayerController PlayerController
        {
            get => _playerController;
            set => _playerController = value;
        }

        public PlayerRace PlayerRace
        {
            get => _playerRace;
            set => _playerRace = value;
        }

        public bool FixedStartPosition
        {
            get => _playerFlags.HasFlag(PlayerFlags.FixedStartPosition);
            set => _playerFlags = value ? _playerFlags | PlayerFlags.FixedStartPosition : _playerFlags & ~PlayerFlags.FixedStartPosition;
        }

        public bool IsRaceSelectable
        {
            get => _playerFlags.HasFlag(PlayerFlags.RaceSelectable);
            set => _playerFlags = value ? _playerFlags | PlayerFlags.RaceSelectable : _playerFlags & ~PlayerFlags.RaceSelectable;
        }

        public string PlayerName
        {
            get => _playerName;
            set => _playerName = value;
        }

        public PointF StartPosition
        {
            get => _startPosition;
            set => _startPosition = value;
        }

        public static PlayerData Parse(Stream stream, bool leaveOpen = false)
        {
            var data = new PlayerData();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                data._playerNumber = reader.ReadInt32();
                data._playerController = (PlayerController)reader.ReadInt32();
                data._playerRace = (PlayerRace)reader.ReadInt32();
                data._playerFlags = (PlayerFlags)reader.ReadInt32();
                data._playerName = reader.ReadChars();
                data._startPosition = new PointF(reader.ReadSingle(), reader.ReadSingle());

                // Note: if _playerController is Computer, these values sometimes appear somewhat random.
                data._allyLowPriorityFlags = reader.ReadInt32();
                data._allyHighPriorityFlags = reader.ReadInt32();
            }

            return data;
        }

        public void WriteTo(BinaryWriter writer)
        {
            writer.Write(_playerNumber);
            writer.Write((int)_playerController);
            writer.Write((int)_playerRace);
            writer.Write((int)_playerFlags);
            writer.WriteString(_playerName);
            writer.Write(_startPosition.X);
            writer.Write(_startPosition.Y);
            writer.Write(_allyLowPriorityFlags);
            writer.Write(_allyHighPriorityFlags);
        }

        public bool HasLowPriorityFlag(int otherPlayerIndex)
        {
            return (_allyLowPriorityFlags & (1 << otherPlayerIndex)) != 0;
        }

        public bool HasHighPriorityFlag(int otherPlayerIndex)
        {
            return (_allyHighPriorityFlags & (1 << otherPlayerIndex)) != 0;
        }

        public IEnumerable<(int index, bool highPriority)> GetStartLocationPriorities()
        {
            const int MaxPlayerSlots = 24;

            for (var index = 0; index < MaxPlayerSlots; index++)
            {
                if (HasLowPriorityFlag(index))
                {
                    yield return (index, false);
                }
                else if (HasHighPriorityFlag(index))
                {
                    yield return (index, true);
                }
            }
        }
    }
}
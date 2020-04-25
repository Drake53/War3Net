// ------------------------------------------------------------------------------
// <copyright file="Player.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Replay
{
    public sealed class Player
    {
        private PlayerRecord _record;
        private byte _id;
        private string _name;
        private byte[] _customData;

        public static Player Parse(Stream stream, bool leaveOpen = false)
        {
            var player = new Player();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                player._record = (PlayerRecord)reader.ReadByte();
                player._id = reader.ReadByte();
                player._name = reader.ReadChars();
                if (string.IsNullOrEmpty(player._name))
                {
                    player._name = $"Player {player._id}";
                }

                var customDataLength = reader.ReadByte();
                player._customData = new byte[customDataLength];
                for (var i = 0; i < customDataLength; i++)
                {
                    player._customData[i] = reader.ReadByte();
                }
            }

            return player;
        }
    }
}
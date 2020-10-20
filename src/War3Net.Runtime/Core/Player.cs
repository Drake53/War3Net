// ------------------------------------------------------------------------------
// <copyright file="Player.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Runtime.Core
{
    public sealed class Player : Agent
    {
        public const int MaxPlayerSlotCount = 24 + 4;

        private static readonly Player[] _players = new Player[MaxPlayerSlotCount];

        private static Player _localPlayer;

        private readonly int _id;
        private readonly string _name;

        private Player(int id, string name)
        {
            _id = id;
            _name = name;
        }

        public static Player LocalPlayer => _localPlayer;

        public int Id => _id;

        public string Name => _name;

        public static void CreatePlayersTest()
        {
            for (var i = 0; i < MaxPlayerSlotCount; i++)
            {
                _players[i] = new Player(i, i == 0 ? "WorldEdit" : $"Player {i + 1}");
            }

            _localPlayer = _players[0];
        }

        public static Player GetPlayer(int id)
        {
            return _players[id];
        }

        public override void Dispose()
        {
            // throw new NotImplementedException();
        }
    }
}
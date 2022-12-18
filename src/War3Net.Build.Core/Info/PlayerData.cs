// ------------------------------------------------------------------------------
// <copyright file="PlayerData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Numerics;

using War3Net.Build.Common;

namespace War3Net.Build.Info
{
    public sealed partial class PlayerData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerData"/> class.
        /// </summary>
        public PlayerData()
        {
            AllyLowPriorityFlags = new Bitmask32(0);
            AllyHighPriorityFlags = new Bitmask32(0);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerData"/> class.
        /// </summary>
        public PlayerData(int id)
            : this()
        {
            Id = id;
            Name = $"Player {id + 1}";
        }

        public int Id { get; set; }

        public PlayerController Controller { get; set; }

        public PlayerRace Race { get; set; }

        public PlayerFlags Flags { get; set; }

        public string Name { get; set; }

        public Vector2 StartPosition { get; set; }

        public Bitmask32 AllyLowPriorityFlags { get; set; }

        public Bitmask32 AllyHighPriorityFlags { get; set; }

        public Bitmask32 EnemyLowPriorityFlags { get; set; }

        public Bitmask32 EnemyHighPriorityFlags { get; set; }

        public override string ToString() => Name;
    }
}
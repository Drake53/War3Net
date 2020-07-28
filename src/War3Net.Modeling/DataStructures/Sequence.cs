// ------------------------------------------------------------------------------
// <copyright file="Sequence.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Modeling.Enums;

namespace War3Net.Modeling.DataStructures
{
    public struct Sequence
    {
        public string Name { get; set; }

        public uint IntervalStart { get; set; }

        public uint IntervalEnd { get; set; }

        public float MoveSpeed { get; set; }

        public SequenceFlags Flags { get; set; }

        public float Rarity { get; set; }

        public uint SyncPoint { get; set; }

        public Extent Extent { get; set; }
    }
}
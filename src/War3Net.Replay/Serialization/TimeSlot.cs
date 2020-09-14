// ------------------------------------------------------------------------------
// <copyright file="TimeSlot.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using MessagePack;

namespace War3Net.Replay.Serialization
{
    [MessagePackObject]
    public struct TimeSlot
    {
        [Key(0)]
        public ushort TimeIncrement;

        [Key(1)]
        public CommandData[] Transactions;
    }
}
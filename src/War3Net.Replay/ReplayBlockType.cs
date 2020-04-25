// ------------------------------------------------------------------------------
// <copyright file="ReplayBlockType.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Replay
{
    public enum ReplayBlockType : byte
    {
        LeaveGame = 0x17,

        StartBlockOne = 0x1A,
        StartBlockTwo = 0x1B,
        StartBlockThree = 0x1C,

        TimeslotOld = 0x1E,
        Timeslot = 0x1F,

        ChatMessage = 0x20,
        ChecksumOrSeed = 0x22,
        Unknown = 0x23,
        ForcedGameEnd = 0x2F,
    }
}
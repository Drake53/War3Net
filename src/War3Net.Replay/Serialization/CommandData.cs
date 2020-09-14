// ------------------------------------------------------------------------------
// <copyright file="CommandData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using MessagePack;

namespace War3Net.Replay.Serialization
{
    [MessagePackObject]
    public struct CommandData
    {
        [Key(0)]
        public byte PlayerId;

        [Key(1)]
        public Action[] Actions;
    }
}
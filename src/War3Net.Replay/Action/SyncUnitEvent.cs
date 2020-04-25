// ------------------------------------------------------------------------------
// <copyright file="SyncUnitEvent.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

namespace War3Net.Replay.Action
{
    public sealed class SyncUnitEvent : GamecacheSyncEvent
    {
        public SyncUnitEvent(Stream data)
            : base(data)
        {
            using (var reader = new BinaryReader(data, new UTF8Encoding(false, true), true))
            {
                var unitType = reader.ReadChars(4);
                // todo
            }

            data.Seek(87, SeekOrigin.Current);
        }
    }
}
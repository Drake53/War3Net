// ------------------------------------------------------------------------------
// <copyright file="SyncRealEvent.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

namespace War3Net.Replay.Action
{
    public sealed class SyncRealEvent : GamecacheSyncEvent
    {
        private readonly float _value;

        public SyncRealEvent(Stream data)
            : base(data)
        {
            using (var reader = new BinaryReader(data, new UTF8Encoding(false, true), true))
            {
                _value = reader.ReadSingle();
            }
        }
    }
}
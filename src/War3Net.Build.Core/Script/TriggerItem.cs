// ------------------------------------------------------------------------------
// <copyright file="TriggerItem.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Script
{
    public abstract class TriggerItem
    {
        internal TriggerItem(TriggerItemType triggerItemType)
        {
            Type = triggerItemType;
        }

        public TriggerItemType Type { get; private init; }

        public string Name { get; set; }

        public int Id { get; set; }

        public int ParentId { get; set; }

        internal abstract void WriteTo(BinaryWriter writer, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion);
    }
}
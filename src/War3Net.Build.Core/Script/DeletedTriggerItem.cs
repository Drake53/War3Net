// ------------------------------------------------------------------------------
// <copyright file="DeletedTriggerItem.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Script
{
    public sealed class DeletedTriggerItem : TriggerItem
    {
        internal DeletedTriggerItem(TriggerItemType triggerItemType)
            : base(triggerItemType)
        {
        }

        internal DeletedTriggerItem(BinaryReader reader, TriggerItemType triggerItemType, TriggerData triggerData, MapTriggersFormatVersion formatVersion, bool useNewFormat)
            : base(triggerItemType)
        {
            ReadFrom(reader, triggerData, formatVersion, useNewFormat);
        }

        internal void ReadFrom(BinaryReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, bool useNewFormat)
        {
            Id = reader.ReadInt32();

            Name = "<DELETED>";
            ParentId = -1;
        }

        internal override void WriteTo(BinaryWriter writer, MapTriggersFormatVersion formatVersion, bool useNewFormat)
        {
            if (useNewFormat)
            {
                writer.Write(Id);
            }
        }
    }
}
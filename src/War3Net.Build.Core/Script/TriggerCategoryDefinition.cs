// ------------------------------------------------------------------------------
// <copyright file="TriggerCategoryDefinition.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed class TriggerCategoryDefinition : TriggerItem
    {
        internal TriggerCategoryDefinition(TriggerItemType triggerItemType = TriggerItemType.Category)
            : base(triggerItemType)
        {
        }

        internal TriggerCategoryDefinition(BinaryReader reader, TriggerItemType triggerItemType, TriggerData triggerData, MapTriggersFormatVersion formatVersion, bool useNewFormat)
            : base(triggerItemType)
        {
            ReadFrom(reader, triggerData, formatVersion, useNewFormat);
        }

        public bool IsComment { get; set; }

        public int Unk { get; set; }

        internal void ReadFrom(BinaryReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, bool useNewFormat)
        {
            Id = reader.ReadInt32();
            Name = reader.ReadChars();
            if (formatVersion >= MapTriggersFormatVersion.Tft)
            {
                IsComment = reader.ReadBool();
            }

            if (useNewFormat)
            {
                Unk = reader.ReadInt32();
                ParentId = reader.ReadInt32();
            }
        }

        internal override void WriteTo(BinaryWriter writer, MapTriggersFormatVersion formatVersion, bool useNewFormat)
        {
            writer.Write(Id);
            writer.WriteString(Name);
            if (formatVersion >= MapTriggersFormatVersion.Tft)
            {
                writer.WriteBool(IsComment);
            }

            if (useNewFormat)
            {
                writer.Write(Unk);
                writer.Write(ParentId);
            }
        }
    }
}
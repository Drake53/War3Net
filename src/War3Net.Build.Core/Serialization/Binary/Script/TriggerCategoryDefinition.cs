namespace War3Net.Build.Script
{
    public sealed partial class TriggerCategoryDefinition : TriggerItem
    {
        internal TriggerCategoryDefinition(BinaryReader reader, TriggerItemType triggerItemType, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
            : base(triggerItemType)
        {
            ReadFrom(reader, triggerData, formatVersion, subVersion);
        }

        internal void ReadFrom(BinaryReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            Id = reader.ReadInt32();
            Name = reader.ReadChars();
            if (formatVersion >= MapTriggersFormatVersion.v7)
            {
                IsComment = reader.ReadBool();
            }

            if (subVersion is not null)
            {
                IsExpanded = reader.ReadBool();
                ParentId = reader.ReadInt32();
            }
        }

        internal override void WriteTo(BinaryWriter writer, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            writer.Write(Id);
            writer.WriteString(Name);
            if (formatVersion >= MapTriggersFormatVersion.v7)
            {
                writer.WriteBool(IsComment);
            }

            if (subVersion is not null)
            {
                writer.WriteBool(IsExpanded);
                writer.Write(ParentId);
            }
        }
    }
}
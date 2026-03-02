namespace War3Net.Build.Script
{
    public sealed partial class DeletedTriggerItem : TriggerItem
    {
        internal DeletedTriggerItem(BinaryReader reader, TriggerItemType triggerItemType, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
            : base(triggerItemType)
        {
            ReadFrom(reader, triggerData, formatVersion, subVersion);
        }

        internal void ReadFrom(BinaryReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            Id = reader.ReadInt32();

            Name = "<DELETED>";
            ParentId = -1;
        }

        internal override void WriteTo(BinaryWriter writer, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            if (subVersion is not null)
            {
                writer.Write(Id);
            }
        }
    }
}
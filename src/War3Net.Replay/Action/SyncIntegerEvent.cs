namespace War3Net.Replay.Action
{
    public sealed class SyncIntegerEvent : GamecacheSyncEvent
    {
        private readonly int _value;

        public SyncIntegerEvent(Stream data)
            : base(data)
        {
            using (var reader = new BinaryReader(data, new UTF8Encoding(false, true), true))
            {
                _value = reader.ReadInt32();
            }
        }

        public bool IsMmdAction => CampaignFileName == "MMD.Dat";

        public bool IsMmdMessage => IsMmdAction && MissionKey.StartsWith("val:");

        public bool IsMmdChecksum => IsMmdAction && MissionKey.StartsWith("chk:");
    }
}
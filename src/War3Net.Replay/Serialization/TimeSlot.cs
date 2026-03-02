using MessagePack;

namespace War3Net.Replay.Serialization
{
    [MessagePackObject]
    public struct TimeSlot
    {
        [Key(0)]
        public ushort TimeIncrement;

        [Key(1)]
        public CommandData[] Transactions;
    }
}
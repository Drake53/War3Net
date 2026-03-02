using MessagePack;

namespace War3Net.Replay.Serialization
{
    [MessagePackObject]
    public struct Action
    {
        [Key(0)]
        public byte ActionId;

        [Key(1)]
        public byte[] Data;
    }
}
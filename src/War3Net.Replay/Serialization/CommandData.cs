namespace War3Net.Replay.Serialization
{
    [MessagePackObject]
    public struct CommandData
    {
        [Key(0)]
        public byte PlayerId;

        [Key(1)]
        public Action[] Actions;
    }
}
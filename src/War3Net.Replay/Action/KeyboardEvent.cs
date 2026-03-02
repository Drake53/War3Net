namespace War3Net.Replay.Action
{
    public sealed class KeyboardEventBlock : ActionBlock
    {
        public KeyboardEventBlock(Stream data)
        {
            using var reader = new BinaryReader(data, new UTF8Encoding(false, true), true);

            var unk1 = reader.ReadUInt32();
            var unk2 = reader.ReadUInt32();
            if (unk1 != unk2)
            {
                throw new InvalidDataException();
            }

            var unk3 = reader.ReadUInt32();

            var keyType = reader.ReadUInt32();
            var metaKeyType = reader.ReadUInt32();
        }
    }
}
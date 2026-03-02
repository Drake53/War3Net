using System;

namespace War3Net.IO.Mpq
{
    [Flags]
    public enum AttributesFlags
    {
        Crc32 = 1 << 0,
        DateTime = 1 << 1,
        Unk0x04 = 1 << 2,
    }
}
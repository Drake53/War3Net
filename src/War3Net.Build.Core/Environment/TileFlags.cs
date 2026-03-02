using System;

namespace War3Net.Build.Environment
{
    [Flags]
    public enum TileFlags : byte
    {
        Ramp = 1 << 0,
        Blighted = 1 << 1,
        Water = 1 << 2,
        Boundary = 1 << 3,
    }
}
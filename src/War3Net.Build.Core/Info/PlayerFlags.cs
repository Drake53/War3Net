using System;

namespace War3Net.Build.Info
{
    [Flags]
    public enum PlayerFlags
    {
        FixedStartPosition = 1 << 0,
        RaceSelectable = 1 << 1,
    }
}
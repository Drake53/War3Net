using System;

namespace War3Net.Build.Info
{
    [Flags]
    public enum CampaignFlags
    {
        VariableDifficultyLevels = 1 << 0,
        RequiresExpansion = 1 << 1,
    }
}
namespace War3Net.Build.Info
{
    [Flags]
    public enum ForceFlags
    {
        Allied = 1 << 0,
        AlliedVictory = 1 << 1,
        UNK4 = 1 << 2,
        ShareVision = 1 << 3,
        ShareUnitControl = 1 << 4,
        ShareAdvancedUnitControl = 1 << 5,
    }
}
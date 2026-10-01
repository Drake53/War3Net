namespace War3Net.Build.Info
{
    /// <summary>Introduced in patch 3.0.0.</summary>
    [Flags]
    public enum PlayerRaceHud
    {
        Human = 1 << 0,
        Orc = 1 << 1,
        NightElf = 1 << 2,
        Undead = 1 << 3,
        Selectable = 1 << 6,
        Forsaken = 1 << 7,
    }
}
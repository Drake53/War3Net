namespace War3Net.Build.Configuration
{
    [Flags]
    public enum GameConfigurationFlags
    {
        IsFogOfWarDisabled = 1 << 0,
        IsVictoryDefeatConditionsDisabled = 1 << 1,
    }
}
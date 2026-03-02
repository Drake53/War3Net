namespace War3Net.Build.Configuration
{
    [Flags]
    public enum GameConfigurationPlayerInfoFlags
    {
        IsUser = 1 << 0,
        IsObserver = 1 << 1,
        LoadCustomAIFile = 1 << 2,
        AIFilePathIsAbsolute = 1 << 3,
    }
}
using System;

namespace War3Net.Build.Audio
{
    [Obsolete]
    // NOTE: this is similar case as Sound.EAXSetting, where it's stored as string, but in WE it's limited to the options in a dropdown.
    public enum SoundEnvironment
    {
        Mountains,
        Lake,
        Psychotic,
        Dungeon,
    }
}
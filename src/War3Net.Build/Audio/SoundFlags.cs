// ------------------------------------------------------------------------------
// <copyright file="SoundFlags.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Build.Audio
{
    [Flags]
    public enum SoundFlags
    {
        Looping = 1 << 0,
        Is3DSound = 1 << 1,
        StopWhenOutOfRange = 1 << 2,
        Music = 1 << 3,
    }
}
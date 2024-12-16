// ------------------------------------------------------------------------------
// <copyright file="RegionExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Audio;
using War3Net.Build.Environment;

namespace War3Net.Build.Extensions
{

    public static class SoundExtensions
    {
        public static string GetVariableName(this Sound sound)
        {
            return $"gg_snd_{sound.Name.Replace(' ', '_')}";
        }
    }
}
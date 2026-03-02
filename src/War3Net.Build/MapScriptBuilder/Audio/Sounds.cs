// ------------------------------------------------------------------------------
// <copyright file="Sounds.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using War3Net.Build.Audio;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateSoundVariables(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var mapSounds = map.Sounds;
            if (mapSounds is null)
            {
                return;
            }

            foreach (var sound in mapSounds.Sounds)
            {
                if (sound.Flags.HasFlag(SoundFlags.Music))
                {
                    writer.WriteAlignedGlobal(
                        JassKeyword.String,
                        sound.Name);
                }
                else
                {
                    writer.WriteAlignedGlobal(
                        TypeName.Sound,
                        sound.Name,
                        JassKeyword.Null);
                }
            }
        }

        protected internal virtual bool ShouldGenerateSoundVariables(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Sounds is not null
                && map.Sounds.Sounds.Count > 0;
        }
    }
}
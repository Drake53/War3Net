// ------------------------------------------------------------------------------
// <copyright file="VolumeGroupApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Enums;

namespace War3Net.Runtime.Api.Common.Enums
{
    public static class VolumeGroupApi
    {
        public static readonly VolumeGroup SOUND_VOLUMEGROUP_UNITMOVEMENT = ConvertVolumeGroup((int)VolumeGroup.Type.UnitMovement);
        public static readonly VolumeGroup SOUND_VOLUMEGROUP_UNITSOUNDS = ConvertVolumeGroup((int)VolumeGroup.Type.UnitSounds);
        public static readonly VolumeGroup SOUND_VOLUMEGROUP_COMBAT = ConvertVolumeGroup((int)VolumeGroup.Type.Combat);
        public static readonly VolumeGroup SOUND_VOLUMEGROUP_SPELLS = ConvertVolumeGroup((int)VolumeGroup.Type.Spells);
        public static readonly VolumeGroup SOUND_VOLUMEGROUP_UI = ConvertVolumeGroup((int)VolumeGroup.Type.UI);
        public static readonly VolumeGroup SOUND_VOLUMEGROUP_MUSIC = ConvertVolumeGroup((int)VolumeGroup.Type.Music);
        public static readonly VolumeGroup SOUND_VOLUMEGROUP_AMBIENTSOUNDS = ConvertVolumeGroup((int)VolumeGroup.Type.AmbientSounds);
        public static readonly VolumeGroup SOUND_VOLUMEGROUP_FIRE = ConvertVolumeGroup((int)VolumeGroup.Type.Fire);

        public static VolumeGroup ConvertVolumeGroup(int i)
        {
            return VolumeGroup.GetVolumeGroup(i);
        }
    }
}
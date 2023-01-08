// ------------------------------------------------------------------------------
// <copyright file="Sound.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Numerics;

namespace War3Net.Build.Audio
{
    public sealed partial class Sound
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sound"/> class.
        /// </summary>
        public Sound()
        {
        }

        public string Name { get; set; }

        public string FilePath { get; set; }

        // TODO: enum?
        public string EaxSetting { get; set; }

        public SoundFlags Flags { get; set; }

        public int FadeInRate { get; set; }

        public int FadeOutRate { get; set; }

        public int Volume { get; set; }

        public float Pitch { get; set; }

        // used when RANDOMPITCH flag is set
        public float PitchVariance { get; set; }

        public int Priority { get; set; }

        public SoundChannel Channel { get; set; }

        public float MinDistance { get; set; }

        public float MaxDistance { get; set; }

        public float DistanceCutoff { get; set; }

        public float ConeAngleInside { get; set; }

        public float ConeAngleOutside { get; set; }

        public int ConeOutsideVolume { get; set; }

        public Vector3 ConeOrientation { get; set; }

        // 'SoundName' in .slk files? (reforged only, can be different from name in filepath, eg DeathHumanLargeBuilding = BuildingDeathLargeHuman.wav).
        public string SoundName { get; set; }

        public int DialogueTextKey { get; set; }

        public string Unk2 { get; set; }

        public int DialogueSpeakerNameKey { get; set; }

        public int Unk4 { get; set; }

        public string Unk5 { get; set; }

        public string Unk6 { get; set; }

        public string Unk7 { get; set; }

        public string FacialAnimationLabel { get; set; }

        public string FacialAnimationGroupLabel { get; set; }

        public string FacialAnimationSetFilepath { get; set; }

        public int Unk11 { get; set; }

        public override string ToString() => FilePath;
    }
}
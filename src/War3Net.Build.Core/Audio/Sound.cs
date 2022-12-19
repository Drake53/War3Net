// ------------------------------------------------------------------------------
// <copyright file="Sound.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Numerics;

using War3Net.Common.Extensions;

namespace War3Net.Build.Audio
{
    public sealed class Sound
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sound"/> class.
        /// </summary>
        public Sound()
        {
        }

        internal Sound(BinaryReader reader, MapSoundsFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
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

        internal void ReadFrom(BinaryReader reader, MapSoundsFormatVersion formatVersion)
        {
            Name = reader.ReadChars();
            FilePath = reader.ReadChars();
            EaxSetting = reader.ReadChars();
            Flags = reader.ReadInt32<SoundFlags>();
            FadeInRate = reader.ReadInt32();
            FadeOutRate = reader.ReadInt32();
            Volume = reader.ReadInt32();
            Pitch = reader.ReadSingle();
            PitchVariance = reader.ReadSingle();
            Priority = reader.ReadInt32();
            Channel = reader.ReadInt32<SoundChannel>();
            MinDistance = reader.ReadSingle();
            MaxDistance = reader.ReadSingle();
            DistanceCutoff = reader.ReadSingle();
            ConeAngleInside = reader.ReadSingle();
            ConeAngleOutside = reader.ReadSingle();
            ConeOutsideVolume = reader.ReadInt32();
            ConeOrientation = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

            if (formatVersion >= MapSoundsFormatVersion.v2)
            {
                var repeatVariableName = reader.ReadChars();
                SoundName = reader.ReadChars();
                var repeatSoundPath = reader.ReadChars();

                if (repeatVariableName != Name || repeatSoundPath != FilePath)
                {
                    throw new InvalidDataException($"Expected sound's {nameof(Name)} and {nameof(FilePath)} to be repeated.");
                }

                DialogueTextKey = reader.ReadInt32();
                Unk2 = reader.ReadChars();
                DialogueSpeakerNameKey = reader.ReadInt32();

                Unk4 = reader.ReadInt32();
                if (Unk4 != 0)
                {
                    Unk5 = reader.ReadChars();
                }

                Unk6 = reader.ReadChars();
                Unk7 = reader.ReadChars();
                FacialAnimationLabel = reader.ReadChars();
                FacialAnimationGroupLabel = reader.ReadChars();
                FacialAnimationSetFilepath = reader.ReadChars();

                if (formatVersion >= MapSoundsFormatVersion.v3)
                {
                    Unk11 = reader.ReadInt32();
                }
            }
        }

        internal void WriteTo(BinaryWriter writer, MapSoundsFormatVersion formatVersion)
        {
            writer.WriteString(Name);
            writer.WriteString(FilePath);
            writer.WriteString(EaxSetting);
            writer.Write((int)Flags);
            writer.Write(FadeInRate);
            writer.Write(FadeOutRate);
            writer.Write(Volume);
            writer.Write(Pitch);
            writer.Write(PitchVariance);
            writer.Write(Priority);
            writer.Write((int)Channel);
            writer.Write(MinDistance);
            writer.Write(MaxDistance);
            writer.Write(DistanceCutoff);
            writer.Write(ConeAngleInside);
            writer.Write(ConeAngleOutside);
            writer.Write(ConeOutsideVolume);
            writer.Write(ConeOrientation.X);
            writer.Write(ConeOrientation.Y);
            writer.Write(ConeOrientation.Z);

            if (formatVersion >= MapSoundsFormatVersion.v2)
            {
                writer.WriteString(Name);
                writer.WriteString(SoundName);
                writer.WriteString(FilePath);

                writer.Write(DialogueTextKey);
                writer.WriteString(Unk2);
                writer.Write(DialogueSpeakerNameKey);

                writer.Write(Unk4);
                if (Unk4 != 0)
                {
                    writer.WriteString(Unk5);
                }

                writer.WriteString(Unk6);
                writer.WriteString(Unk7);
                writer.WriteString(FacialAnimationLabel);
                writer.WriteString(FacialAnimationGroupLabel);
                writer.WriteString(FacialAnimationSetFilepath);

                if (formatVersion >= MapSoundsFormatVersion.v3)
                {
                    writer.Write(Unk11);
                }
            }
        }
    }
}
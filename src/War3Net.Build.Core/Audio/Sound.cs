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

        // TODO: reforged properties

        // 'SoundName' in .slk files? (reforged only, can be different from name in filepath, eg DeathHumanLargeBuilding = BuildingDeathLargeHuman.wav).
        public string SoundName { get; set; }

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

            if (formatVersion >= MapSoundsFormatVersion.Reforged)
            {
                var repeatVariableName = reader.ReadChars();
                SoundName = reader.ReadChars();
                var repeatSoundPath = reader.ReadChars();

                if (repeatVariableName != Name || repeatSoundPath != FilePath)
                {
                    throw new InvalidDataException($"Expected sound's {nameof(Name)} and {nameof(FilePath)} to be repeated.");
                }

                var unk2 = reader.ReadInt32();
                var unk3 = reader.ReadByte();
                var unk4 = reader.ReadInt32();
                var unk5 = reader.ReadByte();
                var unk6 = reader.ReadInt32();
                var unk7 = reader.ReadInt32();

                if (formatVersion >= MapSoundsFormatVersion.ReforgedV3)
                {
                    var unk8 = reader.ReadInt32();
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

            if (formatVersion >= MapSoundsFormatVersion.Reforged)
            {
                writer.WriteString(Name);
                writer.WriteString(SoundName);
                writer.WriteString(FilePath);

                writer.Write(-1);
                writer.Write((byte)0);
                writer.Write(-1);
                writer.Write((byte)0);
                writer.Write(0);
                writer.Write(0);

                if (formatVersion >= MapSoundsFormatVersion.ReforgedV3)
                {
                    writer.Write(1);
                }
            }
        }
    }
}
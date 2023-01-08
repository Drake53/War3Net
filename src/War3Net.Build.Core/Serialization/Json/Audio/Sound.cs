// ------------------------------------------------------------------------------
// <copyright file="Sound.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Audio
{
    public sealed partial class Sound
    {
        internal Sound(JsonElement jsonElement, MapSoundsFormatVersion formatVersion)
        {
            GetFrom(jsonElement, formatVersion);
        }

        internal Sound(ref Utf8JsonReader reader, MapSoundsFormatVersion formatVersion)
        {
            ReadFrom(ref reader, formatVersion);
        }

        internal void GetFrom(JsonElement jsonElement, MapSoundsFormatVersion formatVersion)
        {
            Name = jsonElement.GetString(nameof(Name));
            FilePath = jsonElement.GetString(nameof(FilePath));
            EaxSetting = jsonElement.GetString(nameof(EaxSetting));
            Flags = jsonElement.GetInt32<SoundFlags>(nameof(Flags));
            FadeInRate = jsonElement.GetInt32(nameof(FadeInRate));
            FadeOutRate = jsonElement.GetInt32(nameof(FadeOutRate));
            Volume = jsonElement.GetInt32(nameof(Volume));
            Pitch = jsonElement.GetSingle(nameof(Pitch));
            PitchVariance = jsonElement.GetSingle(nameof(PitchVariance));
            Priority = jsonElement.GetInt32(nameof(Priority));
            Channel = jsonElement.GetInt32<SoundChannel>(nameof(Channel));
            MinDistance = jsonElement.GetSingle(nameof(MinDistance));
            MaxDistance = jsonElement.GetSingle(nameof(MaxDistance));
            DistanceCutoff = jsonElement.GetSingle(nameof(DistanceCutoff));
            ConeAngleInside = jsonElement.GetSingle(nameof(ConeAngleInside));
            ConeAngleOutside = jsonElement.GetSingle(nameof(ConeAngleOutside));
            ConeOutsideVolume = jsonElement.GetInt32(nameof(ConeOutsideVolume));
            ConeOrientation = jsonElement.GetVector3(nameof(ConeOrientation));

            if (formatVersion >= MapSoundsFormatVersion.v2)
            {
                SoundName = jsonElement.GetString(nameof(SoundName));
                DialogueTextKey = jsonElement.GetInt32(nameof(DialogueTextKey));
                Unk2 = jsonElement.GetString(nameof(Unk2));
                DialogueSpeakerNameKey = jsonElement.GetInt32(nameof(DialogueSpeakerNameKey));

                Unk4 = jsonElement.GetInt32(nameof(Unk4));
                if (Unk4 != 0)
                {
                    Unk5 = jsonElement.GetString(nameof(Unk5));
                }

                Unk6 = jsonElement.GetString(nameof(Unk6));
                Unk7 = jsonElement.GetString(nameof(Unk7));
                FacialAnimationLabel = jsonElement.GetString(nameof(FacialAnimationLabel));
                FacialAnimationGroupLabel = jsonElement.GetString(nameof(FacialAnimationGroupLabel));
                FacialAnimationSetFilepath = jsonElement.GetString(nameof(FacialAnimationSetFilepath));

                if (formatVersion >= MapSoundsFormatVersion.v3)
                {
                    Unk11 = jsonElement.GetInt32(nameof(Unk11));
                }
            }
        }

        internal void ReadFrom(ref Utf8JsonReader reader, MapSoundsFormatVersion formatVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapSoundsFormatVersion formatVersion)
        {
            writer.WriteStartObject();

            writer.WriteString(nameof(Name), Name);
            writer.WriteString(nameof(FilePath), FilePath);
            writer.WriteString(nameof(EaxSetting), EaxSetting);
            writer.WriteObject(nameof(Flags), Flags, options);
            writer.WriteNumber(nameof(FadeInRate), FadeInRate);
            writer.WriteNumber(nameof(FadeOutRate), FadeOutRate);
            writer.WriteNumber(nameof(Volume), Volume);
            writer.WriteNumber(nameof(Pitch), Pitch);
            writer.WriteNumber(nameof(PitchVariance), PitchVariance);
            writer.WriteNumber(nameof(Priority), Priority);
            writer.WriteObject(nameof(Channel), Channel, options);
            writer.WriteNumber(nameof(MinDistance), MinDistance);
            writer.WriteNumber(nameof(MaxDistance), MaxDistance);
            writer.WriteNumber(nameof(DistanceCutoff), DistanceCutoff);
            writer.WriteNumber(nameof(ConeAngleInside), ConeAngleInside);
            writer.WriteNumber(nameof(ConeAngleOutside), ConeAngleOutside);
            writer.WriteNumber(nameof(ConeOutsideVolume), ConeOutsideVolume);
            writer.Write(nameof(ConeOrientation), ConeOrientation);

            if (formatVersion >= MapSoundsFormatVersion.v2)
            {
                writer.WriteString(nameof(SoundName), SoundName);
                writer.WriteNumber(nameof(DialogueTextKey), DialogueTextKey);
                writer.WriteString(nameof(Unk2), Unk2);
                writer.WriteNumber(nameof(DialogueSpeakerNameKey), DialogueSpeakerNameKey);

                writer.WriteNumber(nameof(Unk4), Unk4);
                if (Unk4 != 0)
                {
                    writer.WriteString(nameof(Unk5), Unk5);
                }

                writer.WriteString(nameof(Unk6), Unk6);
                writer.WriteString(nameof(Unk7), Unk7);
                writer.WriteString(nameof(FacialAnimationLabel), FacialAnimationLabel);
                writer.WriteString(nameof(FacialAnimationGroupLabel), FacialAnimationGroupLabel);
                writer.WriteString(nameof(FacialAnimationSetFilepath), FacialAnimationSetFilepath);

                if (formatVersion >= MapSoundsFormatVersion.v3)
                {
                    writer.WriteNumber(nameof(Unk11), Unk11);
                }
            }

            writer.WriteEndObject();
        }
    }
}
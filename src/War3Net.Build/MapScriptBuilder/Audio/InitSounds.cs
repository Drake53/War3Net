// ------------------------------------------------------------------------------
// <copyright file="InitSounds.cs" company="Drake53">
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
        protected internal virtual void GenerateInitSounds(Map map, IndentedTextWriter writer)
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
                throw new ArgumentException($"Function '{GeneratedFunctionName.InitSounds}' cannot be generated without {nameof(MapSounds)}.", nameof(map));
            }

            writer.WriteFunction(GeneratedFunctionName.InitSounds);

            foreach (var sound in mapSounds.Sounds)
            {
                if (sound.Flags.HasFlag(SoundFlags.Music))
                {
                    writer.WriteSet(
                        sound.Name,
                        JassLiteral.String(sound.FilePath));
                }
                else
                {
                    var is3DSound = sound.Flags.HasFlag(SoundFlags.Is3DSound)
                        && sound.Channel != SoundChannel.Error
                        && sound.Channel != SoundChannel.Music
                        && sound.Channel != SoundChannel.UserInterface;

                    writer.WriteSet(
                        sound.Name,
                        JassExpression.InvokeSpaced(
                            NativeName.CreateSound,
                            JassLiteral.String(sound.FilePath),
                            JassLiteral.Bool(sound.Flags.HasFlag(SoundFlags.Looping)),
                            JassLiteral.Bool(is3DSound),
                            JassLiteral.Bool(sound.Flags.HasFlag(SoundFlags.StopWhenOutOfRange)),
                            JassLiteral.Int(sound.FadeInRate),
                            JassLiteral.Int(sound.FadeOutRate),
                            JassLiteral.String(sound.EaxSetting)));

                    if (!string.IsNullOrEmpty(sound.FacialAnimationLabel))
                    {
                        writer.WriteCall(
                            NativeName.SetSoundFacialAnimationLabel,
                            sound.Name,
                            JassLiteral.String(sound.FacialAnimationLabel));
                    }

                    if (!string.IsNullOrEmpty(sound.FacialAnimationGroupLabel))
                    {
                        writer.WriteCall(
                            NativeName.SetSoundFacialAnimationGroupLabel,
                            sound.Name,
                            JassLiteral.String(sound.FacialAnimationGroupLabel));
                    }

                    if (!string.IsNullOrEmpty(sound.FacialAnimationSetFilepath))
                    {
                        writer.WriteCall(
                            NativeName.SetSoundFacialAnimationSetFilepath,
                            sound.Name,
                            JassLiteral.String(sound.FacialAnimationSetFilepath));
                    }

                    if (sound.DialogueSpeakerNameKey > 0)
                    {
                        writer.WriteCall(
                            NativeName.SetDialogueSpeakerNameKey,
                            sound.Name,
                            JassLiteral.String($"TRIGSTR_{sound.DialogueSpeakerNameKey}"));
                    }

                    if (sound.DialogueTextKey > 0)
                    {
                        writer.WriteCall(
                            NativeName.SetDialogueTextKey,
                            sound.Name,
                            JassLiteral.String($"TRIGSTR_{sound.DialogueTextKey}"));
                    }

                    if (sound.DistanceCutoff != 3000f)
                    {
                        var distanceCutoff = sound.DistanceCutoff == uint.MaxValue ? 3000f : sound.DistanceCutoff;
                        writer.WriteCall(
                            NativeName.SetSoundDistanceCutoff,
                            sound.Name,
                            JassLiteral.Real(distanceCutoff));
                    }

                    if ((int)sound.Channel != -1)
                    {
                        var channel = sound.Channel == SoundChannel.Undefined ? SoundChannel.General : sound.Channel;
                        writer.WriteCall(
                            NativeName.SetSoundChannel,
                            sound.Name,
                            JassLiteral.Int((int)channel));
                    }

                    writer.WriteCall(
                        NativeName.SetSoundVolume,
                        sound.Name,
                        JassLiteral.Int(sound.Volume == -1 ? 127 : sound.Volume));

                    writer.WriteCall(
                        NativeName.SetSoundPitch,
                        sound.Name,
                        JassLiteral.Real(sound.Pitch == uint.MaxValue ? 1f : sound.Pitch));

                    if (is3DSound)
                    {
                        writer.WriteCall(
                            NativeName.SetSoundDistances,
                            sound.Name,
                            JassLiteral.Real(sound.MinDistance == uint.MaxValue ? 0f : sound.MinDistance),
                            JassLiteral.Real(sound.MaxDistance == uint.MaxValue ? 10000f : sound.MaxDistance));

                        writer.WriteCall(
                            NativeName.SetSoundConeAngles,
                            sound.Name,
                            JassLiteral.Real(sound.ConeAngleInside == uint.MaxValue ? 0f : sound.ConeAngleInside),
                            JassLiteral.Real(sound.ConeAngleOutside == uint.MaxValue ? 0f : sound.ConeAngleOutside),
                            JassLiteral.Int(sound.ConeOutsideVolume == -1 ? 127 : sound.ConeOutsideVolume));

                        writer.WriteCall(
                            NativeName.SetSoundConeOrientation,
                            sound.Name,
                            JassLiteral.Real(sound.ConeOrientation.X == uint.MaxValue ? 0f : sound.ConeOrientation.X),
                            JassLiteral.Real(sound.ConeOrientation.Y == uint.MaxValue ? 0f : sound.ConeOrientation.Y),
                            JassLiteral.Real(sound.ConeOrientation.Z == uint.MaxValue ? 0f : sound.ConeOrientation.Z));
                    }
                }
            }

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateInitSounds(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Sounds is not null;
        }
    }
}
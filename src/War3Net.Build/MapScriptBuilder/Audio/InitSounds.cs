// ------------------------------------------------------------------------------
// <copyright file="InitSounds.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using War3Net.Build.Audio;
using War3Net.Build.Providers;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual JassFunctionDeclarationSyntax InitSounds(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapSounds = map.Sounds;
            if (mapSounds is null)
            {
                throw new ArgumentException($"Function '{nameof(InitSounds)}' cannot be generated without {nameof(MapSounds)}.", nameof(map));
            }

            var statements = new List<JassStatementSyntax>();

            foreach (var sound in mapSounds.Sounds)
            {
                if (sound.Flags.HasFlag(SoundFlags.Music))
                {
                    statements.Add(SyntaxFactory.SetStatement(
                        sound.Name,
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(EscapedStringProvider.GetEscapedString(sound.FilePath)))));
                }
                else
                {
                    var is3DSound = sound.Flags.HasFlag(SoundFlags.Is3DSound)
                        && sound.Channel != SoundChannel.Error
                        && sound.Channel != SoundChannel.Music
                        && sound.Channel != SoundChannel.UserInterface;

                    statements.Add(SyntaxFactory.SetStatement(
                        sound.Name,
                        SyntaxFactory.InvocationExpression(
                            NativeName.CreateSound,
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(EscapedStringProvider.GetEscapedString(sound.FilePath))),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(sound.Flags.HasFlag(SoundFlags.Looping))),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(is3DSound)),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(sound.Flags.HasFlag(SoundFlags.StopWhenOutOfRange))),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(sound.FadeInRate)),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(sound.FadeOutRate)),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(EscapedStringProvider.GetEscapedString(sound.EaxSetting))))));

                    if (!string.IsNullOrEmpty(sound.FacialAnimationLabel))
                    {
                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetSoundFacialAnimationLabel,
                            SyntaxFactory.ParseIdentifierName(sound.Name),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(sound.FacialAnimationLabel))));
                    }

                    if (!string.IsNullOrEmpty(sound.FacialAnimationGroupLabel))
                    {
                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetSoundFacialAnimationGroupLabel,
                            SyntaxFactory.ParseIdentifierName(sound.Name),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(sound.FacialAnimationGroupLabel))));
                    }

                    if (!string.IsNullOrEmpty(sound.FacialAnimationSetFilepath))
                    {
                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetSoundFacialAnimationSetFilepath,
                            SyntaxFactory.ParseIdentifierName(sound.Name),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(sound.FacialAnimationSetFilepath))));
                    }

                    if (sound.DialogueSpeakerNameKey > 0)
                    {
                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetDialogueSpeakerNameKey,
                            SyntaxFactory.ParseIdentifierName(sound.Name),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal($"TRIGSTR_{sound.DialogueSpeakerNameKey}"))));
                    }

                    if (sound.DialogueTextKey > 0)
                    {
                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetDialogueTextKey,
                            SyntaxFactory.ParseIdentifierName(sound.Name),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal($"TRIGSTR_{sound.DialogueTextKey}"))));
                    }

                    if (sound.DistanceCutoff != 3000f)
                    {
                        var distanceCutoff = sound.DistanceCutoff == uint.MaxValue ? 3000f : sound.DistanceCutoff;
                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetSoundDistanceCutoff,
                            SyntaxFactory.ParseIdentifierName(sound.Name),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(distanceCutoff, precision: 1))));
                    }

                    if ((int)sound.Channel != -1)
                    {
                        var channel = sound.Channel == SoundChannel.Undefined ? SoundChannel.General : sound.Channel;
                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetSoundChannel,
                            SyntaxFactory.ParseIdentifierName(sound.Name),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal((int)channel))));
                    }

                    statements.Add(SyntaxFactory.CallStatement(
                        NativeName.SetSoundVolume,
                        SyntaxFactory.ParseIdentifierName(sound.Name),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(sound.Volume == -1 ? 127 : sound.Volume))));

                    statements.Add(SyntaxFactory.CallStatement(
                        NativeName.SetSoundPitch,
                        SyntaxFactory.ParseIdentifierName(sound.Name),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(sound.Pitch == uint.MaxValue ? 1f : sound.Pitch, precision: 1))));

                    if (is3DSound)
                    {
                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetSoundDistances,
                            SyntaxFactory.ParseIdentifierName(sound.Name),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(sound.MinDistance == uint.MaxValue ? 0f : sound.MinDistance, precision: 1)),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(sound.MaxDistance == uint.MaxValue ? 10000f : sound.MaxDistance, precision: 1))));

                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetSoundConeAngles,
                            SyntaxFactory.ParseIdentifierName(sound.Name),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(sound.ConeAngleInside == uint.MaxValue ? 0f : sound.ConeAngleInside, precision: 1)),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(sound.ConeAngleOutside == uint.MaxValue ? 0f : sound.ConeAngleOutside, precision: 1)),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(sound.ConeOutsideVolume == -1 ? 127 : sound.ConeOutsideVolume))));

                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetSoundConeOrientation,
                            SyntaxFactory.ParseIdentifierName(sound.Name),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(sound.ConeOrientation.X == uint.MaxValue ? 0f : sound.ConeOrientation.X, precision: 1)),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(sound.ConeOrientation.Y == uint.MaxValue ? 0f : sound.ConeOrientation.Y, precision: 1)),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(sound.ConeOrientation.Z == uint.MaxValue ? 0f : sound.ConeOrientation.Z, precision: 1))));
                    }
                }
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(InitSounds)), statements);
        }

        protected internal virtual bool InitSoundsCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Sounds is not null;
        }
    }
}
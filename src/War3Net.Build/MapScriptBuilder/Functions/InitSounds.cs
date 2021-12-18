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

            var mapInfo = map.Info;

            var statements = new List<IStatementSyntax>();

            foreach (var sound in mapSounds.Sounds)
            {
                if (sound.Flags.HasFlag(SoundFlags.Music))
                {
                    statements.Add(SyntaxFactory.SetStatement(
                        sound.Name,
                        SyntaxFactory.LiteralExpression(EscapedStringProvider.GetEscapedString(sound.FilePath))));
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
                            SyntaxFactory.LiteralExpression(EscapedStringProvider.GetEscapedString(sound.FilePath)),
                            SyntaxFactory.LiteralExpression(sound.Flags.HasFlag(SoundFlags.Looping)),
                            SyntaxFactory.LiteralExpression(is3DSound),
                            SyntaxFactory.LiteralExpression(sound.Flags.HasFlag(SoundFlags.StopWhenOutOfRange)),
                            SyntaxFactory.LiteralExpression(sound.FadeInRate),
                            SyntaxFactory.LiteralExpression(sound.FadeOutRate),
                            SyntaxFactory.LiteralExpression(EscapedStringProvider.GetEscapedString(sound.EaxSetting)))));

                    if (sound.DistanceCutoff != 3000f)
                    {
                        var distanceCutoff = sound.DistanceCutoff == uint.MaxValue ? 3000f : sound.DistanceCutoff;
                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetSoundDistanceCutoff,
                            SyntaxFactory.VariableReferenceExpression(sound.Name),
                            SyntaxFactory.LiteralExpression(distanceCutoff, precision: 1)));
                    }

                    if ((int)sound.Channel != -1)
                    {
                        var channel = sound.Channel == SoundChannel.Undefined ? SoundChannel.General : sound.Channel;
                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetSoundChannel,
                            SyntaxFactory.VariableReferenceExpression(sound.Name),
                            SyntaxFactory.LiteralExpression((int)channel)));
                    }

                    statements.Add(SyntaxFactory.CallStatement(
                        NativeName.SetSoundVolume,
                        SyntaxFactory.VariableReferenceExpression(sound.Name),
                        SyntaxFactory.LiteralExpression(sound.Volume == -1 ? 127 : sound.Volume)));

                    statements.Add(SyntaxFactory.CallStatement(
                        NativeName.SetSoundPitch,
                        SyntaxFactory.VariableReferenceExpression(sound.Name),
                        SyntaxFactory.LiteralExpression(sound.Pitch == uint.MaxValue ? 1f : sound.Pitch, precision: 1)));

                    if (is3DSound)
                    {
                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetSoundDistances,
                            SyntaxFactory.VariableReferenceExpression(sound.Name),
                            SyntaxFactory.LiteralExpression(sound.MinDistance == uint.MaxValue ? 0f : sound.MinDistance, precision: 1),
                            SyntaxFactory.LiteralExpression(sound.MaxDistance == uint.MaxValue ? 10000f : sound.MaxDistance, precision: 1)));

                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetSoundConeAngles,
                            SyntaxFactory.VariableReferenceExpression(sound.Name),
                            SyntaxFactory.LiteralExpression(sound.ConeAngleInside == uint.MaxValue ? 0f : sound.ConeAngleInside, precision: 1),
                            SyntaxFactory.LiteralExpression(sound.ConeAngleOutside == uint.MaxValue ? 0f : sound.ConeAngleOutside, precision: 1),
                            SyntaxFactory.LiteralExpression(sound.ConeOutsideVolume == -1 ? 127 : sound.ConeOutsideVolume)));

                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.SetSoundConeOrientation,
                            SyntaxFactory.VariableReferenceExpression(sound.Name),
                            SyntaxFactory.LiteralExpression(sound.ConeOrientation.X == uint.MaxValue ? 0f : sound.ConeOrientation.X, precision: 1),
                            SyntaxFactory.LiteralExpression(sound.ConeOrientation.Y == uint.MaxValue ? 0f : sound.ConeOrientation.Y, precision: 1),
                            SyntaxFactory.LiteralExpression(sound.ConeOrientation.Z == uint.MaxValue ? 0f : sound.ConeOrientation.Z, precision: 1)));
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
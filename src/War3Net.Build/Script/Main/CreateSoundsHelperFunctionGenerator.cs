// ------------------------------------------------------------------------------
// <copyright file="CreateSoundsHelperFunctionGenerator.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.Build.Audio;

namespace War3Net.Build.Script.Main
{
    internal static partial class MainFunctionGenerator<TBuilder, TGlobalDeclarationSyntax, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
    {
        private static TFunctionSyntax GenerateCreateSoundsHelperFunction(TBuilder builder)
        {
            return builder.Build("InitSounds", GetCreateSoundsHelperFunctionStatements(builder));
        }

        private static IEnumerable<TStatementSyntax> GetCreateSoundsHelperFunctionStatements(TBuilder builder)
        {
            foreach (var sound in builder.Data.MapSounds)
            {
                var is3DSound = sound.Flags.HasFlag(SoundFlags.Is3DSound)
                    && sound.Channel != SoundChannel.Error
                    && sound.Channel != SoundChannel.Music
                    && sound.Channel != SoundChannel.UserInterface;

                yield return builder.GenerateAssignmentStatement(
                    sound.Name,
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.CreateSound),
                        builder.GenerateStringLiteralExpression(sound.FilePath),
                        builder.GenerateBooleanLiteralExpression(sound.Flags.HasFlag(SoundFlags.Looping)),
                        builder.GenerateBooleanLiteralExpression(is3DSound),
                        builder.GenerateBooleanLiteralExpression(is3DSound ? sound.Flags.HasFlag(SoundFlags.StopWhenOutOfRange) : false),
                        builder.GenerateIntegerLiteralExpression(sound.FadeInRate),
                        builder.GenerateIntegerLiteralExpression(sound.FadeOutRate),
                        builder.GenerateStringLiteralExpression(sound.EaxSetting)));

                // var hasNonDefaultFields = false;

                // if ((int)sound.Channel != -1)
                {
                    // hasNonDefaultFields = true;
                    var channel = (int)sound.Channel == -1 ? 0 : (int)sound.Channel;
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.SetSoundChannel),
                        builder.GenerateVariableExpression(sound.Name),
                        builder.GenerateIntegerLiteralExpression(channel));
                }

                // if (sound.Volume != -1)
                {
                    // hasNonDefaultFields = true;
                    var volume = sound.Volume == -1 ? 127 : sound.Volume;
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.SetSoundVolume),
                        builder.GenerateVariableExpression(sound.Name),
                        builder.GenerateIntegerLiteralExpression(volume));
                }

                // if (sound.Pitch != uint.MaxValue)
                {
                    // hasNonDefaultFields = true;
                    var pitch = sound.Pitch == uint.MaxValue ? 1f : sound.Pitch;
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.SetSoundPitch),
                        builder.GenerateVariableExpression(sound.Name),
                        builder.GenerateFloatLiteralExpression(pitch));
                }

                if (is3DSound)
                {
                    // if (sound.MinDistance != uint.MaxValue && sound.MaxDistance != uint.MaxValue)
                    {
                        // hasNonDefaultFields = true;
                        var minDistance = sound.MinDistance == uint.MaxValue ? 0f : sound.MinDistance;
                        var maxDistance = sound.MaxDistance == uint.MaxValue ? 10000f : sound.MaxDistance;
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.SetSoundDistances),
                            builder.GenerateVariableExpression(sound.Name),
                            builder.GenerateFloatLiteralExpression(minDistance),
                            builder.GenerateFloatLiteralExpression(maxDistance));
                    }

                    // if (sound.DistanceCutoff != uint.MaxValue)
                    {
                        // hasNonDefaultFields = true;
                        var distanceCutoff = sound.DistanceCutoff == uint.MaxValue ? 3000f : sound.DistanceCutoff;
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.SetSoundDistanceCutoff),
                            builder.GenerateVariableExpression(sound.Name),
                            builder.GenerateFloatLiteralExpression(distanceCutoff));
                    }

                    // if (sound.ConeAngleInside != uint.MaxValue && sound.ConeAngleOutside != uint.MaxValue && sound.ConeOutsideVolume != -1)
                    {
                        // hasNonDefaultFields = true;
                        var angleInside = sound.ConeAngleInside == uint.MaxValue ? 0f : sound.ConeAngleInside;
                        var angleOutside = sound.ConeAngleOutside == uint.MaxValue ? 0f : sound.ConeAngleOutside;
                        var outsideVolume = sound.ConeOutsideVolume == -1 ? 127 : sound.ConeOutsideVolume;
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.SetSoundConeAngles),
                            builder.GenerateVariableExpression(sound.Name),
                            builder.GenerateFloatLiteralExpression(angleInside),
                            builder.GenerateFloatLiteralExpression(angleOutside),
                            builder.GenerateIntegerLiteralExpression(outsideVolume));
                    }

                    // if (sound.ConeOrientation.X != uint.MaxValue && sound.ConeOrientation.Y != uint.MaxValue && sound.ConeOrientation.Z != uint.MaxValue)
                    {
                        // hasNonDefaultFields = true;
                        var orientationX = sound.ConeOrientation.X == uint.MaxValue ? 0f : sound.ConeOrientation.X;
                        var orientationY = sound.ConeOrientation.Y == uint.MaxValue ? 0f : sound.ConeOrientation.Y;
                        var orientationZ = sound.ConeOrientation.Z == uint.MaxValue ? 0f : sound.ConeOrientation.Z;
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.SetSoundConeOrientation),
                            builder.GenerateVariableExpression(sound.Name),
                            builder.GenerateFloatLiteralExpression(orientationX),
                            builder.GenerateFloatLiteralExpression(orientationY),
                            builder.GenerateFloatLiteralExpression(orientationZ));
                    }
                }

                // TODO: get soundParams label (and duration?) given filepath by reading .slk files (at UI/SoundInfo)...
                // currently using default values for imported files instead
                /*if (!hasNonDefaultFields)
                {
                    throw new NotSupportedException("Unable to create sound using SetSoundParamsFromLabel.");

                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.SetSoundParamsFromLabel),
                        builder.GenerateVariableExpression(sound.Name),
                        builder.GenerateStringLiteralExpression("???"));

                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.SetSoundDuration),
                        builder.GenerateVariableExpression(sound.Name),
                        builder.GenerateInvocationExpression(
                            nameof(War3Api.Common.GetSoundFileDuration),
                            builder.GenerateStringLiteralExpression(sound.FilePath)));
                }*/
            }
        }
    }
}
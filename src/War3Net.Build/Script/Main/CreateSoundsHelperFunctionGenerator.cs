// ------------------------------------------------------------------------------
// <copyright file="CreateSoundsHelperFunctionGenerator.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
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

                var hasNonDefaultFields = false;

                if ((int)sound.Channel != -1)
                {
                    hasNonDefaultFields = true;
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.SetSoundChannel),
                        builder.GenerateVariableExpression(sound.Name),
                        builder.GenerateIntegerLiteralExpression((int)sound.Channel));
                }

                if (sound.Volume != -1)
                {
                    hasNonDefaultFields = true;
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.SetSoundVolume),
                        builder.GenerateVariableExpression(sound.Name),
                        builder.GenerateIntegerLiteralExpression(sound.Volume));
                }

                if (sound.Pitch != uint.MaxValue)
                {
                    hasNonDefaultFields = true;
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.SetSoundPitch),
                        builder.GenerateVariableExpression(sound.Name),
                        builder.GenerateFloatLiteralExpression(sound.Pitch));
                }

                if (is3DSound)
                {
                    if (sound.MinDistance != uint.MaxValue && sound.MaxDistance != uint.MaxValue)
                    {
                        hasNonDefaultFields = true;
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.SetSoundDistances),
                            builder.GenerateVariableExpression(sound.Name),
                            builder.GenerateFloatLiteralExpression(sound.MinDistance),
                            builder.GenerateFloatLiteralExpression(sound.MaxDistance));
                    }

                    if (sound.DistanceCutoff != uint.MaxValue)
                    {
                        hasNonDefaultFields = true;
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.SetSoundDistanceCutoff),
                            builder.GenerateVariableExpression(sound.Name),
                            builder.GenerateFloatLiteralExpression(sound.DistanceCutoff));
                    }

                    if (sound.ConeAngleInside != uint.MaxValue && sound.ConeAngleOutside != uint.MaxValue && sound.ConeOutsideVolume != -1)
                    {
                        hasNonDefaultFields = true;
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.SetSoundConeAngles),
                            builder.GenerateVariableExpression(sound.Name),
                            builder.GenerateFloatLiteralExpression(sound.ConeAngleInside),
                            builder.GenerateFloatLiteralExpression(sound.ConeAngleOutside),
                            builder.GenerateIntegerLiteralExpression(sound.ConeOutsideVolume));
                    }

                    if (sound.ConeOrientation.X != uint.MaxValue && sound.ConeOrientation.Y != uint.MaxValue && sound.ConeOrientation.Z != uint.MaxValue)
                    {
                        hasNonDefaultFields = true;
                        yield return builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.SetSoundConeOrientation),
                            builder.GenerateVariableExpression(sound.Name),
                            builder.GenerateFloatLiteralExpression(sound.ConeOrientation.X),
                            builder.GenerateFloatLiteralExpression(sound.ConeOrientation.Y),
                            builder.GenerateFloatLiteralExpression(sound.ConeOrientation.Z));
                    }
                }

                if (!hasNonDefaultFields)
                {
                    // TODO: how to get soundParams label (and duration) given filepath?
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
                }
            }
        }
    }
}
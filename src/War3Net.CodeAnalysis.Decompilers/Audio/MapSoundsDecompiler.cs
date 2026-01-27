// ------------------------------------------------------------------------------
// <copyright file="MapSoundsDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using War3Net.Build.Audio;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        public bool TryDecompileMapSounds(MapSoundsFormatVersion formatVersion, [NotNullWhen(true)] out MapSounds? mapSounds)
        {
            foreach (var candidateFunction in GetCandidateFunctions("InitSounds"))
            {
                if (TryDecompileMapSounds(candidateFunction.FunctionDeclaration, formatVersion, out mapSounds))
                {
                    candidateFunction.Handled = true;

                    return true;
                }
            }

            mapSounds = null;
            return false;
        }

        public bool TryDecompileMapSounds(JassFunctionDeclarationSyntax functionDeclaration, MapSoundsFormatVersion formatVersion, [NotNullWhen(true)] out MapSounds? mapSounds)
        {
            if (functionDeclaration is null)
            {
                throw new ArgumentNullException(nameof(functionDeclaration));
            }

            var sounds = new Dictionary<string, Sound>(StringComparer.Ordinal);

            foreach (var statement in functionDeclaration.Statements)
            {
                if (statement is JassSetStatementSyntax setStatement)
                {
                    if (setStatement.ElementAccessClause is null &&
                        setStatement.IdentifierName.Token.Text.StartsWith("gg_snd_", StringComparison.Ordinal))
                    {
                        if (setStatement.EqualsValueClause.Expression is JassInvocationExpressionSyntax invocationExpression &&
                            string.Equals(invocationExpression.IdentifierName.Token.Text, "CreateSound", StringComparison.Ordinal))
                        {
                            if (invocationExpression.ArgumentList.Arguments.Items.Length == 7 &&
                                invocationExpression.ArgumentList.Arguments.Items[0].TryGetNotNullStringExpressionValue(out var fileName) &&
                                invocationExpression.ArgumentList.Arguments.Items[1].TryGetBooleanExpressionValue(out var looping) &&
                                invocationExpression.ArgumentList.Arguments.Items[2].TryGetBooleanExpressionValue(out var is3D) &&
                                invocationExpression.ArgumentList.Arguments.Items[3].TryGetBooleanExpressionValue(out var stopWhenOutOfRange) &&
                                invocationExpression.ArgumentList.Arguments.Items[4].TryGetIntegerExpressionValue(out var fadeInRate) &&
                                invocationExpression.ArgumentList.Arguments.Items[5].TryGetIntegerExpressionValue(out var fadeOutRate) &&
                                invocationExpression.ArgumentList.Arguments.Items[6].TryGetNotNullStringExpressionValue(out var eaxSetting))
                            {
                                var flags = (SoundFlags)0;
                                if (looping)
                                {
                                    flags |= SoundFlags.Looping;
                                }

                                if (is3D)
                                {
                                    flags |= SoundFlags.Is3DSound;
                                }

                                if (stopWhenOutOfRange)
                                {
                                    flags |= SoundFlags.StopWhenOutOfRange;
                                }

                                var filePath = Regex.Unescape(fileName);
                                Context.ImportedFileNames.Add(filePath);

                                if (!is3D && !IsInternalSound(filePath))
                                {
                                    flags |= SoundFlags.UNK16;
                                }

                                sounds.Add(setStatement.IdentifierName.Token.Text, new Sound
                                {
                                    Name = setStatement.IdentifierName.Token.Text,
                                    FilePath = filePath,
                                    EaxSetting = eaxSetting,
                                    Flags = flags,
                                    FadeInRate = fadeInRate,
                                    FadeOutRate = fadeOutRate,

                                    DialogueTextKey = -1,
                                    DialogueSpeakerNameKey = -1,
                                    FacialAnimationLabel = string.Empty,
                                    FacialAnimationGroupLabel = string.Empty,
                                    FacialAnimationSetFilepath = string.Empty,
                                });
                            }
                            else
                            {
                                mapSounds = null;
                                return false;
                            }
                        }
                        else if (setStatement.EqualsValueClause.Expression.TryGetNotNullStringExpressionValue(out var musicFileName))
                        {
                            var flags = SoundFlags.Music;

                            var filePath = Regex.Unescape(musicFileName);
                            Context.ImportedFileNames.Add(filePath);

                            if (!IsInternalSound(filePath))
                            {
                                flags |= SoundFlags.UNK16;
                            }

                            sounds.Add(setStatement.IdentifierName.Token.Text, new Sound
                            {
                                Name = setStatement.IdentifierName.Token.Text,
                                FilePath = filePath,
                                EaxSetting = string.Empty,
                                Flags = flags,
                                FadeInRate = 10,
                                FadeOutRate = 10,
                            });
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (statement is JassCallStatementSyntax callStatement)
                {
                    if (string.Equals(callStatement.IdentifierName.Token.Text, "SetSoundParamsFromLabel", StringComparison.Ordinal))
                    {
                        continue;
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "SetSoundFacialAnimationLabel", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.Arguments.Items.Length == 2 &&
                            callStatement.ArgumentList.Arguments.Items[0].TryGetIdentifierNameValue(out var variableName) &&
                            callStatement.ArgumentList.Arguments.Items[1].TryGetNotNullStringExpressionValue(out var facialAnimationLabel) &&
                            sounds.TryGetValue(variableName, out var sound))
                        {
                            sound.FacialAnimationLabel = facialAnimationLabel;
                        }
                        else
                        {
                            mapSounds = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "SetSoundFacialAnimationGroupLabel", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.Arguments.Items.Length == 2 &&
                            callStatement.ArgumentList.Arguments.Items[0].TryGetIdentifierNameValue(out var variableName) &&
                            callStatement.ArgumentList.Arguments.Items[1].TryGetNotNullStringExpressionValue(out var facialAnimationGroupLabel) &&
                            sounds.TryGetValue(variableName, out var sound))
                        {
                            sound.FacialAnimationGroupLabel = facialAnimationGroupLabel;
                        }
                        else
                        {
                            mapSounds = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "SetSoundFacialAnimationSetFilepath", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.Arguments.Items.Length == 2 &&
                            callStatement.ArgumentList.Arguments.Items[0].TryGetIdentifierNameValue(out var variableName) &&
                            callStatement.ArgumentList.Arguments.Items[1].TryGetNotNullStringExpressionValue(out var facialAnimationSetFilepath) &&
                            sounds.TryGetValue(variableName, out var sound))
                        {
                            sound.FacialAnimationSetFilepath = facialAnimationSetFilepath;
                        }
                        else
                        {
                            mapSounds = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "SetDialogueSpeakerNameKey", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.Arguments.Items.Length == 2 &&
                            callStatement.ArgumentList.Arguments.Items[0].TryGetIdentifierNameValue(out var variableName) &&
                            callStatement.ArgumentList.Arguments.Items[1].TryGetNotNullStringExpressionValue(out var dialogueSpeakerNameKeyString) &&
                            sounds.TryGetValue(variableName, out var sound) &&
                            dialogueSpeakerNameKeyString.StartsWith("TRIGSTR_", StringComparison.Ordinal) &&
                            int.TryParse(dialogueSpeakerNameKeyString["TRIGSTR_".Length..], NumberStyles.None, CultureInfo.InvariantCulture, out var dialogueSpeakerNameKey))
                        {
                            sound.DialogueSpeakerNameKey = dialogueSpeakerNameKey;
                        }
                        else
                        {
                            mapSounds = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "SetDialogueTextKey", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.Arguments.Items.Length == 2 &&
                            callStatement.ArgumentList.Arguments.Items[0].TryGetIdentifierNameValue(out var variableName) &&
                            callStatement.ArgumentList.Arguments.Items[1].TryGetNotNullStringExpressionValue(out var dialogueTextKeyString) &&
                            sounds.TryGetValue(variableName, out var sound) &&
                            dialogueTextKeyString.StartsWith("TRIGSTR_", StringComparison.Ordinal) &&
                            int.TryParse(dialogueTextKeyString["TRIGSTR_".Length..], NumberStyles.None, CultureInfo.InvariantCulture, out var dialogueTextKey))
                        {
                            sound.DialogueTextKey = dialogueTextKey;
                        }
                        else
                        {
                            mapSounds = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "SetSoundDuration", StringComparison.Ordinal))
                    {
                        continue;
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "SetSoundDistanceCutoff", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.Arguments.Items.Length == 2 &&
                            callStatement.ArgumentList.Arguments.Items[0].TryGetIdentifierNameValue(out var variableName) &&
                            callStatement.ArgumentList.Arguments.Items[1].TryGetRealExpressionValue(out var cutoff) &&
                            sounds.TryGetValue(variableName, out var sound))
                        {
                            sound.DistanceCutoff = cutoff;
                        }
                        else
                        {
                            mapSounds = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "SetSoundChannel", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.Arguments.Items.Length == 2 &&
                            callStatement.ArgumentList.Arguments.Items[0].TryGetIdentifierNameValue(out var variableName) &&
                            callStatement.ArgumentList.Arguments.Items[1].TryGetIntegerExpressionValue(out var channel) &&
                            sounds.TryGetValue(variableName, out var sound))
                        {
                            sound.Channel = (SoundChannel)channel;
                        }
                        else
                        {
                            mapSounds = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "SetSoundVolume", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.Arguments.Items.Length == 2 &&
                            callStatement.ArgumentList.Arguments.Items[0].TryGetIdentifierNameValue(out var variableName) &&
                            callStatement.ArgumentList.Arguments.Items[1].TryGetIntegerExpressionValue(out var volume) &&
                            sounds.TryGetValue(variableName, out var sound))
                        {
                            sound.Volume = volume;
                        }
                        else
                        {
                            mapSounds = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "SetSoundPitch", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.Arguments.Items.Length == 2 &&
                            callStatement.ArgumentList.Arguments.Items[0].TryGetIdentifierNameValue(out var variableName) &&
                            callStatement.ArgumentList.Arguments.Items[1].TryGetRealExpressionValue(out var pitch) &&
                            sounds.TryGetValue(variableName, out var sound))
                        {
                            sound.Pitch = pitch;
                        }
                        else
                        {
                            mapSounds = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "SetSoundDistances", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.Arguments.Items.Length == 3 &&
                            callStatement.ArgumentList.Arguments.Items[0].TryGetIdentifierNameValue(out var variableName) &&
                            callStatement.ArgumentList.Arguments.Items[1].TryGetRealExpressionValue(out var minDist) &&
                            callStatement.ArgumentList.Arguments.Items[2].TryGetRealExpressionValue(out var maxDist) &&
                            sounds.TryGetValue(variableName, out var sound) &&
                            sound.Flags.HasFlag(SoundFlags.Is3DSound))
                        {
                            sound.MinDistance = minDist;
                            sound.MaxDistance = maxDist;
                        }
                        else
                        {
                            mapSounds = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "SetSoundConeAngles", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.Arguments.Items.Length == 4 &&
                            callStatement.ArgumentList.Arguments.Items[0].TryGetIdentifierNameValue(out var variableName) &&
                            callStatement.ArgumentList.Arguments.Items[1].TryGetRealExpressionValue(out var inside) &&
                            callStatement.ArgumentList.Arguments.Items[2].TryGetRealExpressionValue(out var outside) &&
                            callStatement.ArgumentList.Arguments.Items[3].TryGetIntegerExpressionValue(out var outsideVolume) &&
                            sounds.TryGetValue(variableName, out var sound) &&
                            sound.Flags.HasFlag(SoundFlags.Is3DSound))
                        {
                            sound.ConeAngleInside = inside;
                            sound.ConeAngleOutside = outside;
                            sound.ConeOutsideVolume = outsideVolume;
                        }
                        else
                        {
                            mapSounds = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "SetSoundConeOrientation", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.Arguments.Items.Length == 4 &&
                            callStatement.ArgumentList.Arguments.Items[0].TryGetIdentifierNameValue(out var variableName) &&
                            callStatement.ArgumentList.Arguments.Items[1].TryGetRealExpressionValue(out var x) &&
                            callStatement.ArgumentList.Arguments.Items[2].TryGetRealExpressionValue(out var y) &&
                            callStatement.ArgumentList.Arguments.Items[3].TryGetRealExpressionValue(out var z) &&
                            sounds.TryGetValue(variableName, out var sound) &&
                            sound.Flags.HasFlag(SoundFlags.Is3DSound))
                        {
                            sound.ConeOrientation = new(x, y, z);
                        }
                        else
                        {
                            mapSounds = null;
                            return false;
                        }
                    }
                    else
                    {
                        mapSounds = null;
                        return false;
                    }
                }
                else
                {
                    mapSounds = null;
                    return false;
                }
            }

            if (sounds.Count > 0)
            {
                mapSounds = new MapSounds(formatVersion);
                mapSounds.Sounds.AddRange(sounds.Values);
                return true;
            }

            mapSounds = null;
            return false;
        }

        [Obsolete]
        private static bool IsInternalSound(string filePath)
        {
            return filePath.StartsWith(@"Sound\", StringComparison.OrdinalIgnoreCase)
                || filePath.StartsWith(@"Sound/", StringComparison.OrdinalIgnoreCase)
                || filePath.StartsWith(@"UI\", StringComparison.OrdinalIgnoreCase)
                || filePath.StartsWith(@"UI/", StringComparison.OrdinalIgnoreCase)
                || filePath.StartsWith(@"Units\", StringComparison.OrdinalIgnoreCase)
                || filePath.StartsWith(@"Units/", StringComparison.OrdinalIgnoreCase);
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="MapSoundsDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

            foreach (var statement in functionDeclaration.Body.Statements)
            {
                if (statement is JassSetStatementSyntax setStatement)
                {
                    if (setStatement.Indexer is null &&
                        setStatement.IdentifierName.Name.StartsWith("gg_snd_", StringComparison.Ordinal))
                    {
                        if (setStatement.Value.Expression is JassInvocationExpressionSyntax invocationExpression &&
                            string.Equals(invocationExpression.IdentifierName.Name, "CreateSound", StringComparison.Ordinal))
                        {
                            if (invocationExpression.Arguments.Arguments.Length == 7 &&
                                invocationExpression.Arguments.Arguments[0] is JassStringLiteralExpressionSyntax fileNameLiteralExpression &&
                                invocationExpression.Arguments.Arguments[1] is JassBooleanLiteralExpressionSyntax loopingLiteralExpression &&
                                invocationExpression.Arguments.Arguments[2] is JassBooleanLiteralExpressionSyntax is3DLiteralExpression &&
                                invocationExpression.Arguments.Arguments[3] is JassBooleanLiteralExpressionSyntax stopWhenOutOfRangeLiteralExpression &&
                                invocationExpression.Arguments.Arguments[4].TryGetIntegerExpressionValue(out var fadeInRate) &&
                                invocationExpression.Arguments.Arguments[5].TryGetIntegerExpressionValue(out var fadeOutRate) &&
                                invocationExpression.Arguments.Arguments[6] is JassStringLiteralExpressionSyntax eaxSettingLiteralExpression)
                            {
                                var flags = (SoundFlags)0;
                                if (loopingLiteralExpression.Value)
                                {
                                    flags |= SoundFlags.Looping;
                                }

                                if (is3DLiteralExpression.Value)
                                {
                                    flags |= SoundFlags.Is3DSound;
                                }

                                if (stopWhenOutOfRangeLiteralExpression.Value)
                                {
                                    flags |= SoundFlags.StopWhenOutOfRange;
                                }

                                var filePath = Regex.Unescape(fileNameLiteralExpression.Value);
                                Context.ImportedFileNames.Add(filePath);

                                if (!is3DLiteralExpression.Value && !IsInternalSound(filePath))
                                {
                                    flags |= SoundFlags.UNK16;
                                }

                                sounds.Add(setStatement.IdentifierName.Name, new Sound
                                {
                                    Name = setStatement.IdentifierName.Name,
                                    FilePath = filePath,
                                    EaxSetting = eaxSettingLiteralExpression.Value,
                                    Flags = flags,
                                    FadeInRate = fadeInRate,
                                    FadeOutRate = fadeOutRate,
                                });
                            }
                            else
                            {
                                mapSounds = null;
                                return false;
                            }
                        }
                        else if (setStatement.Value.Expression is JassStringLiteralExpressionSyntax stringLiteralExpression)
                        {
                            var flags = SoundFlags.Music;

                            var filePath = Regex.Unescape(stringLiteralExpression.Value);
                            Context.ImportedFileNames.Add(filePath);

                            if (!IsInternalSound(filePath))
                            {
                                flags |= SoundFlags.UNK16;
                            }

                            sounds.Add(setStatement.IdentifierName.Name, new Sound
                            {
                                Name = setStatement.IdentifierName.Name,
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
                    if (string.Equals(callStatement.IdentifierName.Name, "SetSoundParamsFromLabel", StringComparison.Ordinal) ||
                        string.Equals(callStatement.IdentifierName.Name, "SetSoundDuration", StringComparison.Ordinal))
                    {
                        continue;
                    }
                    else if (string.Equals(callStatement.IdentifierName.Name, "SetSoundDistanceCutoff", StringComparison.Ordinal))
                    {
                        if (callStatement.Arguments.Arguments.Length == 2 &&
                            callStatement.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax variableReferenceExpression &&
                            callStatement.Arguments.Arguments[1].TryGetRealExpressionValue(out var cutoff) &&
                            sounds.TryGetValue(variableReferenceExpression.IdentifierName.Name, out var sound))
                        {
                            sound.DistanceCutoff = cutoff;
                        }
                        else
                        {
                            mapSounds = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Name, "SetSoundChannel", StringComparison.Ordinal))
                    {
                        if (callStatement.Arguments.Arguments.Length == 2 &&
                            callStatement.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax variableReferenceExpression &&
                            callStatement.Arguments.Arguments[1].TryGetIntegerExpressionValue(out var channel) &&
                            sounds.TryGetValue(variableReferenceExpression.IdentifierName.Name, out var sound))
                        {
                            sound.Channel = (SoundChannel)channel;
                        }
                        else
                        {
                            mapSounds = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Name, "SetSoundVolume", StringComparison.Ordinal))
                    {
                        if (callStatement.Arguments.Arguments.Length == 2 &&
                            callStatement.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax variableReferenceExpression &&
                            callStatement.Arguments.Arguments[1].TryGetIntegerExpressionValue(out var volume) &&
                            sounds.TryGetValue(variableReferenceExpression.IdentifierName.Name, out var sound))
                        {
                            sound.Volume = volume;
                        }
                        else
                        {
                            mapSounds = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Name, "SetSoundPitch", StringComparison.Ordinal))
                    {
                        if (callStatement.Arguments.Arguments.Length == 2 &&
                            callStatement.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax variableReferenceExpression &&
                            callStatement.Arguments.Arguments[1].TryGetRealExpressionValue(out var pitch) &&
                            sounds.TryGetValue(variableReferenceExpression.IdentifierName.Name, out var sound))
                        {
                            sound.Pitch = pitch;
                        }
                        else
                        {
                            mapSounds = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Name, "SetSoundDistances", StringComparison.Ordinal))
                    {
                        if (callStatement.Arguments.Arguments.Length == 3 &&
                            callStatement.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax variableReferenceExpression &&
                            callStatement.Arguments.Arguments[1].TryGetRealExpressionValue(out var minDist) &&
                            callStatement.Arguments.Arguments[2].TryGetRealExpressionValue(out var maxDist) &&
                            sounds.TryGetValue(variableReferenceExpression.IdentifierName.Name, out var sound) &&
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
                    else if (string.Equals(callStatement.IdentifierName.Name, "SetSoundConeAngles", StringComparison.Ordinal))
                    {
                        if (callStatement.Arguments.Arguments.Length == 4 &&
                            callStatement.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax variableReferenceExpression &&
                            callStatement.Arguments.Arguments[1].TryGetRealExpressionValue(out var inside) &&
                            callStatement.Arguments.Arguments[2].TryGetRealExpressionValue(out var outside) &&
                            callStatement.Arguments.Arguments[3].TryGetIntegerExpressionValue(out var outsideVolume) &&
                            sounds.TryGetValue(variableReferenceExpression.IdentifierName.Name, out var sound) &&
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
                    else if (string.Equals(callStatement.IdentifierName.Name, "SetSoundConeOrientation", StringComparison.Ordinal))
                    {
                        if (callStatement.Arguments.Arguments.Length == 4 &&
                            callStatement.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax variableReferenceExpression &&
                            callStatement.Arguments.Arguments[1].TryGetRealExpressionValue(out var x) &&
                            callStatement.Arguments.Arguments[2].TryGetRealExpressionValue(out var y) &&
                            callStatement.Arguments.Arguments[3].TryGetRealExpressionValue(out var z) &&
                            sounds.TryGetValue(variableReferenceExpression.IdentifierName.Name, out var sound) &&
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

            if (sounds.Any())
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
// ------------------------------------------------------------------------------
// 
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// 
// ------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

using War3Net.Build.Audio;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        [RegisterStatementParser]
        internal void ParseSoundCreation(StatementParserInput input)
        {
            var variableAssignment = GetVariableAssignment(input.StatementChildren);
            if (variableAssignment == null || !variableAssignment.StartsWith("gg_snd_"))
            {
                return;
            }

            var sound = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "CreateSound")
            .SafeMapFirst(x =>
            {
                var filePath = Regex.Unescape(((JassStringLiteralExpressionSyntax)x.Arguments.Arguments[0]).Value);
                Context.ImportedFileNames.Add(filePath);

                return new Sound
                {
                    FilePath = filePath,
                    Flags = ParseSoundFlags(x.Arguments.Arguments, filePath),
                    FadeInRate = x.Arguments.Arguments[4].GetValueOrDefault<int>(),
                    FadeOutRate = x.Arguments.Arguments[5].GetValueOrDefault<int>(),
                    EaxSetting = ((JassStringLiteralExpressionSyntax)x.Arguments.Arguments[6]).Value,
                    DialogueTextKey = -1,
                    DialogueSpeakerNameKey = -1
                };
            });

            if (sound != null)
            {
                sound.Name = variableAssignment;
                Context.Add(sound, variableAssignment);
                Context.HandledStatements.Add(input.Statement);
            }
        }

        [RegisterStatementParser]
        internal void ParseSetSoundParamsFromLabel(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "SetSoundParamsFromLabel")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    SoundName = (x.Arguments.Arguments[1] as JassStringLiteralExpressionSyntax)?.Value,
                };
            });

            if (match != null)
            {
                var sound = Context.Get<Sound>(match.VariableName) ?? Context.GetLastCreated<Sound>();
                if (sound != null)
                {
                    sound.SoundName = match.SoundName;
                    Context.HandledStatements.Add(input.Statement);
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseSetSoundFacialAnimationLabel(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "SetSoundFacialAnimationLabel")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    FacialAnimationLabel = (x.Arguments.Arguments[1] as JassStringLiteralExpressionSyntax)?.Value,
                };
            });

            if (match != null)
            {
                var sound = Context.Get<Sound>(match.VariableName) ?? Context.GetLastCreated<Sound>();
                if (sound != null)
                {
                    sound.FacialAnimationLabel = match.FacialAnimationLabel;
                    Context.HandledStatements.Add(input.Statement);
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseSetSoundFacialAnimationGroupLabel(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "SetSoundFacialAnimationGroupLabel")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    FacialAnimationGroupLabel = (x.Arguments.Arguments[1] as JassStringLiteralExpressionSyntax)?.Value,
                };
            });

            if (match != null)
            {
                var sound = Context.Get<Sound>(match.VariableName) ?? Context.GetLastCreated<Sound>();
                if (sound != null)
                {
                    sound.FacialAnimationGroupLabel = match.FacialAnimationGroupLabel;
                    Context.HandledStatements.Add(input.Statement);
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseSetSoundFacialAnimationSetFilepath(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "SetSoundFacialAnimationSetFilepath")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    FacialAnimationSetFilepath = (x.Arguments.Arguments[1] as JassStringLiteralExpressionSyntax)?.Value,
                };
            });

            if (match != null)
            {
                var sound = Context.Get<Sound>(match.VariableName) ?? Context.GetLastCreated<Sound>();
                if (sound != null)
                {
                    sound.FacialAnimationSetFilepath = match.FacialAnimationSetFilepath;
                    Context.HandledStatements.Add(input.Statement);
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseSetDialogueSpeakerNameKey(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "SetDialogueSpeakerNameKey")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    DialogueSpeakerNameKey_TriggerString = (x.Arguments.Arguments[1] as JassStringLiteralExpressionSyntax).Value,
                };
            });

            if (match != null)
            {
                var sound = Context.Get<Sound>(match.VariableName) ?? Context.GetLastCreated<Sound>();
                if (sound != null)
                {
                    if (match.DialogueSpeakerNameKey_TriggerString.StartsWith("TRIGSTR_", StringComparison.Ordinal) && int.TryParse(match.DialogueSpeakerNameKey_TriggerString["TRIGSTR_".Length..], NumberStyles.None, CultureInfo.InvariantCulture, out var dialogueSpeakerNameKey))
                    {
                        sound.DialogueSpeakerNameKey = dialogueSpeakerNameKey;
                        Context.HandledStatements.Add(input.Statement);
                    }
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseSetDialogueTextKey(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "SetDialogueTextKey")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    DialogueTextKey_TriggerString = (x.Arguments.Arguments[1] as JassStringLiteralExpressionSyntax).Value,
                };
            });

            if (match != null)
            {
                var sound = Context.Get<Sound>(match.VariableName) ?? Context.GetLastCreated<Sound>();
                if (sound != null)
                {
                    if (match.DialogueTextKey_TriggerString.StartsWith("TRIGSTR_", StringComparison.Ordinal) && int.TryParse(match.DialogueTextKey_TriggerString["TRIGSTR_".Length..], NumberStyles.None, CultureInfo.InvariantCulture, out var dialogueTextKey))
                    {
                        sound.DialogueTextKey = dialogueTextKey;
                        Context.HandledStatements.Add(input.Statement);
                    }
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseSetSoundDistanceCutoff(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "SetSoundDistanceCutoff")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    DistanceCutoff = x.Arguments.Arguments[1].GetValueOrDefault<float>(),
                };
            });

            if (match != null)
            {
                var sound = Context.Get<Sound>(match.VariableName) ?? Context.GetLastCreated<Sound>();
                if (sound != null)
                {
                    sound.DistanceCutoff = match.DistanceCutoff;
                    Context.HandledStatements.Add(input.Statement);
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseSetSoundChannel(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "SetSoundChannel")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    Channel = (SoundChannel)x.Arguments.Arguments[1].GetValueOrDefault<int>(),
                };
            });

            if (match != null)
            {
                var sound = Context.Get<Sound>(match.VariableName) ?? Context.GetLastCreated<Sound>();
                if (sound != null)
                {
                    sound.Channel = match.Channel;
                    Context.HandledStatements.Add(input.Statement);
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseSetSoundVolume(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "SetSoundVolume")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    Volume = x.Arguments.Arguments[1].GetValueOrDefault<int>(),
                };
            });

            if (match != null)
            {
                var sound = Context.Get<Sound>(match.VariableName) ?? Context.GetLastCreated<Sound>();
                if (sound != null)
                {
                    sound.Volume = match.Volume;
                    Context.HandledStatements.Add(input.Statement);
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseSetSoundPitch(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "SetSoundPitch")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    Pitch = x.Arguments.Arguments[1].GetValueOrDefault<float>(),
                };
            });

            if (match != null)
            {
                var sound = Context.Get<Sound>(match.VariableName) ?? Context.GetLastCreated<Sound>();
                if (sound != null)
                {
                    sound.Pitch = match.Pitch;
                    Context.HandledStatements.Add(input.Statement);
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseSetSoundDistances(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "SetSoundDistances")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    MinDistance = x.Arguments.Arguments[1].GetValueOrDefault<float>(),
                    MaxDistance = x.Arguments.Arguments[2].GetValueOrDefault<float>()
                };
            });

            if (match != null)
            {
                var sound = Context.Get<Sound>(match.VariableName) ?? Context.GetLastCreated<Sound>();
                if (sound != null)
                {
                    sound.MinDistance = match.MinDistance;
                    sound.MaxDistance = match.MaxDistance;
                    Context.HandledStatements.Add(input.Statement);
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseSetSoundConeAngles(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "SetSoundConeAngles")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    ConeAngleInside = x.Arguments.Arguments[1].GetValueOrDefault<float>(),
                    ConeAngleOutside = x.Arguments.Arguments[2].GetValueOrDefault<float>(),
                    ConeOutsideVolume = x.Arguments.Arguments[3].GetValueOrDefault<int>()
                };
            });

            if (match != null)
            {
                var sound = Context.Get<Sound>(match.VariableName) ?? Context.GetLastCreated<Sound>();
                if (sound != null && sound.Flags.HasFlag(SoundFlags.Is3DSound))
                {
                    sound.ConeAngleInside = match.ConeAngleInside;
                    sound.ConeAngleOutside = match.ConeAngleOutside;
                    sound.ConeOutsideVolume = match.ConeOutsideVolume;
                    Context.HandledStatements.Add(input.Statement);
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseSetSoundConeOrientation(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "SetSoundConeOrientation")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    x = x.Arguments.Arguments[1].GetValueOrDefault<float>(),
                    y = x.Arguments.Arguments[2].GetValueOrDefault<float>(),
                    z = x.Arguments.Arguments[3].GetValueOrDefault<float>(),
                };
            });

            if (match != null)
            {
                var sound = Context.Get<Sound>(match.VariableName) ?? Context.GetLastCreated<Sound>();
                if (sound != null && sound.Flags.HasFlag(SoundFlags.Is3DSound))
                {
                    sound.ConeOrientation = new(match.x, match.y, match.z);
                    Context.HandledStatements.Add(input.Statement);
                }
            }
        }
    }
}
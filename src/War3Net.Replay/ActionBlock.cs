// ------------------------------------------------------------------------------
// <copyright file="ActionBlock.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using War3Net.Common.Extensions;
using War3Net.Replay.Action;

namespace War3Net.Replay
{
    public /*abstract*/ class ActionBlock
    {
        // temp: add offset to figure out size of unknown action blocks
        public static ActionBlock Parse(Stream data, long offset)
        {
            return Parse(data, (ActionBlockType)data.ReadByte(), offset);
        }

        public static ActionBlock Parse(Stream data, ActionBlockType actionBlockType, long offset)
        {
            switch (actionBlockType)
            {
                case ActionBlockType.Pause:
                case ActionBlockType.Resume:
                    return null;

                case ActionBlockType.SetGameSpeed:
                    data.Seek(1, SeekOrigin.Current);
                    return null;

                case ActionBlockType.SaveGame:
                    using (var reader = new BinaryReader(data, new UTF8Encoding(false, true), true))
                    {
                        reader.ReadChars();
                    }

                    return null;

                case ActionBlockType.SaveGameFinished:
                    data.Seek(4, SeekOrigin.Current);
                    return null;

                case ActionBlockType.AbilityNoTarget:
                    data.Seek(14, SeekOrigin.Current);
                    return null;

                case ActionBlockType.AbilityLocationTarget:
                    data.Seek(21, SeekOrigin.Current);
                    return null;

                case ActionBlockType.AbilityWidgetTarget:
                    data.Seek(29, SeekOrigin.Current);
                    return null;

                case ActionBlockType.ItemDropOrGive:
                    data.Seek(37, SeekOrigin.Current);
                    return null;

                case ActionBlockType.AbilityTwoTarget:
                    data.Seek(42, SeekOrigin.Current);
                    return null;

                case ActionBlockType.ChangeSelection:
                case ActionBlockType.AssignGroupHotkey:
                    data.Seek(1, SeekOrigin.Current);
                    data.Seek(data.ReadWordAsInt() * 8, SeekOrigin.Current);
                    return null;

                case ActionBlockType.SelectGroupHotkey:
                    data.Seek(2, SeekOrigin.Current);
                    return null;

                case ActionBlockType.SelectSubgroup:
                    data.Seek(12, SeekOrigin.Current);
                    return null;

                case ActionBlockType.UNK1B:
                case ActionBlockType.SelectGroundItem:
                    data.Seek(9, SeekOrigin.Current);
                    return null;

                case ActionBlockType.CancelHeroRevival:
                case ActionBlockType.UNK21:
                    data.Seek(8, SeekOrigin.Current);
                    return null;

                case ActionBlockType.RemoveUnitFromBuildingQueue:
                    data.Seek(5, SeekOrigin.Current);
                    return null;

                case (ActionBlockType)0x27:
                case (ActionBlockType)0x28:
                case (ActionBlockType)0x2D:
                    data.Seek(5, SeekOrigin.Current);
                    return null;

                case (ActionBlockType)0x2E:
                    data.Seek(4, SeekOrigin.Current);
                    return null;

                case ActionBlockType.ChangeAllyOptions:
                    data.Seek(5, SeekOrigin.Current);
                    return null;

                case ActionBlockType.TransferResources:
                    data.Seek(9, SeekOrigin.Current);
                    return null;

                case ActionBlockType.TriggeredChatCommand:
                    data.Seek(8, SeekOrigin.Current);
                    using (var reader = new BinaryReader(data, new UTF8Encoding(false, true), true))
                    {
                        reader.ReadChars();
                    }

                    return null;

                case ActionBlockType.ScenarioTrigger:
                    data.Seek(12, SeekOrigin.Current);
                    return null;

                case ActionBlockType.PingMinimap:
                    data.Seek(12, SeekOrigin.Current);
                    return null;

                case ActionBlockType.ContinueGameA:
                case ActionBlockType.ContinueGameB:
                    data.Seek(16, SeekOrigin.Current);
                    return null;

                case ActionBlockType.SyncInteger:
                    return new SyncIntegerEvent(data);

                case ActionBlockType.SyncReal:
                    return new SyncRealEvent(data);

                case ActionBlockType.SyncBoolean:
                    return new SyncBooleanEvent(data);

                case ActionBlockType.SyncUnit:
                    return new SyncUnitEvent(data);

                case ActionBlockType.SyncStringNoData:
                    return null;

                case ActionBlockType.SyncString:
                    throw new NotImplementedException();
                    // return new SyncStringEvent(data);

                case ActionBlockType.UNK75:
                    data.Seek(1, SeekOrigin.Current);
                    return null;

                case ActionBlockType.KeyboardEvent:
                    return new KeyboardEventBlock(data);

                case ActionBlockType.UNK7B:
                    data.Seek(16, SeekOrigin.Current);
                    return null;

                default:
                    if (!Enum.IsDefined(typeof(ActionBlockType), actionBlockType))
                    {
                        var futureBytes = new List<string>();
                        while (data.Position < offset)
                        {
                            futureBytes.Add(((ActionBlockType)data.ReadByte()).ToString());
                            if (futureBytes.Count > 100)
                            {
                                break;
                            }
                        }

                        throw new InvalidDataException($"Unknown action block type: {actionBlockType: X}\n{futureBytes.Aggregate((accum, next) => $"{accum}\n{next}")}.");
                    }

                    return null;
            }
        }
    }
}
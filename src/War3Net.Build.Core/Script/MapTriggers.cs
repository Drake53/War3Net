// ------------------------------------------------------------------------------
// <copyright file="MapTriggers.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    // https://github.com/stijnherfst/HiveWE/wiki/war3map.wtg-Triggers
    // http://www.wc3c.net/tools/specs/index.html
    public sealed class MapTriggers
    {
        public const string FileName = "war3map.wtg";

        internal static readonly int FileFormatHeader = "WTG!".FromRawcode();

        private const int NewFormatId = unchecked((int)0x80000004);

        private readonly List<TriggerItem> _triggerItems;
        private readonly List<VariableDefinition> _variables;

        private MapTriggersFormatVersion _version;
        private bool _newFormat;
        private int _gameVersion;

        private MapTriggers()
        {
            _triggerItems = new List<TriggerItem>();
            _variables = new List<VariableDefinition>();
        }

        public static bool IsRequired => false;

        public MapTriggersFormatVersion FormatVersion
        {
            get => _version;
            set => _version = value;
        }

        public bool UseNewFormat
        {
            get => _newFormat;
            set => _newFormat = value;
        }

        public int GameVersion
        {
            get => _gameVersion;
            set => _gameVersion = value;
        }

        public static MapTriggers Parse(Stream stream, bool leaveOpen = false) => Parse(stream, TriggerData.Default, leaveOpen);

        public static MapTriggers Parse(Stream stream, TriggerData triggerData, bool leaveOpen = false)
        {
            try
            {
                var triggers = new MapTriggers();
                using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
                {
                    var header = reader.ReadInt32();
                    if (header != FileFormatHeader)
                    {
                        throw new InvalidDataException($"Expected file header signature at the start of .wtg file.");
                    }

                    triggers._version = (MapTriggersFormatVersion)reader.ReadInt32();
                    if (!Enum.IsDefined(typeof(MapTriggersFormatVersion), triggers._version))
                    {
                        if ((int)triggers._version != NewFormatId)
                        {
                            throw new NotSupportedException($"Unknown version of '{FileName}': {triggers._version}");
                        }

                        triggers._version = reader.ReadInt32<MapTriggersFormatVersion>();
                        triggers._newFormat = true;

                        var triggerItemCounts = new Dictionary<TriggerItemType, int>();

                        triggerItemCounts[TriggerItemType.RootCategory] = reader.ReadInt32();
                        var countDeletedRootCategory = reader.ReadInt32();
                        for (var i = 0; i < countDeletedRootCategory; i++)
                        {
                            triggers._triggerItems.Add(DeletedTriggerItem.Parse(stream, TriggerItemType.RootCategory, true));
                        }

                        triggerItemCounts[TriggerItemType.UNK2] = reader.ReadInt32();
                        var countDeletedUNK2 = reader.ReadInt32();
                        for (var i = 0; i < countDeletedUNK2; i++)
                        {
                            triggers._triggerItems.Add(DeletedTriggerItem.Parse(stream, TriggerItemType.UNK2, true));
                        }

                        triggerItemCounts[TriggerItemType.Category] = reader.ReadInt32();
                        var countDeletedCategory = reader.ReadInt32();
                        for (var i = 0; i < countDeletedCategory; i++)
                        {
                            triggers._triggerItems.Add(DeletedTriggerItem.Parse(stream, TriggerItemType.Category, true));
                        }

                        triggerItemCounts[TriggerItemType.Gui] = reader.ReadInt32();
                        var countDeletedGui = reader.ReadInt32();
                        for (var i = 0; i < countDeletedGui; i++)
                        {
                            triggers._triggerItems.Add(DeletedTriggerItem.Parse(stream, TriggerItemType.Gui, true));
                        }

                        triggerItemCounts[TriggerItemType.Comment] = reader.ReadInt32();
                        var countDeletedComment = reader.ReadInt32();
                        for (var i = 0; i < countDeletedComment; i++)
                        {
                            triggers._triggerItems.Add(DeletedTriggerItem.Parse(stream, TriggerItemType.Comment, true));
                        }

                        triggerItemCounts[TriggerItemType.Script] = reader.ReadInt32();
                        var countDeletedScript = reader.ReadInt32();
                        for (var i = 0; i < countDeletedScript; i++)
                        {
                            triggers._triggerItems.Add(DeletedTriggerItem.Parse(stream, TriggerItemType.Script, true));
                        }

                        triggerItemCounts[TriggerItemType.Variable] = reader.ReadInt32();
                        var countDeletedVariable = reader.ReadInt32();
                        for (var i = 0; i < countDeletedVariable; i++)
                        {
                            triggers._triggerItems.Add(DeletedTriggerItem.Parse(stream, TriggerItemType.Variable, true));
                        }

                        triggerItemCounts[TriggerItemType.UNK128] = reader.ReadInt32();
                        var countDeletedUNK128 = reader.ReadInt32();
                        for (var i = 0; i < countDeletedUNK128; i++)
                        {
                            triggers._triggerItems.Add(DeletedTriggerItem.Parse(stream, TriggerItemType.UNK128, true));
                        }

                        triggers._gameVersion = reader.ReadInt32();

                        var variableDefinitionCount = reader.ReadInt32();
                        for (var i = 0; i < variableDefinitionCount; i++)
                        {
                            triggers._variables.Add(VariableDefinition.Parse(stream, triggers._version, true, true));
                        }

                        var itemCount = reader.ReadInt32();
                        for (var i = 0; i < itemCount; i++)
                        {
                            var type = reader.ReadInt32<TriggerItemType>();
                            switch (type)
                            {
                                case TriggerItemType.RootCategory:
                                case TriggerItemType.Category:
                                    triggers._triggerItems.Add(TriggerCategoryDefinition.Parse(stream, triggers._version, type, true));
                                    break;

                                case TriggerItemType.Gui:
                                case TriggerItemType.Comment:
                                case TriggerItemType.Script:
                                    triggers._triggerItems.Add(TriggerDefinition.Parse(stream, triggerData, triggers._version, type, true));
                                    break;

                                case TriggerItemType.Variable:
                                    triggers._triggerItems.Add(TriggerVariableDefinition.Parse(stream, triggers._version, true));
                                    break;

                                case TriggerItemType.UNK2:
                                case TriggerItemType.UNK128:
                                    throw new NotSupportedException();

                                default:
                                    throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(TriggerItemType));
                            }
                        }

                        foreach (TriggerItemType type in Enum.GetValues(typeof(TriggerItemType)))
                        {
                            var count = triggers._triggerItems.Count(item => item.ItemType == type);
                            if (count > triggerItemCounts[type])
                            {
                                throw new InvalidDataException($"Expected {triggerItemCounts[type]} trigger items of type {type}, but got {count}.");
                            }

                            while (count < triggerItemCounts[type])
                            {
                                triggers._triggerItems.Add(new DeletedTriggerItem(type));
                                count++;
                            }
                        }
                    }
                    else
                    {
                        var triggerCategoryDefinitionCount = reader.ReadInt32();
                        for (var i = 0; i < triggerCategoryDefinitionCount; i++)
                        {
                            triggers._triggerItems.Add(TriggerCategoryDefinition.Parse(stream, triggers._version, null, true));
                        }

                        triggers._gameVersion = reader.ReadInt32();

                        var variableDefinitionCount = reader.ReadInt32();
                        for (var i = 0; i < variableDefinitionCount; i++)
                        {
                            triggers._variables.Add(VariableDefinition.Parse(stream, triggers._version, false, true));
                        }

                        var triggerDefinitionCount = reader.ReadInt32();
                        for (var i = 0; i < triggerDefinitionCount; i++)
                        {
                            triggers._triggerItems.Add(TriggerDefinition.Parse(stream, triggerData, triggers._version, null, true));
                        }
                    }
                }

                return triggers;
            }
            catch (DecoderFallbackException e)
            {
                throw new InvalidDataException($"The '{FileName}' file contains invalid characters.", e);
            }
            catch (EndOfStreamException e)
            {
                throw new InvalidDataException($"The '{FileName}' file is missing data, or its data is invalid.", e);
            }
            catch
            {
                throw;
            }
        }

        public static void Serialize(MapTriggers mapTriggers, Stream stream, bool leaveOpen = false)
        {
            mapTriggers.SerializeTo(stream, leaveOpen);
        }

        public void SerializeTo(Stream stream, bool leaveOpen = false)
        {
            using (var writer = new BinaryWriter(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                writer.Write(FileFormatHeader);

                if (_newFormat)
                {
                    writer.Write(NewFormatId);
                    writer.Write((int)_version);

                    foreach (TriggerItemType type in Enum.GetValues(typeof(TriggerItemType)))
                    {
                        writer.Write(_triggerItems.Count(item => item.ItemType == type));

                        var deletedItems = _triggerItems.Where(item => item is DeletedTriggerItem && item.ItemType == type && item.Id != -1).ToList();
                        writer.Write(deletedItems.Count);
                        foreach (var deletedItem in deletedItems)
                        {
                            deletedItem.WriteTo(writer, _version, true);
                        }
                    }

                    writer.Write(_gameVersion);

                    writer.Write(_variables.Count);
                    foreach (var variable in _variables)
                    {
                        variable.WriteTo(writer, _version, true);
                    }

                    writer.Write(_triggerItems.Count(item => item is not DeletedTriggerItem));
                    foreach (var triggerItem in _triggerItems)
                    {
                        if (triggerItem is DeletedTriggerItem)
                        {
                            continue;
                        }

                        writer.Write((int)triggerItem.ItemType);
                        triggerItem.WriteTo(writer, _version, true);
                    }
                }
                else
                {
                    writer.Write((int)_version);

                    var triggerCategories = _triggerItems.Where(item => item is TriggerCategoryDefinition && item.ItemType != TriggerItemType.RootCategory).ToArray();
                    writer.Write(triggerCategories.Length);
                    foreach (var triggerCategory in triggerCategories)
                    {
                        triggerCategory.WriteTo(writer, _version, false);
                    }

                    writer.Write(_gameVersion);

                    writer.Write(_variables.Count);
                    foreach (var variable in _variables)
                    {
                        variable.WriteTo(writer, _version, false);
                    }

                    var triggers = _triggerItems.Where(item => item is TriggerDefinition).ToArray();
                    writer.Write(triggers.Length);
                    foreach (var trigger in triggers)
                    {
                        trigger.WriteTo(writer, _version, false);
                    }
                }
            }
        }
    }
}
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

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    // https://github.com/stijnherfst/HiveWE/wiki/war3map.wtg-Triggers
    // http://www.wc3c.net/tools/specs/index.html
    public sealed class MapTriggers
    {
        public const string FileName = "war3map.wtg";

        public static readonly int FileFormatSignature = "WTG!".FromRawcode();

        /// <summary>
        /// Initializes a new instance of the <see cref="MapTriggers"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        /// <param name="subVersion"></param>
        public MapTriggers(MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            FormatVersion = formatVersion;
            SubVersion = subVersion;
        }

        internal MapTriggers(BinaryReader reader, TriggerData triggerData)
        {
            ReadFrom(reader, triggerData);
        }

        public MapTriggersFormatVersion FormatVersion { get; set; }

        public MapTriggersSubVersion? SubVersion { get; set; }

        public int GameVersion { get; set; }

        public List<VariableDefinition> Variables { get; init; } = new();

        public List<TriggerItem> TriggerItems { get; init; } = new();

        public override string ToString() => FileName;

        internal void ReadFrom(BinaryReader reader, TriggerData triggerData)
        {
            var header = reader.ReadInt32();
            if (header != FileFormatSignature)
            {
                throw new InvalidDataException($"Expected file header signature at the start of .wtg file.");
            }

            var version = reader.ReadInt32();
            if (Enum.IsDefined(typeof(MapTriggersFormatVersion), version))
            {
                FormatVersion = (MapTriggersFormatVersion)version;
                SubVersion = null;

                nint triggerCategoryDefinitionCount = reader.ReadInt32();
                for (nint i = 0; i < triggerCategoryDefinitionCount; i++)
                {
                    TriggerItems.Add(reader.ReadTriggerCategoryDefinition(TriggerItemType.Category, triggerData, FormatVersion, SubVersion));
                }

                GameVersion = reader.ReadInt32();

                nint variableDefinitionCount = reader.ReadInt32();
                for (nint i = 0; i < variableDefinitionCount; i++)
                {
                    Variables.Add(reader.ReadVariableDefinition(triggerData, FormatVersion, SubVersion));
                }

                nint triggerDefinitionCount = reader.ReadInt32();
                for (nint i = 0; i < triggerDefinitionCount; i++)
                {
                    TriggerItems.Add(reader.ReadTriggerDefinition(TriggerItemType.Gui, triggerData, FormatVersion, SubVersion));
                }
            }
            else if (Enum.IsDefined(typeof(MapTriggersSubVersion), version))
            {
                FormatVersion = reader.ReadInt32<MapTriggersFormatVersion>();
                SubVersion = (MapTriggersSubVersion)version;

                var triggerItemCounts = new Dictionary<TriggerItemType, int>();
                foreach (TriggerItemType triggerItemType in Enum.GetValues(typeof(TriggerItemType)))
                {
                    triggerItemCounts[triggerItemType] = reader.ReadInt32();
                    nint deletedTriggerItemCount = reader.ReadInt32();
                    for (nint i = 0; i < deletedTriggerItemCount; i++)
                    {
                        TriggerItems.Add(reader.ReadDeletedTriggerItem(triggerItemType, triggerData, FormatVersion, SubVersion));
                    }
                }

                GameVersion = reader.ReadInt32();

                nint variableDefinitionCount = reader.ReadInt32();
                for (nint i = 0; i < variableDefinitionCount; i++)
                {
                    Variables.Add(reader.ReadVariableDefinition(triggerData, FormatVersion, SubVersion));
                }

                nint triggerItemCount = reader.ReadInt32();
                for (nint i = 0; i < triggerItemCount; i++)
                {
                    var triggerItemType = reader.ReadInt32<TriggerItemType>();
                    switch (triggerItemType)
                    {
                        case TriggerItemType.RootCategory:
                        case TriggerItemType.Category:
                            TriggerItems.Add(reader.ReadTriggerCategoryDefinition(triggerItemType, triggerData, FormatVersion, SubVersion));
                            break;

                        case TriggerItemType.Gui:
                        case TriggerItemType.Comment:
                        case TriggerItemType.Script:
                            TriggerItems.Add(reader.ReadTriggerDefinition(triggerItemType, triggerData, FormatVersion, SubVersion));
                            break;

                        case TriggerItemType.Variable:
                            TriggerItems.Add(reader.ReadTriggerVariableDefinition(triggerItemType, triggerData, FormatVersion, SubVersion));
                            break;

                        case TriggerItemType.UNK1:
                        case TriggerItemType.UNK7:
                            throw new NotSupportedException();

                        default:
                            throw new InvalidEnumArgumentException(nameof(triggerItemType), (int)triggerItemType, typeof(TriggerItemType));
                    }
                }

                foreach (TriggerItemType triggerItemType in Enum.GetValues(typeof(TriggerItemType)))
                {
                    var count = TriggerItems.Count(item => item.Type == triggerItemType);
                    if (count > triggerItemCounts[triggerItemType])
                    {
                        throw new InvalidDataException($"Expected {triggerItemCounts[triggerItemType]} trigger items of type {triggerItemType}, but got {count}.");
                    }

                    while (count < triggerItemCounts[triggerItemType])
                    {
                        TriggerItems.Add(new DeletedTriggerItem(triggerItemType));
                        count++;
                    }
                }
            }
            else
            {
                throw new NotSupportedException($"Unknown version of '{FileName}': {version}");
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write(FileFormatSignature);

            if (SubVersion is null)
            {
                writer.Write((int)FormatVersion);

                var triggerCategories = TriggerItems.Where(item => item is TriggerCategoryDefinition && item.Type != TriggerItemType.RootCategory).ToArray();
                writer.Write(triggerCategories.Length);
                foreach (var triggerCategory in triggerCategories)
                {
                    writer.Write(triggerCategory, FormatVersion, SubVersion);
                }

                writer.Write(GameVersion);

                writer.Write(Variables.Count);
                foreach (var variable in Variables)
                {
                    writer.Write(variable, FormatVersion, SubVersion);
                }

                var triggers = TriggerItems.Where(item => item is TriggerDefinition).ToArray();
                writer.Write(triggers.Length);
                foreach (var trigger in triggers)
                {
                    writer.Write(trigger, FormatVersion, SubVersion);
                }
            }
            else
            {
                writer.Write((int)SubVersion);
                writer.Write((int)FormatVersion);

                foreach (TriggerItemType triggerItemType in Enum.GetValues(typeof(TriggerItemType)))
                {
                    writer.Write(TriggerItems.Count(item => item.Type == triggerItemType));

                    var deletedItems = TriggerItems.Where(item => item is DeletedTriggerItem && item.Type == triggerItemType && item.Id != -1).ToList();
                    writer.Write(deletedItems.Count);
                    foreach (var deletedItem in deletedItems)
                    {
                        writer.Write(deletedItem, FormatVersion, SubVersion);
                    }
                }

                writer.Write(GameVersion);

                writer.Write(Variables.Count);
                foreach (var variable in Variables)
                {
                    writer.Write(variable, FormatVersion, SubVersion);
                }

                writer.Write(TriggerItems.Count(item => item is not DeletedTriggerItem));
                foreach (var triggerItem in TriggerItems)
                {
                    if (triggerItem is DeletedTriggerItem)
                    {
                        continue;
                    }

                    writer.Write((int)triggerItem.Type);
                    writer.Write(triggerItem, FormatVersion, SubVersion);
                }
            }
        }
    }
}
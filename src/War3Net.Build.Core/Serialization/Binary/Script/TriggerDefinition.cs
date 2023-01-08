﻿// ------------------------------------------------------------------------------
// <copyright file="TriggerDefinition.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed partial class TriggerDefinition : TriggerItem
    {
        internal TriggerDefinition(BinaryReader reader, TriggerItemType triggerItemType, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
            : base(triggerItemType)
        {
            ReadFrom(reader, triggerData, formatVersion, subVersion);
        }

        internal void ReadFrom(BinaryReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            Name = reader.ReadChars();
            Description = reader.ReadChars();
            if (formatVersion >= MapTriggersFormatVersion.v7)
            {
                IsComment = reader.ReadBool();
            }

            if (subVersion is not null)
            {
                Id = reader.ReadInt32();
            }

            IsEnabled = reader.ReadBool();
            IsCustomTextTrigger = reader.ReadBool();
            IsInitiallyOn = !reader.ReadBool();
            RunOnMapInit = reader.ReadBool();
            ParentId = reader.ReadInt32();

            nint guiFunctionCount = reader.ReadInt32();
            if (IsCustomTextTrigger && guiFunctionCount > 0)
            {
                throw new InvalidDataException($"Custom text trigger should not have any GUI functions.");
            }

            for (nint i = 0; i < guiFunctionCount; i++)
            {
                Functions.Add(reader.ReadTriggerFunction(triggerData, formatVersion, subVersion, false));
            }
        }

        internal override void WriteTo(BinaryWriter writer, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            writer.WriteString(Name);
            writer.WriteString(Description);
            if (formatVersion >= MapTriggersFormatVersion.v7)
            {
                writer.WriteBool(IsComment);
            }

            if (subVersion is not null)
            {
                writer.Write(Id);
            }

            writer.WriteBool(IsEnabled);
            writer.WriteBool(IsCustomTextTrigger);
            writer.WriteBool(!IsInitiallyOn);
            writer.WriteBool(RunOnMapInit);
            writer.Write(ParentId);

            writer.Write(Functions.Count);
            foreach (var function in Functions)
            {
                writer.Write(function, formatVersion, subVersion);
            }
        }
    }
}
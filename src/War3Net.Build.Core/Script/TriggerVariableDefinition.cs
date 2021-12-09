﻿// ------------------------------------------------------------------------------
// <copyright file="TriggerVariableDefinition.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed class TriggerVariableDefinition : TriggerItem
    {
        public TriggerVariableDefinition(TriggerItemType triggerItemType = TriggerItemType.Variable)
            : base(triggerItemType)
        {
        }

        internal TriggerVariableDefinition(BinaryReader reader, TriggerItemType triggerItemType, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
            : base(triggerItemType)
        {
            ReadFrom(reader, triggerData, formatVersion, subVersion);
        }

        internal void ReadFrom(BinaryReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            Id = reader.ReadInt32();
            Name = reader.ReadChars();
            ParentId = reader.ReadInt32();
        }

        internal override void WriteTo(BinaryWriter writer, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            writer.Write(Id);
            writer.WriteString(Name);
            writer.Write(ParentId);
        }
    }
}
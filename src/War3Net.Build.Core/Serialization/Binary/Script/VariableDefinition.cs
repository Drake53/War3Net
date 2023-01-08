// ------------------------------------------------------------------------------
// <copyright file="VariableDefinition.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed partial class VariableDefinition
    {
        internal VariableDefinition(BinaryReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            ReadFrom(reader, triggerData, formatVersion, subVersion);
        }

        internal void ReadFrom(BinaryReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            Name = reader.ReadChars();
            Type = reader.ReadChars();
            Unk = reader.ReadInt32();
            IsArray = reader.ReadBool();
            if (formatVersion >= MapTriggersFormatVersion.v7)
            {
                ArraySize = reader.ReadInt32();
            }

            IsInitialized = reader.ReadBool();
            InitialValue = reader.ReadChars();

            if (subVersion is not null)
            {
                Id = reader.ReadInt32();
                ParentId = reader.ReadInt32();
            }
        }

        internal void WriteTo(BinaryWriter writer, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            writer.WriteString(Name);
            writer.WriteString(Type);
            writer.Write(Unk);
            writer.WriteBool(IsArray);
            if (formatVersion >= MapTriggersFormatVersion.v7)
            {
                writer.Write(ArraySize);
            }

            writer.WriteBool(IsInitialized);
            writer.WriteString(InitialValue);

            if (subVersion is not null)
            {
                writer.Write(Id);
                writer.Write(ParentId);
            }
        }
    }
}
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
    public sealed class VariableDefinition
    {
        internal VariableDefinition(BinaryReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, bool useNewFormat)
        {
            ReadFrom(reader, triggerData, formatVersion, useNewFormat);
        }

        public string Name { get; set; }

        public string Type { get; set; }

        public int Unk { get; set; }

        public bool IsArray { get; set; }

        public int ArraySize { get; set; }

        public bool IsInitialized { get; set; }

        public string InitialValue { get; set; }

        public int Id { get; set; }

        public int ParentId { get; set; }

        public override string ToString()
        {
            return $"{Type} {Name}{(IsArray ? $"[{(ArraySize > 0 ? $"{ArraySize}" : string.Empty)}]" : string.Empty)}{(IsInitialized ? $" = {InitialValue}" : string.Empty)}";
        }

        internal void ReadFrom(BinaryReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, bool useNewFormat)
        {
            Name = reader.ReadChars();
            Type = reader.ReadChars();
            Unk = reader.ReadInt32();
            IsArray = reader.ReadBool();
            if (formatVersion >= MapTriggersFormatVersion.Tft)
            {
                ArraySize = reader.ReadInt32();
            }

            IsInitialized = reader.ReadBool();
            InitialValue = reader.ReadChars();

            if (useNewFormat)
            {
                Id = reader.ReadInt32();
                ParentId = reader.ReadInt32();
            }
        }

        internal void WriteTo(BinaryWriter writer, MapTriggersFormatVersion formatVersion, bool useNewFormat)
        {
            writer.WriteString(Name);
            writer.WriteString(Type);
            writer.Write(Unk);
            writer.WriteBool(IsArray);
            if (formatVersion >= MapTriggersFormatVersion.Tft)
            {
                writer.Write(ArraySize);
            }

            writer.WriteBool(IsInitialized);
            writer.WriteString(InitialValue);

            if (useNewFormat)
            {
                writer.Write(Id);
                writer.Write(ParentId);
            }
        }
    }
}
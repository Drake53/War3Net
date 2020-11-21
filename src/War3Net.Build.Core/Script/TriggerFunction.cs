// ------------------------------------------------------------------------------
// <copyright file="TriggerFunction.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed class TriggerFunction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerFunction"/> class.
        /// </summary>
        public TriggerFunction()
        {
        }

        internal TriggerFunction(BinaryReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, bool useNewFormat, bool isChildFunction)
        {
            ReadFrom(reader, triggerData, formatVersion, useNewFormat, isChildFunction);
        }

        public TriggerFunctionType Type { get; set; }

        public int Branch { get; set; } = -1;

        public string Name { get; set; }

        public bool IsEnabled { get; set; }

        public List<TriggerFunctionParameter> Parameters { get; init; } = new();

        public List<TriggerFunction> ChildFunctions { get; init; } = new();

        public override string ToString()
        {
            return $"{Name}({string.Join(", ", Parameters)})";
        }

        internal void ReadFrom(BinaryReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, bool useNewFormat, bool isChildFunction)
        {
            Type = reader.ReadInt32<TriggerFunctionType>();
            if (isChildFunction)
            {
                Branch = reader.ReadInt32();
            }

            Name = reader.ReadChars();
            IsEnabled = reader.ReadBool();

            var parameterCount = triggerData.GetParameterCount(Type, Name);
            for (var i = 0; i < parameterCount; i++)
            {
                Parameters.Add(reader.ReadTriggerFunctionParameter(triggerData, formatVersion, useNewFormat));
            }

            if (formatVersion >= MapTriggersFormatVersion.Tft)
            {
                var nestedfunctionCount = reader.ReadInt32();
                if (nestedfunctionCount > 0)
                {
                    for (var i = 0; i < nestedfunctionCount; i++)
                    {
                        ChildFunctions.Add(reader.ReadTriggerFunction(triggerData, formatVersion, useNewFormat, true));
                    }
                }
            }
        }

        internal void WriteTo(BinaryWriter writer, MapTriggersFormatVersion formatVersion, bool useNewFormat)
        {
            writer.Write((int)Type);
            if (Branch != -1)
            {
                writer.Write(Branch);
            }

            writer.WriteString(Name);
            writer.WriteBool(IsEnabled);

            foreach (var parameter in Parameters)
            {
                writer.Write(parameter, formatVersion, useNewFormat);
            }

            if (formatVersion >= MapTriggersFormatVersion.Tft)
            {
                writer.Write(ChildFunctions.Count);
                foreach (var childFunction in ChildFunctions)
                {
                    writer.Write(childFunction, formatVersion, useNewFormat);
                }
            }
        }
    }
}
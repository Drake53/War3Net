﻿// ------------------------------------------------------------------------------
// <copyright file="TriggerFunction.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.ComponentModel;
using System.IO;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed partial class TriggerFunction
    {
        internal TriggerFunction(BinaryReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion, bool isChildFunction)
        {
            ReadFrom(reader, triggerData, formatVersion, subVersion, isChildFunction);
        }

        internal void ReadFrom(BinaryReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion, bool isChildFunction)
        {
            Type = reader.ReadInt32<TriggerFunctionType>();
            if (isChildFunction)
            {
                Branch = reader.ReadInt32();
            }

            Name = reader.ReadChars();
            IsEnabled = reader.ReadBool();

            nint parameterCount = Type switch
            {
                TriggerFunctionType.Event => triggerData.TriggerEvents[Name].ArgumentTypes.Length,
                TriggerFunctionType.Condition => triggerData.TriggerConditions[Name].ArgumentTypes.Length,
                TriggerFunctionType.Action => triggerData.TriggerActions[Name].ArgumentTypes.Length,
                TriggerFunctionType.Call => triggerData.TriggerCalls[Name].ArgumentTypes.Length,

                _ => throw new InvalidEnumArgumentException(nameof(Type), (int)Type, typeof(TriggerFunctionType)),
            };

            for (nint i = 0; i < parameterCount; i++)
            {
                Parameters.Add(reader.ReadTriggerFunctionParameter(triggerData, formatVersion, subVersion));
            }

            if (formatVersion >= MapTriggersFormatVersion.v7)
            {
                var nestedfunctionCount = reader.ReadInt32();
                if (nestedfunctionCount > 0)
                {
                    for (nint i = 0; i < nestedfunctionCount; i++)
                    {
                        ChildFunctions.Add(reader.ReadTriggerFunction(triggerData, formatVersion, subVersion, true));
                    }
                }
            }
        }

        internal void WriteTo(BinaryWriter writer, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            writer.Write((int)Type);
            if (Branch.HasValue)
            {
                writer.Write(Branch.Value);
            }

            writer.WriteString(Name);
            writer.WriteBool(IsEnabled);

            foreach (var parameter in Parameters)
            {
                writer.Write(parameter, formatVersion, subVersion);
            }

            if (formatVersion >= MapTriggersFormatVersion.v7)
            {
                writer.Write(ChildFunctions.Count);
                foreach (var childFunction in ChildFunctions)
                {
                    writer.Write(childFunction, formatVersion, subVersion);
                }
            }
        }
    }
}
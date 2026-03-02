using System.IO;
using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed partial class TriggerFunctionParameter
    {
        internal TriggerFunctionParameter(BinaryReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            ReadFrom(reader, triggerData, formatVersion, subVersion);
        }

        internal void ReadFrom(BinaryReader reader, TriggerData triggerData, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            Type = reader.ReadInt32<TriggerFunctionParameterType>();
            Value = reader.ReadChars();

            var haveFunction = reader.ReadBool();
            if (haveFunction)
            {
                if (Type != TriggerFunctionParameterType.Function)
                {
                    throw new InvalidDataException($"Parameter must be of type '{TriggerFunctionParameterType.Function}' to have a function.");
                }

                Function = reader.ReadTriggerFunction(triggerData, formatVersion, subVersion, false);
            }

            var haveArrayIndexer = reader.ReadBool();
            if (haveArrayIndexer)
            {
                if (Type != TriggerFunctionParameterType.Variable)
                {
                    throw new InvalidDataException($"Parameter must be of type '{TriggerFunctionParameterType.Variable}' to have an array indexer.");
                }

                ArrayIndexer = reader.ReadTriggerFunctionParameter(triggerData, formatVersion, subVersion);
            }
        }

        internal void WriteTo(BinaryWriter writer, MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            writer.Write((int)Type);
            writer.WriteString(Value);

            writer.WriteBool(Function is not null);
            Function?.WriteTo(writer, formatVersion, subVersion);

            writer.WriteBool(ArrayIndexer is not null);
            ArrayIndexer?.WriteTo(writer, formatVersion, subVersion);
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="TriggerFunctionParameter.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed class TriggerFunctionParameter
    {
        private TriggerFunctionParameterType _type;
        private string _value;
        private TriggerFunction? _function;
        private TriggerFunctionParameter? _arrayIndexer;

        public static TriggerFunctionParameter Parse(Stream stream, TriggerData triggerData, MapTriggersFormatVersion formatVersion, bool leaveOpen)
        {
            var parameter = new TriggerFunctionParameter();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                parameter._type = reader.ReadInt32<TriggerFunctionParameterType>();
                parameter._value = reader.ReadChars();

                var haveFunction = reader.ReadBool();
                if (haveFunction)
                {
                    if (parameter._type != TriggerFunctionParameterType.Function)
                    {
                        throw new InvalidDataException($"Parameter must be of type '{TriggerFunctionParameterType.Function}' to have a function.");
                    }

                    parameter._function = TriggerFunction.Parse(stream, triggerData, formatVersion, false, true);
                }

                var haveArrayIndexer = reader.ReadBool();
                if (haveArrayIndexer)
                {
                    if (parameter._type != TriggerFunctionParameterType.Variable)
                    {
                        throw new InvalidDataException($"Parameter must be of type '{TriggerFunctionParameterType.Variable}' to have an array indexer.");
                    }

                    parameter._arrayIndexer = Parse(stream, triggerData, formatVersion, true);
                }
            }

            return parameter;
        }

        public void WriteTo(BinaryWriter writer, MapTriggersFormatVersion formatVersion)
        {
            writer.Write((int)_type);
            writer.WriteString(_value);

            writer.WriteBool(_function is not null);
            _function?.WriteTo(writer, formatVersion);

            writer.WriteBool(_arrayIndexer is not null);
            _arrayIndexer?.WriteTo(writer, formatVersion);
        }

        public override string ToString()
        {
            return _type switch
            {
                TriggerFunctionParameterType.Preset => _value,
                TriggerFunctionParameterType.Variable => $"{_value}{(_arrayIndexer is null ? string.Empty : $"[{_arrayIndexer}]")}",
                TriggerFunctionParameterType.Function => _function?.ToString() ?? $"{_value}()",
                TriggerFunctionParameterType.String => $"\"{_value}\"",
                TriggerFunctionParameterType.Undefined => $"{{{_type}}}",
            };
        }
    }
}
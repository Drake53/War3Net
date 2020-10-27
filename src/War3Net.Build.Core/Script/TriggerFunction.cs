// ------------------------------------------------------------------------------
// <copyright file="TriggerFunction.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed class TriggerFunction
    {
        private readonly List<TriggerFunctionParameter> _parameters;
        private readonly List<TriggerFunction> _nestedFunctions;

        private TriggerFunctionType _type;
        private int _branch;
        private string _name;
        private bool _isEnabled;

        private TriggerFunction()
        {
            _parameters = new List<TriggerFunctionParameter>();
            _nestedFunctions = new List<TriggerFunction>();
        }

        public static TriggerFunction Parse(Stream stream, TriggerData triggerData, MapTriggersFormatVersion formatVersion, bool isChildFunction, bool leaveOpen)
        {
            var function = new TriggerFunction();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                function._type = reader.ReadInt32<TriggerFunctionType>();
                function._branch = isChildFunction ? reader.ReadInt32() : -1;
                function._name = reader.ReadChars();
                function._isEnabled = reader.ReadBool();

                var parameterCount = triggerData.GetParameterCount(function._type, function._name);
                for (var i = 0; i < parameterCount; i++)
                {
                    function._parameters.Add(TriggerFunctionParameter.Parse(stream, triggerData, formatVersion, true));
                }

                if (formatVersion >= MapTriggersFormatVersion.Tft)
                {
                    var nestedfunctionCount = reader.ReadInt32();
                    if (nestedfunctionCount > 0)
                    {
                        for (var i = 0; i < nestedfunctionCount; i++)
                        {
                            function._nestedFunctions.Add(Parse(stream, triggerData, formatVersion, true, true));
                        }
                    }
                }
            }

            return function;
        }

        public void WriteTo(BinaryWriter writer, MapTriggersFormatVersion formatVersion)
        {
            writer.Write((int)_type);
            if (_branch != -1)
            {
                writer.Write(_branch);
            }

            writer.WriteString(_name);
            writer.Write(_isEnabled ? 1 : 0);

            foreach (var parameter in _parameters)
            {
                parameter.WriteTo(writer, formatVersion);
            }

            if (formatVersion >= MapTriggersFormatVersion.Tft)
            {
                writer.Write(_nestedFunctions.Count);
                foreach (var nestedFunction in _nestedFunctions)
                {
                    nestedFunction.WriteTo(writer, formatVersion);
                }
            }
        }

        public override string ToString()
        {
            return $"{_name}({string.Join(", ", _parameters)})";
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="VariableDefinition.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed class VariableDefinition
    {
        private string _name;
        private string _type;
        private int _unk;
        private bool _isArray;
        private int _arraySize;
        private bool _isInitialized;
        private string _initialValue;
        private int _id;
        private int _parentId;

        private VariableDefinition()
        {
        }

        public static VariableDefinition Parse(Stream stream, MapTriggersFormatVersion formatVersion, bool useNewFormat, bool leaveOpen)
        {
            var variable = new VariableDefinition();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                variable._name = reader.ReadChars();
                variable._type = reader.ReadChars();
                variable._unk = reader.ReadInt32();
                variable._isArray = reader.ReadBool();
                if (formatVersion >= MapTriggersFormatVersion.Tft)
                {
                    variable._arraySize = reader.ReadInt32();
                }

                variable._isInitialized = reader.ReadBool();
                variable._initialValue = reader.ReadChars();

                if (useNewFormat)
                {
                    variable._id = reader.ReadInt32();
                    variable._parentId = reader.ReadInt32();
                }
            }

            return variable;
        }

        public void WriteTo(BinaryWriter writer, MapTriggersFormatVersion formatVersion, bool useNewFormat)
        {
            writer.WriteString(_name);
            writer.WriteString(_type);
            writer.Write(_unk);
            writer.Write(_isArray ? 1 : 0);
            if (formatVersion >= MapTriggersFormatVersion.Tft)
            {
                writer.Write(_arraySize);
            }

            writer.Write(_isInitialized ? 1 : 0);
            writer.WriteString(_initialValue);

            if (useNewFormat)
            {
                writer.Write(_id);
                writer.Write(_parentId);
            }
        }

        public override string ToString()
        {
            return $"{_type} {_name}{(_isArray ? $"[{(_arraySize > 0 ? $"{_arraySize}" : string.Empty)}]" : string.Empty)}{(_isInitialized ? $" = {_initialValue}" : string.Empty)}";
        }
    }
}
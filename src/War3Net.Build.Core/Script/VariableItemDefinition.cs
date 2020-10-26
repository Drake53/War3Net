// ------------------------------------------------------------------------------
// <copyright file="VariableItemDefinition.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed class VariableItemDefinition : TriggerItem
    {
        private int _id;
        private string _name;
        private int _parentId;

        private VariableItemDefinition()
            : base(TriggerItemType.Variable)
        {
        }

        public override string Name
        {
            get => _name;
            set => _name = value;
        }

        public override int Id
        {
            get => _id;
            set => _id = value;
        }

        public override int ParentId
        {
            get => _parentId;
            set => _parentId = value;
        }

        public static VariableItemDefinition Parse(Stream stream, MapTriggersFormatVersion formatVersion, bool leaveOpen)
        {
            var variable = new VariableItemDefinition();
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                variable._id = reader.ReadInt32();
                variable._name = reader.ReadChars();
                variable._parentId = reader.ReadInt32();
            }

            return variable;
        }

        public override void WriteTo(BinaryWriter writer, MapTriggersFormatVersion formatVersion, bool useNewFormat)
        {
            writer.Write(_id);
            writer.WriteString(_name);
            writer.Write(_parentId);
        }
    }
}
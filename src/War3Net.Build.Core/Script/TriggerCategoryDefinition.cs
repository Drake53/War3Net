// ------------------------------------------------------------------------------
// <copyright file="TriggerCategoryDefinition.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    public sealed class TriggerCategoryDefinition : TriggerItem
    {
        private int _id;
        private string _name;
        private bool _isComment;
        private int _unk;
        private int _parentId;

        private TriggerCategoryDefinition(TriggerItemType type)
            : base(type)
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

        public static TriggerCategoryDefinition Parse(Stream stream, MapTriggersFormatVersion formatVersion, TriggerItemType? type, bool leaveOpen)
        {
            var useNewFormat = type != null;
            var triggerCategory = new TriggerCategoryDefinition(type ?? TriggerItemType.Category);
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                triggerCategory._id = reader.ReadInt32();
                triggerCategory._name = reader.ReadChars();
                if (formatVersion >= MapTriggersFormatVersion.Tft)
                {
                    triggerCategory._isComment = reader.ReadBool();
                }

                if (useNewFormat)
                {
                    triggerCategory._unk = reader.ReadInt32();
                    triggerCategory._parentId = reader.ReadInt32();
                }
            }

            return triggerCategory;
        }

        public override void WriteTo(BinaryWriter writer, MapTriggersFormatVersion formatVersion, bool useNewFormat)
        {
            writer.Write(_id);
            writer.WriteString(_name);
            if (formatVersion >= MapTriggersFormatVersion.Tft)
            {
                writer.WriteBool(_isComment);
            }

            if (useNewFormat)
            {
                writer.Write(_unk);
                writer.Write(_parentId);
            }
        }
    }
}
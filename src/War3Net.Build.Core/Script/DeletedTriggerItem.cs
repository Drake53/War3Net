// ------------------------------------------------------------------------------
// <copyright file="DeletedTriggerItem.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;
using System.Text;

namespace War3Net.Build.Script
{
    public sealed class DeletedTriggerItem : TriggerItem
    {
        private int _id;

        internal DeletedTriggerItem(TriggerItemType type)
            : base(type)
        {
            _id = -1;
        }

        public override string Name
        {
            get => "<DELETED>";
            set => throw new NotSupportedException();
        }

        public override int Id
        {
            get => _id;
            set => _id = value;
        }

        public override int ParentId
        {
            get => -1;
            set => throw new NotSupportedException();
        }

        public static DeletedTriggerItem Parse(Stream stream, TriggerItemType type, bool leaveOpen)
        {
            var item = new DeletedTriggerItem(type);
            using (var reader = new BinaryReader(stream, new UTF8Encoding(false, true), leaveOpen))
            {
                item._id = reader.ReadInt32();
            }

            return item;
        }

        public override void WriteTo(BinaryWriter writer, MapTriggersFormatVersion formatVersion, bool useNewFormat)
        {
            if (useNewFormat)
            {
                writer.Write(_id);
            }
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="ObjectDataModification.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.IO;

using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public abstract class ObjectDataModification
    {
        internal ObjectDataModification()
        {
        }

        public int Id { get; set; }

        public ObjectDataType Type { get; set; }

        public object Value { get; set; }

        internal int SanityCheck { get; set; }

        public int ValueAsInt => Value is int i ? i : throw new InvalidOperationException();

        public float ValueAsFloat => Value is float f ? f : throw new InvalidOperationException();

        public string ValueAsString => Value is string s ? s : throw new InvalidOperationException();

        [Obsolete]
        public bool ValueAsBool => Value is bool b ? b : throw new InvalidOperationException();

        [Obsolete]
        public char ValueAsChar => Value is char c ? c : throw new InvalidOperationException();

        public override string ToString() => Id.ToRawcode();

        protected object ReadValue(BinaryReader reader, ObjectDataFormatVersion formatVersion)
        {
            return Type switch
            {
                ObjectDataType.Int => reader.ReadInt32(),
                ObjectDataType.Real => reader.ReadSingle(),
                ObjectDataType.Unreal => reader.ReadSingle(),
                ObjectDataType.String => reader.ReadChars(),
                ObjectDataType.Bool => reader.ReadBoolean(),
                ObjectDataType.Char => reader.ReadChar(),

                _ => throw new InvalidEnumArgumentException(nameof(Type), (int)Type, typeof(ObjectDataType)),
            };
        }

        protected void WriteValue(BinaryWriter writer, ObjectDataFormatVersion formatVersion)
        {
            switch (Type)
            {
                case ObjectDataType.Int:
                    writer.Write((int)Value);
                    break;

                case ObjectDataType.Real:
                case ObjectDataType.Unreal:
                    writer.Write((float)Value);
                    break;

                case ObjectDataType.String:
                    writer.WriteString((string)Value);
                    break;

                case ObjectDataType.Bool:
                    writer.Write((bool)Value);
                    break;

                case ObjectDataType.Char:
                    writer.Write((char)Value);
                    break;

                default: throw new InvalidEnumArgumentException(nameof(Type), (int)Type, typeof(ObjectDataType));
            }
        }
    }
}
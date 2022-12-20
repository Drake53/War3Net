// ------------------------------------------------------------------------------
// <copyright file="ObjectDataModification.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.ComponentModel;
using System.IO;

using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public abstract partial class ObjectDataModification
    {
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
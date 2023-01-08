// ------------------------------------------------------------------------------
// <copyright file="ObjectDataModification.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Text.Json;

using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public abstract partial class ObjectDataModification
    {
        protected object GetValue(JsonElement jsonElement, ReadOnlySpan<char> propertyName, ObjectDataFormatVersion formatVersion)
        {
            return Type switch
            {
                ObjectDataType.Int => jsonElement.GetInt32(propertyName),
                ObjectDataType.Real => jsonElement.GetSingle(propertyName),
                ObjectDataType.Unreal => jsonElement.GetSingle(propertyName),
                ObjectDataType.String => jsonElement.GetString(propertyName),
                ObjectDataType.Bool => throw new NotSupportedException(),
                ObjectDataType.Char => throw new NotSupportedException(),

                _ => throw new InvalidEnumArgumentException(nameof(Type), (int)Type, typeof(ObjectDataType)),
            };
        }

        protected void WriteValue(Utf8JsonWriter writer, ReadOnlySpan<char> propertyName, ObjectDataFormatVersion formatVersion)
        {
            switch (Type)
            {
                case ObjectDataType.Int:
                    writer.WriteNumber(propertyName, (int)Value);
                    break;

                case ObjectDataType.Real:
                case ObjectDataType.Unreal:
                    writer.WriteNumber(propertyName, (float)Value);
                    break;

                case ObjectDataType.String:
                    writer.WriteString(propertyName, (string)Value);
                    break;

                case ObjectDataType.Bool:
                    throw new NotSupportedException();

                case ObjectDataType.Char:
                    throw new NotSupportedException();

                default:
                    throw new InvalidEnumArgumentException(nameof(Type), (int)Type, typeof(ObjectDataType));
            }
        }
    }
}
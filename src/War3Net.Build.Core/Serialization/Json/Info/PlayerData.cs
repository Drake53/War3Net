// ------------------------------------------------------------------------------
// <copyright file="PlayerData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Text.Json;

using War3Net.Build.Extensions;

namespace War3Net.Build.Info
{
    public sealed partial class PlayerData
    {
        internal void ReadFrom(ref Utf8JsonReader reader, MapInfoFormatVersion formatVersion)
        {
            throw new NotImplementedException();
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapInfoFormatVersion formatVersion)
        {
            writer.WriteStartObject();

            writer.WriteNumber(nameof(Id), Id);
            writer.WriteObject(nameof(Controller), Controller, options);
            writer.WriteObject(nameof(Race), Race, options);
            writer.WriteObject(nameof(Flags), Flags, options);
            writer.WriteString(nameof(Name), Name);
            writer.Write(nameof(StartPosition), StartPosition);
            writer.Write(nameof(AllyLowPriorityFlags), AllyLowPriorityFlags, options);
            writer.Write(nameof(AllyHighPriorityFlags), AllyHighPriorityFlags, options);

            if (formatVersion >= MapInfoFormatVersion.Reforged)
            {
                writer.Write(nameof(EnemyLowPriorityFlags), EnemyLowPriorityFlags, options);
                writer.Write(nameof(EnemyHighPriorityFlags), EnemyHighPriorityFlags, options);
            }

            writer.WriteEndObject();
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="PlayerData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Common;
using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Info
{
    public sealed partial class PlayerData
    {
        internal PlayerData(JsonElement jsonElement, MapInfoFormatVersion formatVersion)
        {
            GetFrom(jsonElement, formatVersion);
        }

        internal PlayerData(ref Utf8JsonReader reader, MapInfoFormatVersion formatVersion)
        {
            ReadFrom(ref reader, formatVersion);
        }

        internal void GetFrom(JsonElement jsonElement, MapInfoFormatVersion formatVersion)
        {
            Id = jsonElement.GetInt32(nameof(Id));
            Controller = jsonElement.GetInt32<PlayerController>(nameof(Controller));
            Race = jsonElement.GetInt32<PlayerRace>(nameof(Race));
            Flags = jsonElement.GetInt32<PlayerFlags>(nameof(Flags));
            Name = jsonElement.GetString(nameof(Name));
            StartPosition = jsonElement.GetVector2(nameof(StartPosition));
            AllyLowPriorityFlags = jsonElement.GetBitmask32(nameof(AllyLowPriorityFlags));
            AllyHighPriorityFlags = jsonElement.GetBitmask32(nameof(AllyHighPriorityFlags));

            if (formatVersion >= MapInfoFormatVersion.Reforged)
            {
                EnemyLowPriorityFlags = jsonElement.GetBitmask32(nameof(EnemyLowPriorityFlags));
                EnemyHighPriorityFlags = jsonElement.GetBitmask32(nameof(EnemyHighPriorityFlags));
            }
            else
            {
                EnemyLowPriorityFlags = new Bitmask32(0);
                EnemyHighPriorityFlags = new Bitmask32(0);
            }
        }

        internal void ReadFrom(ref Utf8JsonReader reader, MapInfoFormatVersion formatVersion)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion);
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
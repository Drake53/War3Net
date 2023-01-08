// ------------------------------------------------------------------------------
// <copyright file="Camera.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    public sealed partial class Camera
    {
        internal Camera(JsonElement jsonElement, MapCamerasFormatVersion formatVersion, bool useNewFormat)
        {
            GetFrom(jsonElement, formatVersion, useNewFormat);
        }

        internal Camera(ref Utf8JsonReader reader, MapCamerasFormatVersion formatVersion, bool useNewFormat)
        {
            ReadFrom(ref reader, formatVersion, useNewFormat);
        }

        internal void GetFrom(JsonElement jsonElement, MapCamerasFormatVersion formatVersion, bool useNewFormat)
        {
            TargetPosition = jsonElement.GetVector2(nameof(TargetPosition));
            ZOffset = jsonElement.GetSingle(nameof(ZOffset));
            Rotation = jsonElement.GetSingle(nameof(Rotation));
            AngleOfAttack = jsonElement.GetSingle(nameof(AngleOfAttack));
            TargetDistance = jsonElement.GetSingle(nameof(TargetDistance));
            Roll = jsonElement.GetSingle(nameof(Roll));
            FieldOfView = jsonElement.GetSingle(nameof(FieldOfView));
            FarClippingPlane = jsonElement.GetSingle(nameof(FarClippingPlane));
            NearClippingPlane = jsonElement.GetSingle(nameof(NearClippingPlane));

            if (useNewFormat)
            {
                LocalPitch = jsonElement.GetSingle(nameof(LocalPitch));
                LocalYaw = jsonElement.GetSingle(nameof(LocalYaw));
                LocalRoll = jsonElement.GetSingle(nameof(LocalRoll));
            }

            Name = jsonElement.GetString(nameof(Name));
        }

        internal void ReadFrom(ref Utf8JsonReader reader, MapCamerasFormatVersion formatVersion, bool useNewFormat)
        {
            GetFrom(JsonDocument.ParseValue(ref reader).RootElement, formatVersion, useNewFormat);
        }

        internal void WriteTo(Utf8JsonWriter writer, JsonSerializerOptions options, MapCamerasFormatVersion formatVersion, bool useNewFormat)
        {
            writer.WriteStartObject();

            writer.Write(nameof(TargetPosition), TargetPosition);
            writer.WriteNumber(nameof(ZOffset), ZOffset);
            writer.WriteNumber(nameof(Rotation), Rotation);
            writer.WriteNumber(nameof(AngleOfAttack), AngleOfAttack);
            writer.WriteNumber(nameof(TargetDistance), TargetDistance);
            writer.WriteNumber(nameof(Roll), Roll);
            writer.WriteNumber(nameof(FieldOfView), FieldOfView);
            writer.WriteNumber(nameof(FarClippingPlane), FarClippingPlane);
            writer.WriteNumber(nameof(NearClippingPlane), NearClippingPlane);

            if (useNewFormat)
            {
                writer.WriteNumber(nameof(LocalPitch), LocalPitch);
                writer.WriteNumber(nameof(LocalYaw), LocalYaw);
                writer.WriteNumber(nameof(LocalRoll), LocalRoll);
            }

            writer.WriteString(nameof(Name), Name);

            writer.WriteEndObject();
        }
    }
}
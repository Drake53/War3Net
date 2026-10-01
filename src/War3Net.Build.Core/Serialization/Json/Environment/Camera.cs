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

                if (formatVersion >= MapCamerasFormatVersion.v3)
                {
                    DofDistance = jsonElement.GetSingle(nameof(DofDistance));
                    DofScale = jsonElement.GetSingle(nameof(DofScale));
                    PosAbsoluteZ = jsonElement.GetSingle(nameof(PosAbsoluteZ));
                }
            }

            Name = jsonElement.GetString(nameof(Name));

            if (formatVersion >= MapCamerasFormatVersion.v3)
            {
                CameraType = jsonElement.GetInt32(nameof(CameraType));
            }
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

                if (formatVersion >= MapCamerasFormatVersion.v3)
                {
                    writer.WriteNumber(nameof(DofDistance), DofDistance);
                    writer.WriteNumber(nameof(DofScale), DofScale);
                    writer.WriteNumber(nameof(PosAbsoluteZ), PosAbsoluteZ);
                }
            }

            writer.WriteString(nameof(Name), Name);

            if (formatVersion >= MapCamerasFormatVersion.v3)
            {
                writer.WriteNumber(nameof(CameraType), CameraType);
            }

            writer.WriteEndObject();
        }
    }
}
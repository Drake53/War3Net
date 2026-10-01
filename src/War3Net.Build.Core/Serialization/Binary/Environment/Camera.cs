namespace War3Net.Build.Environment
{
    public sealed partial class Camera
    {
        internal Camera(BinaryReader reader, MapCamerasFormatVersion formatVersion, bool useNewFormat)
        {
            ReadFrom(reader, formatVersion, useNewFormat);
        }

        internal void ReadFrom(BinaryReader reader, MapCamerasFormatVersion formatVersion, bool useNewFormat)
        {
            TargetPosition = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            ZOffset = reader.ReadSingle();
            Rotation = reader.ReadSingle();
            AngleOfAttack = reader.ReadSingle();
            TargetDistance = reader.ReadSingle();
            Roll = reader.ReadSingle();
            FieldOfView = reader.ReadSingle();
            FarClippingPlane = reader.ReadSingle();
            NearClippingPlane = reader.ReadSingle();

            if (useNewFormat)
            {
                LocalPitch = reader.ReadSingle();
                LocalYaw = reader.ReadSingle();
                LocalRoll = reader.ReadSingle();

                if (formatVersion >= MapCamerasFormatVersion.v3)
                {
                    DofDistance = reader.ReadSingle();
                    DofScale = reader.ReadSingle();
                    PosAbsoluteZ = reader.ReadSingle();
                }
            }

            Name = reader.ReadChars();
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new InvalidDataException($"Camera name must contain at least one non-whitespace character.");
            }

            if (formatVersion >= MapCamerasFormatVersion.v3)
            {
                CameraType = reader.ReadInt32();
            }
        }

        internal void WriteTo(BinaryWriter writer, MapCamerasFormatVersion formatVersion, bool useNewFormat)
        {
            writer.Write(TargetPosition.X);
            writer.Write(TargetPosition.Y);
            writer.Write(ZOffset);
            writer.Write(Rotation);
            writer.Write(AngleOfAttack);
            writer.Write(TargetDistance);
            writer.Write(Roll);
            writer.Write(FieldOfView);
            writer.Write(FarClippingPlane);
            writer.Write(NearClippingPlane);

            if (useNewFormat)
            {
                writer.Write(LocalPitch);
                writer.Write(LocalYaw);
                writer.Write(LocalRoll);

                if (formatVersion >= MapCamerasFormatVersion.v3)
                {
                    writer.Write(DofDistance);
                    writer.Write(DofScale);
                    writer.Write(PosAbsoluteZ);
                }
            }

            writer.WriteString(Name);

            if (formatVersion >= MapCamerasFormatVersion.v3)
            {
                writer.Write(CameraType);
            }
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="Camera.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Numerics;

using War3Net.Common.Extensions;

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
            }

            Name = reader.ReadChars();
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new InvalidDataException($"Camera name must contain at least one non-whitespace character.");
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
            }

            writer.WriteString(Name);
        }
    }
}
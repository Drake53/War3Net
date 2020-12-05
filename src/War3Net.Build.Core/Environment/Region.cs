// ------------------------------------------------------------------------------
// <copyright file="Region.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Drawing;
using System.IO;

using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    public sealed class Region
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Region"/> class.
        /// </summary>
        public Region()
        {
        }

        internal Region(BinaryReader reader, MapRegionsFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        public float Left { get; set; }

        public float Bottom { get; set; }

        public float Right { get; set; }

        public float Top { get; set; }

        public string Name { get; set; }

        public int CreationNumber { get; set; }

        public WeatherType WeatherType { get; set; }

        public string AmbientSound { get; set; }

        public Color Color { get; set; }

        public float Width => Right - Left;

        public float Height => Top - Bottom;

        public float CenterX => 0.5f * (Left + Right);

        public float CenterY => 0.5f * (Top + Bottom);

        internal void ReadFrom(BinaryReader reader, MapRegionsFormatVersion formatVersion)
        {
            Left = reader.ReadSingle();
            Bottom = reader.ReadSingle();
            Right = reader.ReadSingle();
            Top = reader.ReadSingle();
            Name = reader.ReadChars();
            CreationNumber = reader.ReadInt32();
            WeatherType = reader.ReadInt32<WeatherType>();
            AmbientSound = reader.ReadChars();
            Color = Color.FromArgb(reader.ReadInt32());
        }

        internal void WriteTo(BinaryWriter writer, MapRegionsFormatVersion formatVersion)
        {
            writer.Write(Left);
            writer.Write(Bottom);
            writer.Write(Right);
            writer.Write(Top);
            writer.WriteString(Name);
            writer.Write(CreationNumber);
            writer.Write((int)WeatherType);
            writer.WriteString(AmbientSound);
            writer.Write(Color.ToArgb());
        }
    }
}
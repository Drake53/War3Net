// ------------------------------------------------------------------------------
// <copyright file="Region.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Drawing;
using System.IO;

using War3Net.Build.Common;
using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    public sealed partial class Region
    {
        internal Region(BinaryReader reader, MapRegionsFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

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
            Color = reader.ReadColorBgra();
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
            writer.Write(Color.ToBgra());
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="Quadrilateral.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Numerics;

namespace War3Net.Build.Common
{
    public sealed partial class Quadrilateral
    {
        internal Quadrilateral(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        internal void ReadFrom(BinaryReader reader)
        {
            BottomLeft = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            TopRight = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            TopLeft = new Vector2(reader.ReadSingle(), reader.ReadSingle());
            BottomRight = new Vector2(reader.ReadSingle(), reader.ReadSingle());
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write(BottomLeft.X);
            writer.Write(BottomLeft.Y);
            writer.Write(TopRight.X);
            writer.Write(TopRight.Y);
            writer.Write(TopLeft.X);
            writer.Write(TopLeft.Y);
            writer.Write(BottomRight.X);
            writer.Write(BottomRight.Y);
        }
    }
}
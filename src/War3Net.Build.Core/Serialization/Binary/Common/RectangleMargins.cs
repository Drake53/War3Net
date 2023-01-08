// ------------------------------------------------------------------------------
// <copyright file="RectangleMargins.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Common
{
    public sealed partial class RectangleMargins
    {
        internal RectangleMargins(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        internal void ReadFrom(BinaryReader reader)
        {
            Left = reader.ReadInt32();
            Right = reader.ReadInt32();
            Bottom = reader.ReadInt32();
            Top = reader.ReadInt32();
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write(Left);
            writer.Write(Right);
            writer.Write(Bottom);
            writer.Write(Top);
        }
    }
}
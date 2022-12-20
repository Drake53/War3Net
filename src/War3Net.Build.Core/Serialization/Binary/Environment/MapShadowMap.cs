// ------------------------------------------------------------------------------
// <copyright file="MapShadowMap.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Environment
{
    public sealed partial class MapShadowMap
    {
        internal MapShadowMap(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        internal void ReadFrom(BinaryReader reader)
        {
            Cells.AddRange(reader.ReadBytes((int)(reader.BaseStream.Length - reader.BaseStream.Position)));
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write(Cells.ToArray());
        }
    }
}
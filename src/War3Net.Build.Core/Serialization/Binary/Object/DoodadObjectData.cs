// ------------------------------------------------------------------------------
// <copyright file="DoodadObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public sealed partial class DoodadObjectData
    {
        internal DoodadObjectData(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        internal void ReadFrom(BinaryReader reader)
        {
            FormatVersion = reader.ReadInt32<ObjectDataFormatVersion>();

            nint baseDoodadsCount = reader.ReadInt32();
            for (nint i = 0; i < baseDoodadsCount; i++)
            {
                BaseDoodads.Add(reader.ReadVariationObjectModification(FormatVersion));
            }

            nint newDoodadsCount = reader.ReadInt32();
            for (nint i = 0; i < newDoodadsCount; i++)
            {
                NewDoodads.Add(reader.ReadVariationObjectModification(FormatVersion));
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write((int)FormatVersion);

            writer.Write(BaseDoodads.Count);
            foreach (var doodad in BaseDoodads)
            {
                writer.Write(doodad, FormatVersion);
            }

            writer.Write(NewDoodads.Count);
            foreach (var doodad in NewDoodads)
            {
                writer.Write(doodad, FormatVersion);
            }
        }
    }
}
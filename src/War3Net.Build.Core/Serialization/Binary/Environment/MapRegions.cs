// ------------------------------------------------------------------------------
// <copyright file="MapRegions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Environment
{
    public sealed partial class MapRegions
    {
        internal MapRegions(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        internal void ReadFrom(BinaryReader reader)
        {
            FormatVersion = reader.ReadInt32<MapRegionsFormatVersion>();

            nint regionCount = reader.ReadInt32();
            if (regionCount == ProtectionSignature)
            {
                Protected = true;
            }
            else
            {
                for (nint i = 0; i < regionCount; i++)
                {
                    Regions.Add(reader.ReadRegion(FormatVersion));
                }
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write((int)FormatVersion);

            if (Protected)
            {
                writer.Write(ProtectionSignature);
            }
            else
            {
                writer.Write(Regions.Count);
                foreach (var region in Regions)
                {
                    writer.Write(region, FormatVersion);
                }
            }
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="TechData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Extensions;

namespace War3Net.Build.Info
{
    public sealed partial class TechData
    {
        internal void ReadFrom(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            Players = reader.ReadBitmask32();
            Id = reader.ReadInt32();
        }

        internal void WriteTo(BinaryWriter writer, MapInfoFormatVersion formatVersion)
        {
            writer.Write(Players);
            writer.Write(Id);
        }
    }
}
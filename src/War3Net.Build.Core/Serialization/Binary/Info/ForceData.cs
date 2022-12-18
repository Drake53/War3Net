// ------------------------------------------------------------------------------
// <copyright file="ForceData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Info
{
    public sealed partial class ForceData
    {
        internal void ReadFrom(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            Flags = reader.ReadInt32<ForceFlags>();
            Players = reader.ReadBitmask32();
            Name = reader.ReadChars();
        }

        internal void WriteTo(BinaryWriter writer, MapInfoFormatVersion formatVersion)
        {
            writer.Write((int)Flags);
            writer.Write(Players);
            writer.WriteString(Name);
        }
    }
}
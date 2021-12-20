// ------------------------------------------------------------------------------
// <copyright file="CampaignMap.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Common.Extensions;

namespace War3Net.Build.Info
{
    public sealed class CampaignMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignMap"/> class.
        /// </summary>
        public CampaignMap()
        {
        }

        internal CampaignMap(BinaryReader reader, CampaignInfoFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        public string Unk { get; set; }

        public string MapFilePath { get; set; }

        public override string ToString() => MapFilePath;

        internal void ReadFrom(BinaryReader reader, CampaignInfoFormatVersion formatVersion)
        {
            Unk = reader.ReadChars();
            MapFilePath = reader.ReadChars();
        }

        internal void WriteTo(BinaryWriter writer, CampaignInfoFormatVersion formatVersion)
        {
            writer.WriteString(Unk);
            writer.WriteString(MapFilePath);
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="CampaignInfo.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Drawing;
using System.IO;

using War3Net.Build.Extensions;
using War3Net.Common.Extensions;

namespace War3Net.Build.Info
{
    public sealed class CampaignInfo
    {
        public const string FileName = "war3campaign.w3f";

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignInfo"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public CampaignInfo(CampaignInfoFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        internal CampaignInfo(BinaryReader reader)
        {
            ReadFrom(reader);
        }

        public CampaignInfoFormatVersion FormatVersion { get; set; }

        public int CampaignVersion { get; set; }

        public int EditorVersion { get; set; }

        public string CampaignName { get; set; }

        public string CampaignDifficulty { get; set; }

        public string CampaignAuthor { get; set; }

        public string CampaignDescription { get; set; }

        public CampaignFlags CampaignFlags { get; set; }

        public int CampaignBackgroundNumber { get; set; }

        public string BackgroundScreenPath { get; set; }

        public string MinimapPath { get; set; }

        public int AmbientSoundNumber { get; set; }

        public string AmbientSoundPath { get; set; }

        public FogStyle FogStyle { get; set; }

        public float FogStartZ { get; set; }

        public float FogEndZ { get; set; }

        public float FogDensity { get; set; }

        public Color FogColor { get; set; }

        public CampaignRace Race { get; set; }

        public List<CampaignMapButton> MapButtons { get; init; } = new();

        public List<CampaignMap> Maps { get; init; } = new();

        public override string ToString() => FileName;

        internal void ReadFrom(BinaryReader reader)
        {
            FormatVersion = reader.ReadInt32<CampaignInfoFormatVersion>();
            CampaignVersion = reader.ReadInt32();
            EditorVersion = reader.ReadInt32();
            CampaignName = reader.ReadChars();
            CampaignDifficulty = reader.ReadChars();
            CampaignAuthor = reader.ReadChars();
            CampaignDescription = reader.ReadChars();
            CampaignFlags = reader.ReadInt32<CampaignFlags>();
            CampaignBackgroundNumber = reader.ReadInt32();
            BackgroundScreenPath = reader.ReadChars();
            MinimapPath = reader.ReadChars();
            AmbientSoundNumber = reader.ReadInt32();
            AmbientSoundPath = reader.ReadChars();
            FogStyle = reader.ReadInt32<FogStyle>();
            FogStartZ = reader.ReadSingle();
            FogEndZ = reader.ReadSingle();
            FogDensity = reader.ReadSingle();
            FogColor = reader.ReadColorBgra();
            Race = reader.ReadInt32<CampaignRace>();

            nint mapCount = reader.ReadInt32();
            for (nint i = 0; i < mapCount; i++)
            {
                MapButtons.Add(reader.ReadCampaignMapButton(FormatVersion));
            }

            nint mapOrderCount = reader.ReadInt32();
            for (nint i = 0; i < mapOrderCount; i++)
            {
                Maps.Add(reader.ReadCampaignMap(FormatVersion));
            }
        }

        internal void WriteTo(BinaryWriter writer)
        {
            writer.Write((int)FormatVersion);
            writer.Write(CampaignVersion);
            writer.Write(EditorVersion);
            writer.WriteString(CampaignName);
            writer.WriteString(CampaignDifficulty);
            writer.WriteString(CampaignAuthor);
            writer.WriteString(CampaignDescription);
            writer.Write((int)CampaignFlags);
            writer.Write(CampaignBackgroundNumber);
            writer.WriteString(BackgroundScreenPath);
            writer.WriteString(MinimapPath);
            writer.Write(AmbientSoundNumber);
            writer.WriteString(AmbientSoundPath);
            writer.Write((int)FogStyle);
            writer.Write(FogStartZ);
            writer.Write(FogEndZ);
            writer.Write(FogDensity);
            writer.Write(FogColor.ToBgra());
            writer.Write((int)Race);

            writer.Write(MapButtons.Count);
            foreach (var mapButton in MapButtons)
            {
                writer.Write(mapButton, FormatVersion);
            }

            writer.Write(Maps.Count);
            foreach (var map in Maps)
            {
                writer.Write(map, FormatVersion);
            }
        }
    }
}
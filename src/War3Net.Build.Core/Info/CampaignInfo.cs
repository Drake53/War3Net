// ------------------------------------------------------------------------------
// <copyright file="CampaignInfo.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Drawing;

namespace War3Net.Build.Info
{
    public sealed partial class CampaignInfo
    {
        public const string FileExtension = ".w3f";
        public const string FileName = "war3campaign.w3f";

        /// <summary>
        /// Initializes a new instance of the <see cref="CampaignInfo"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public CampaignInfo(CampaignInfoFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
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
    }
}
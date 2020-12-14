// ------------------------------------------------------------------------------
// <copyright file="Campaign.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.Build.Object;
using War3Net.Build.Script;
using War3Net.IO.Mpq;

namespace War3Net.Build
{
    public sealed class Campaign
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Campaign"/> class.
        /// </summary>
        /// <param name="campaignInfo"></param>
        public Campaign(CampaignInfo campaignInfo)
        {
            Info = campaignInfo;
        }

        private Campaign(MpqArchive campaignArchive)
        {
            using var infoStream = MpqFile.OpenRead(campaignArchive, CampaignInfo.FileName);
            using var infoReader = new BinaryReader(infoStream);
            Info = infoReader.ReadCampaignInfo();

            if (MpqFile.Exists(campaignArchive, CampaignAbilityObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, CampaignAbilityObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                AbilityObjectData = reader.ReadCampaignAbilityObjectData();
            }

            if (MpqFile.Exists(campaignArchive, CampaignBuffObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, CampaignBuffObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                BuffObjectData = reader.ReadCampaignBuffObjectData();
            }

            if (MpqFile.Exists(campaignArchive, CampaignDestructableObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, CampaignDestructableObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                DestructableObjectData = reader.ReadCampaignDestructableObjectData();
            }

            if (MpqFile.Exists(campaignArchive, CampaignDoodadObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, CampaignDoodadObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                DoodadObjectData = reader.ReadCampaignDoodadObjectData();
            }

            if (MpqFile.Exists(campaignArchive, CampaignItemObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, CampaignItemObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                ItemObjectData = reader.ReadCampaignItemObjectData();
            }

            if (MpqFile.Exists(campaignArchive, CampaignUnitObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, CampaignUnitObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                UnitObjectData = reader.ReadCampaignUnitObjectData();
            }

            if (MpqFile.Exists(campaignArchive, CampaignUpgradeObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, CampaignUpgradeObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                UpgradeObjectData = reader.ReadCampaignUpgradeObjectData();
            }

            if (MpqFile.Exists(campaignArchive, CampaignTriggerStrings.FileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, CampaignTriggerStrings.FileName);
                using var reader = new StreamReader(fileStream);
                TriggerStrings = reader.ReadCampaignTriggerStrings();
            }
        }

        public CampaignInfo Info { get; set; }

        public CampaignAbilityObjectData? AbilityObjectData { get; set; }

        public CampaignBuffObjectData? BuffObjectData { get; set; }

        public CampaignDestructableObjectData? DestructableObjectData { get; set; }

        public CampaignDoodadObjectData? DoodadObjectData { get; set; }

        public CampaignItemObjectData? ItemObjectData { get; set; }

        public CampaignUnitObjectData? UnitObjectData { get; set; }

        public CampaignUpgradeObjectData? UpgradeObjectData { get; set; }

        public CampaignTriggerStrings? TriggerStrings { get; set; }

        public static Campaign Open(string path)
        {
            if (File.Exists(path))
            {
                using var campaignArchive = MpqArchive.Open(path);
                return new Campaign(campaignArchive);
            }
            else
            {
                throw new ArgumentException("Could not find a file at the specified path.");
            }
        }

        public static Campaign Open(Stream stream)
        {
            using var campaignArchive = MpqArchive.Open(stream);
            return new Campaign(campaignArchive);
        }

        public static Campaign Open(MpqArchive archive)
        {
            return new Campaign(archive);
        }
    }
}
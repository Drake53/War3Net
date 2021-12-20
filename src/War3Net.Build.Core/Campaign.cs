// ------------------------------------------------------------------------------
// <copyright file="Campaign.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.Build.Extensions;
using War3Net.Build.Import;
using War3Net.Build.Info;
using War3Net.Build.Object;
using War3Net.Build.Script;
using War3Net.IO.Mpq;

namespace War3Net.Build
{
    public sealed class Campaign
    {
        private readonly string? _campaignName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Campaign"/> class.
        /// </summary>
        public Campaign()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Campaign"/> class.
        /// </summary>
        /// <param name="campaignInfo"></param>
        [Obsolete]
        public Campaign(CampaignInfo? campaignInfo)
        {
            Info = campaignInfo;
        }

        private Campaign(string? campaignName, string campaignFolder, CampaignFiles campaignFiles)
        {
            _campaignName = campaignName;

            if (campaignFiles.HasFlag(CampaignFiles.ImportedFiles) && File.Exists(Path.Combine(campaignFolder, CampaignImportedFiles.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(campaignFolder, CampaignImportedFiles.FileName));
                using var reader = new BinaryReader(fileStream);
                ImportedFiles = reader.ReadCampaignImportedFiles();
            }

            if (campaignFiles.HasFlag(CampaignFiles.Info) && File.Exists(Path.Combine(campaignFolder, CampaignInfo.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(campaignFolder, CampaignInfo.FileName));
                using var reader = new BinaryReader(fileStream);
                Info = reader.ReadCampaignInfo();
            }

            if (campaignFiles.HasFlag(CampaignFiles.AbilityObjectData) && File.Exists(Path.Combine(campaignFolder, CampaignAbilityObjectData.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(campaignFolder, CampaignAbilityObjectData.FileName));
                using var reader = new BinaryReader(fileStream);
                AbilityObjectData = reader.ReadCampaignAbilityObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.BuffObjectData) && File.Exists(Path.Combine(campaignFolder, CampaignBuffObjectData.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(campaignFolder, CampaignBuffObjectData.FileName));
                using var reader = new BinaryReader(fileStream);
                BuffObjectData = reader.ReadCampaignBuffObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.DestructableObjectData) && File.Exists(Path.Combine(campaignFolder, CampaignDestructableObjectData.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(campaignFolder, CampaignDestructableObjectData.FileName));
                using var reader = new BinaryReader(fileStream);
                DestructableObjectData = reader.ReadCampaignDestructableObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.DoodadObjectData) && File.Exists(Path.Combine(campaignFolder, CampaignDoodadObjectData.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(campaignFolder, CampaignDoodadObjectData.FileName));
                using var reader = new BinaryReader(fileStream);
                DoodadObjectData = reader.ReadCampaignDoodadObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.ItemObjectData) && File.Exists(Path.Combine(campaignFolder, CampaignItemObjectData.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(campaignFolder, CampaignItemObjectData.FileName));
                using var reader = new BinaryReader(fileStream);
                ItemObjectData = reader.ReadCampaignItemObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.UnitObjectData) && File.Exists(Path.Combine(campaignFolder, CampaignUnitObjectData.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(campaignFolder, CampaignUnitObjectData.FileName));
                using var reader = new BinaryReader(fileStream);
                UnitObjectData = reader.ReadCampaignUnitObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.UpgradeObjectData) && File.Exists(Path.Combine(campaignFolder, CampaignUpgradeObjectData.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(campaignFolder, CampaignUpgradeObjectData.FileName));
                using var reader = new BinaryReader(fileStream);
                UpgradeObjectData = reader.ReadCampaignUpgradeObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.TriggerStrings) && File.Exists(Path.Combine(campaignFolder, CampaignTriggerStrings.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(campaignFolder, CampaignTriggerStrings.FileName));
                using var reader = new StreamReader(fileStream);
                TriggerStrings = reader.ReadCampaignTriggerStrings();
            }
        }

        private Campaign(string? campaignName, MpqArchive campaignArchive, CampaignFiles campaignFiles)
        {
            _campaignName = campaignName;

            if (campaignFiles.HasFlag(CampaignFiles.ImportedFiles) && MpqFile.Exists(campaignArchive, CampaignImportedFiles.FileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, CampaignImportedFiles.FileName);
                using var reader = new BinaryReader(fileStream);
                ImportedFiles = reader.ReadCampaignImportedFiles();
            }

            if (campaignFiles.HasFlag(CampaignFiles.Info) && MpqFile.Exists(campaignArchive, CampaignInfo.FileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, CampaignInfo.FileName);
                using var reader = new BinaryReader(fileStream);
                Info = reader.ReadCampaignInfo();
            }

            if (campaignFiles.HasFlag(CampaignFiles.AbilityObjectData) && MpqFile.Exists(campaignArchive, CampaignAbilityObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, CampaignAbilityObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                AbilityObjectData = reader.ReadCampaignAbilityObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.BuffObjectData) && MpqFile.Exists(campaignArchive, CampaignBuffObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, CampaignBuffObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                BuffObjectData = reader.ReadCampaignBuffObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.DestructableObjectData) && MpqFile.Exists(campaignArchive, CampaignDestructableObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, CampaignDestructableObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                DestructableObjectData = reader.ReadCampaignDestructableObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.DoodadObjectData) && MpqFile.Exists(campaignArchive, CampaignDoodadObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, CampaignDoodadObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                DoodadObjectData = reader.ReadCampaignDoodadObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.ItemObjectData) && MpqFile.Exists(campaignArchive, CampaignItemObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, CampaignItemObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                ItemObjectData = reader.ReadCampaignItemObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.UnitObjectData) && MpqFile.Exists(campaignArchive, CampaignUnitObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, CampaignUnitObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                UnitObjectData = reader.ReadCampaignUnitObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.UpgradeObjectData) && MpqFile.Exists(campaignArchive, CampaignUpgradeObjectData.FileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, CampaignUpgradeObjectData.FileName);
                using var reader = new BinaryReader(fileStream);
                UpgradeObjectData = reader.ReadCampaignUpgradeObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.TriggerStrings) && MpqFile.Exists(campaignArchive, CampaignTriggerStrings.FileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, CampaignTriggerStrings.FileName);
                using var reader = new StreamReader(fileStream);
                TriggerStrings = reader.ReadCampaignTriggerStrings();
            }
        }

        public CampaignImportedFiles? ImportedFiles { get; set; }

        public CampaignInfo? Info { get; set; }

        public CampaignAbilityObjectData? AbilityObjectData { get; set; }

        public CampaignBuffObjectData? BuffObjectData { get; set; }

        public CampaignDestructableObjectData? DestructableObjectData { get; set; }

        public CampaignDoodadObjectData? DoodadObjectData { get; set; }

        public CampaignItemObjectData? ItemObjectData { get; set; }

        public CampaignUnitObjectData? UnitObjectData { get; set; }

        public CampaignUpgradeObjectData? UpgradeObjectData { get; set; }

        public CampaignTriggerStrings? TriggerStrings { get; set; }

        /// <summary>
        /// Opens the campaign from the specified file or folder path.
        /// </summary>
        public static Campaign Open(string path, CampaignFiles campaignFiles = CampaignFiles.All)
        {
            var fileInfo = new FileInfo(path);
            if (fileInfo.Exists)
            {
                using var campaignArchive = MpqArchive.Open(path);
                return new Campaign(fileInfo.Name, campaignArchive, campaignFiles);
            }
            else
            {
                var directoryInfo = new DirectoryInfo(path);
                if (directoryInfo.Exists)
                {
                    return new Campaign(directoryInfo.Name, path, campaignFiles);
                }
                else
                {
                    throw new ArgumentException("Could not find a file or folder at the specified path.");
                }
            }
        }

        public static Campaign Open(Stream stream, CampaignFiles campaignFiles = CampaignFiles.All)
        {
            using var campaignArchive = MpqArchive.Open(stream);
            return new Campaign(null, campaignArchive, campaignFiles);
        }

        public static Campaign Open(MpqArchive archive, CampaignFiles campaignFiles = CampaignFiles.All)
        {
            return new Campaign(null, archive, campaignFiles);
        }

        public static bool TryOpen(string path, [NotNullWhen(true)] out Campaign? campaign, CampaignFiles campaignFiles = CampaignFiles.All)
        {
            try
            {
                campaign = Open(path, campaignFiles);
                return true;
            }
            catch
            {
                campaign = null;
                return false;
            }
        }

        public static bool TryOpen(Stream stream, [NotNullWhen(true)] out Campaign? campaign, CampaignFiles campaignFiles = CampaignFiles.All)
        {
            try
            {
                campaign = Open(stream, campaignFiles);
                return true;
            }
            catch
            {
                campaign = null;
                return false;
            }
        }

        public static bool TryOpen(MpqArchive archive, [NotNullWhen(true)] out Campaign? campaign, CampaignFiles campaignFiles = CampaignFiles.All)
        {
            try
            {
                campaign = Open(archive, campaignFiles);
                return true;
            }
            catch
            {
                campaign = null;
                return false;
            }
        }

        public override string? ToString()
        {
            return string.IsNullOrEmpty(_campaignName) ? base.ToString() : _campaignName;
        }
    }
}
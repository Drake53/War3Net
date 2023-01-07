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

            if (campaignFiles.HasFlag(CampaignFiles.ImportedFiles) && File.Exists(Path.Combine(campaignFolder, ImportedFiles.CampaignFileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(campaignFolder, ImportedFiles.CampaignFileName));
                using var reader = new BinaryReader(fileStream);
                ImportedFiles = reader.ReadImportedFiles();
            }

            if (campaignFiles.HasFlag(CampaignFiles.Info) && File.Exists(Path.Combine(campaignFolder, CampaignInfo.FileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(campaignFolder, CampaignInfo.FileName));
                using var reader = new BinaryReader(fileStream);
                Info = reader.ReadCampaignInfo();
            }

            if (campaignFiles.HasFlag(CampaignFiles.AbilityObjectData) && File.Exists(Path.Combine(campaignFolder, AbilityObjectData.CampaignFileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(campaignFolder, AbilityObjectData.CampaignFileName));
                using var reader = new BinaryReader(fileStream);
                AbilityObjectData = reader.ReadAbilityObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.BuffObjectData) && File.Exists(Path.Combine(campaignFolder, BuffObjectData.CampaignFileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(campaignFolder, BuffObjectData.CampaignFileName));
                using var reader = new BinaryReader(fileStream);
                BuffObjectData = reader.ReadBuffObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.DestructableObjectData) && File.Exists(Path.Combine(campaignFolder, DestructableObjectData.CampaignFileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(campaignFolder, DestructableObjectData.CampaignFileName));
                using var reader = new BinaryReader(fileStream);
                DestructableObjectData = reader.ReadDestructableObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.DoodadObjectData) && File.Exists(Path.Combine(campaignFolder, DoodadObjectData.CampaignFileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(campaignFolder, DoodadObjectData.CampaignFileName));
                using var reader = new BinaryReader(fileStream);
                DoodadObjectData = reader.ReadDoodadObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.ItemObjectData) && File.Exists(Path.Combine(campaignFolder, ItemObjectData.CampaignFileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(campaignFolder, ItemObjectData.CampaignFileName));
                using var reader = new BinaryReader(fileStream);
                ItemObjectData = reader.ReadItemObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.UnitObjectData) && File.Exists(Path.Combine(campaignFolder, UnitObjectData.CampaignFileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(campaignFolder, UnitObjectData.CampaignFileName));
                using var reader = new BinaryReader(fileStream);
                UnitObjectData = reader.ReadUnitObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.UpgradeObjectData) && File.Exists(Path.Combine(campaignFolder, UpgradeObjectData.CampaignFileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(campaignFolder, UpgradeObjectData.CampaignFileName));
                using var reader = new BinaryReader(fileStream);
                UpgradeObjectData = reader.ReadUpgradeObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.TriggerStrings) && File.Exists(Path.Combine(campaignFolder, TriggerStrings.CampaignFileName)))
            {
                using var fileStream = File.OpenRead(Path.Combine(campaignFolder, TriggerStrings.CampaignFileName));
                using var reader = new StreamReader(fileStream);
                TriggerStrings = reader.ReadTriggerStrings();
            }
        }

        private Campaign(string? campaignName, MpqArchive campaignArchive, CampaignFiles campaignFiles)
        {
            _campaignName = campaignName;

            if (campaignFiles.HasFlag(CampaignFiles.ImportedFiles) && MpqFile.Exists(campaignArchive, ImportedFiles.CampaignFileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, ImportedFiles.CampaignFileName);
                using var reader = new BinaryReader(fileStream);
                ImportedFiles = reader.ReadImportedFiles();
            }

            if (campaignFiles.HasFlag(CampaignFiles.Info) && MpqFile.Exists(campaignArchive, CampaignInfo.FileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, CampaignInfo.FileName);
                using var reader = new BinaryReader(fileStream);
                Info = reader.ReadCampaignInfo();
            }

            if (campaignFiles.HasFlag(CampaignFiles.AbilityObjectData) && MpqFile.Exists(campaignArchive, AbilityObjectData.CampaignFileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, AbilityObjectData.CampaignFileName);
                using var reader = new BinaryReader(fileStream);
                AbilityObjectData = reader.ReadAbilityObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.BuffObjectData) && MpqFile.Exists(campaignArchive, BuffObjectData.CampaignFileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, BuffObjectData.CampaignFileName);
                using var reader = new BinaryReader(fileStream);
                BuffObjectData = reader.ReadBuffObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.DestructableObjectData) && MpqFile.Exists(campaignArchive, DestructableObjectData.CampaignFileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, DestructableObjectData.CampaignFileName);
                using var reader = new BinaryReader(fileStream);
                DestructableObjectData = reader.ReadDestructableObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.DoodadObjectData) && MpqFile.Exists(campaignArchive, DoodadObjectData.CampaignFileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, DoodadObjectData.CampaignFileName);
                using var reader = new BinaryReader(fileStream);
                DoodadObjectData = reader.ReadDoodadObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.ItemObjectData) && MpqFile.Exists(campaignArchive, ItemObjectData.CampaignFileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, ItemObjectData.CampaignFileName);
                using var reader = new BinaryReader(fileStream);
                ItemObjectData = reader.ReadItemObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.UnitObjectData) && MpqFile.Exists(campaignArchive, UnitObjectData.CampaignFileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, UnitObjectData.CampaignFileName);
                using var reader = new BinaryReader(fileStream);
                UnitObjectData = reader.ReadUnitObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.UpgradeObjectData) && MpqFile.Exists(campaignArchive, UpgradeObjectData.CampaignFileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, UpgradeObjectData.CampaignFileName);
                using var reader = new BinaryReader(fileStream);
                UpgradeObjectData = reader.ReadUpgradeObjectData();
            }

            if (campaignFiles.HasFlag(CampaignFiles.TriggerStrings) && MpqFile.Exists(campaignArchive, TriggerStrings.CampaignFileName))
            {
                using var fileStream = MpqFile.OpenRead(campaignArchive, TriggerStrings.CampaignFileName);
                using var reader = new StreamReader(fileStream);
                TriggerStrings = reader.ReadTriggerStrings();
            }
        }

        public ImportedFiles? ImportedFiles { get; set; }

        public CampaignInfo? Info { get; set; }

        public AbilityObjectData? AbilityObjectData { get; set; }

        public BuffObjectData? BuffObjectData { get; set; }

        public DestructableObjectData? DestructableObjectData { get; set; }

        public DoodadObjectData? DoodadObjectData { get; set; }

        public ItemObjectData? ItemObjectData { get; set; }

        public UnitObjectData? UnitObjectData { get; set; }

        public UpgradeObjectData? UpgradeObjectData { get; set; }

        public TriggerStrings? TriggerStrings { get; set; }

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
            if (!string.IsNullOrEmpty(_campaignName))
            {
                return _campaignName;
            }

            var campaignName = Info?.CampaignName.Localize(TriggerStrings);
            if (!string.IsNullOrEmpty(campaignName))
            {
                return campaignName;
            }

            return base.ToString();
        }
    }
}
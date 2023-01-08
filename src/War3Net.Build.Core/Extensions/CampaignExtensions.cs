// ------------------------------------------------------------------------------
// <copyright file="CampaignExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Build.Import;
using War3Net.Build.Info;
using War3Net.Build.Object;
using War3Net.Build.Script;
using War3Net.Common.Providers;
using War3Net.IO.Mpq;

namespace War3Net.Build.Extensions
{
    public static class CampaignExtensions
    {
        private static readonly Encoding _defaultEncoding = UTF8EncodingProvider.StrictUTF8;

        public static MpqFile? GetImportedFilesFile(this Campaign campaign, Encoding? encoding = null)
        {
            if (campaign.ImportedFiles is null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(campaign.ImportedFiles);
            writer.Flush();

            return MpqFile.New(memoryStream, ImportedFiles.CampaignFileName);
        }

        public static MpqFile? GetInfoFile(this Campaign campaign, Encoding? encoding = null)
        {
            if (campaign.Info is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(campaign.Info);
            writer.Flush();

            return MpqFile.New(memoryStream, CampaignInfo.FileName);
        }

        public static MpqFile? GetAbilityObjectDataFile(this Campaign campaign, Encoding? encoding = null)
        {
            if (campaign.AbilityObjectData is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(campaign.AbilityObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, AbilityObjectData.CampaignFileName);
        }

        public static MpqFile? GetBuffObjectDataFile(this Campaign campaign, Encoding? encoding = null)
        {
            if (campaign.BuffObjectData is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(campaign.BuffObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, BuffObjectData.CampaignFileName);
        }

        public static MpqFile? GetDestructableObjectDataFile(this Campaign campaign, Encoding? encoding = null)
        {
            if (campaign.DestructableObjectData is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(campaign.DestructableObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, DestructableObjectData.CampaignFileName);
        }

        public static MpqFile? GetDoodadObjectDataFile(this Campaign campaign, Encoding? encoding = null)
        {
            if (campaign.DoodadObjectData is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(campaign.DoodadObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, DoodadObjectData.CampaignFileName);
        }

        public static MpqFile? GetItemObjectDataFile(this Campaign campaign, Encoding? encoding = null)
        {
            if (campaign.ItemObjectData is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(campaign.ItemObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, ItemObjectData.CampaignFileName);
        }

        public static MpqFile? GetUnitObjectDataFile(this Campaign campaign, Encoding? encoding = null)
        {
            if (campaign.UnitObjectData is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(campaign.UnitObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, UnitObjectData.CampaignFileName);
        }

        public static MpqFile? GetUpgradeObjectDataFile(this Campaign campaign, Encoding? encoding = null)
        {
            if (campaign.UpgradeObjectData is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(campaign.UpgradeObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, UpgradeObjectData.CampaignFileName);
        }

        public static MpqFile? GetAbilitySkinObjectDataFile(this Campaign campaign, Encoding? encoding = null)
        {
            if (campaign.AbilitySkinObjectData is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(campaign.AbilitySkinObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, AbilityObjectData.CampaignSkinFileName);
        }

        public static MpqFile? GetBuffSkinObjectDataFile(this Campaign campaign, Encoding? encoding = null)
        {
            if (campaign.BuffSkinObjectData is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(campaign.BuffSkinObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, BuffObjectData.CampaignSkinFileName);
        }

        public static MpqFile? GetDestructableSkinObjectDataFile(this Campaign campaign, Encoding? encoding = null)
        {
            if (campaign.DestructableSkinObjectData is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(campaign.DestructableSkinObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, DestructableObjectData.CampaignSkinFileName);
        }

        public static MpqFile? GetDoodadSkinObjectDataFile(this Campaign campaign, Encoding? encoding = null)
        {
            if (campaign.DoodadSkinObjectData is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(campaign.DoodadSkinObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, DoodadObjectData.CampaignSkinFileName);
        }

        public static MpqFile? GetItemSkinObjectDataFile(this Campaign campaign, Encoding? encoding = null)
        {
            if (campaign.ItemSkinObjectData is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(campaign.ItemSkinObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, ItemObjectData.CampaignSkinFileName);
        }

        public static MpqFile? GetUnitSkinObjectDataFile(this Campaign campaign, Encoding? encoding = null)
        {
            if (campaign.UnitSkinObjectData is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(campaign.UnitSkinObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, UnitObjectData.CampaignSkinFileName);
        }

        public static MpqFile? GetUpgradeSkinObjectDataFile(this Campaign campaign, Encoding? encoding = null)
        {
            if (campaign.UpgradeSkinObjectData is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new BinaryWriter(memoryStream, encoding ?? _defaultEncoding, true);

            writer.Write(campaign.UpgradeSkinObjectData);
            writer.Flush();

            return MpqFile.New(memoryStream, UpgradeObjectData.CampaignSkinFileName);
        }

        public static MpqFile? GetTriggerStringsFile(this Campaign campaign, Encoding? encoding = null)
        {
            if (campaign.TriggerStrings is null)
            {
                return null;
            }

            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, encoding ?? _defaultEncoding, leaveOpen: true);

            writer.WriteTriggerStrings(campaign.TriggerStrings);
            writer.Flush();

            return MpqFile.New(memoryStream, TriggerStrings.CampaignFileName);
        }

        public static void SetImportedFilesFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.ImportedFiles = reader.ReadImportedFiles();
        }

        public static void SetInfoFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.Info = reader.ReadCampaignInfo();
        }

        public static void SetAbilityObjectDataFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.AbilityObjectData = reader.ReadAbilityObjectData();
        }

        public static void SetBuffObjectDataFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.BuffObjectData = reader.ReadBuffObjectData();
        }

        public static void SetDestructableObjectDataFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.DestructableObjectData = reader.ReadDestructableObjectData();
        }

        public static void SetDoodadObjectDataFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.DoodadObjectData = reader.ReadDoodadObjectData();
        }

        public static void SetItemObjectDataFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.ItemObjectData = reader.ReadItemObjectData();
        }

        public static void SetUnitObjectDataFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.UnitObjectData = reader.ReadUnitObjectData();
        }

        public static void SetUpgradeObjectDataFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.UpgradeObjectData = reader.ReadUpgradeObjectData();
        }

        public static void SetAbilitySkinObjectDataFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.AbilitySkinObjectData = reader.ReadAbilityObjectData();
        }

        public static void SetBuffSkinObjectDataFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.BuffSkinObjectData = reader.ReadBuffObjectData();
        }

        public static void SetDestructableSkinObjectDataFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.DestructableSkinObjectData = reader.ReadDestructableObjectData();
        }

        public static void SetDoodadSkinObjectDataFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.DoodadSkinObjectData = reader.ReadDoodadObjectData();
        }

        public static void SetItemSkinObjectDataFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.ItemSkinObjectData = reader.ReadItemObjectData();
        }

        public static void SetUnitSkinObjectDataFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.UnitSkinObjectData = reader.ReadUnitObjectData();
        }

        public static void SetUpgradeSkinObjectDataFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.UpgradeSkinObjectData = reader.ReadUpgradeObjectData();
        }

        public static void SetTriggerStringsFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new StreamReader(stream, encoding ?? _defaultEncoding, leaveOpen: leaveOpen);
            campaign.TriggerStrings = reader.ReadTriggerStrings();
        }

        /// <returns><see langword="true"/> if the file was recognized as a war3campaign file.</returns>
        public static bool SetFile(this Campaign campaign, string fileName, bool overwriteFile, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            switch (fileName.ToLowerInvariant())
            {
#pragma warning disable IDE0011, SA1503
                case ImportedFiles.CampaignFileName: if (campaign.ImportedFiles is null || overwriteFile) campaign.SetImportedFilesFile(stream, encoding, leaveOpen); break;
                case CampaignInfo.FileName: if (campaign.Info is null || overwriteFile) campaign.SetInfoFile(stream, encoding, leaveOpen); break;
                case AbilityObjectData.CampaignFileName: if (campaign.AbilityObjectData is null || overwriteFile) campaign.SetAbilityObjectDataFile(stream, encoding, leaveOpen); break;
                case BuffObjectData.CampaignFileName: if (campaign.BuffObjectData is null || overwriteFile) campaign.SetBuffObjectDataFile(stream, encoding, leaveOpen); break;
                case DestructableObjectData.CampaignFileName: if (campaign.DestructableObjectData is null || overwriteFile) campaign.SetDestructableObjectDataFile(stream, encoding, leaveOpen); break;
                case DoodadObjectData.CampaignFileName: if (campaign.DoodadObjectData is null || overwriteFile) campaign.SetDoodadObjectDataFile(stream, encoding, leaveOpen); break;
                case ItemObjectData.CampaignFileName: if (campaign.ItemObjectData is null || overwriteFile) campaign.SetItemObjectDataFile(stream, encoding, leaveOpen); break;
                case UnitObjectData.CampaignFileName: if (campaign.UnitObjectData is null || overwriteFile) campaign.SetUnitObjectDataFile(stream, encoding, leaveOpen); break;
                case UpgradeObjectData.CampaignFileName: if (campaign.UpgradeObjectData is null || overwriteFile) campaign.SetUpgradeObjectDataFile(stream, encoding, leaveOpen); break;
                case /*AbilityObjectData.CampaignSkinFileName*/ "war3campaignskin.w3a": if (campaign.AbilitySkinObjectData is null || overwriteFile) campaign.SetAbilitySkinObjectDataFile(stream, encoding, leaveOpen); break;
                case /*BuffObjectData.CampaignSkinFileName*/ "war3campaignskin.w3h": if (campaign.BuffSkinObjectData is null || overwriteFile) campaign.SetBuffSkinObjectDataFile(stream, encoding, leaveOpen); break;
                case /*DestructableObjectData.CampaignSkinFileName*/ "war3campaignskin.w3b": if (campaign.DestructableSkinObjectData is null || overwriteFile) campaign.SetDestructableSkinObjectDataFile(stream, encoding, leaveOpen); break;
                case /*DoodadObjectData.CampaignSkinFileName*/ "war3campaignskin.w3d": if (campaign.DoodadSkinObjectData is null || overwriteFile) campaign.SetDoodadSkinObjectDataFile(stream, encoding, leaveOpen); break;
                case /*ItemObjectData.CampaignSkinFileName*/ "war3campaignskin.w3t": if (campaign.ItemSkinObjectData is null || overwriteFile) campaign.SetItemSkinObjectDataFile(stream, encoding, leaveOpen); break;
                case /*UnitObjectData.CampaignSkinFileName*/ "war3campaignskin.w3u": if (campaign.UnitSkinObjectData is null || overwriteFile) campaign.SetUnitSkinObjectDataFile(stream, encoding, leaveOpen); break;
                case /*UpgradeObjectData.CampaignSkinFileName*/ "war3campaignskin.w3q": if (campaign.UpgradeSkinObjectData is null || overwriteFile) campaign.SetUpgradeSkinObjectDataFile(stream, encoding, leaveOpen); break;
                case TriggerStrings.CampaignFileName: if (campaign.TriggerStrings is null || overwriteFile) campaign.SetTriggerStringsFile(stream, encoding, leaveOpen); break;
#pragma warning restore IDE0011, SA1503

                default: return false;
            }

            return true;
        }

        public static void LocalizeInfo(this Campaign campaign)
        {
            var info = campaign.Info;
            var strings = campaign.TriggerStrings;
            if (info is null || strings is null)
            {
                return;
            }

            if (strings.TryGetValue(info.CampaignName, out var campaignName))
            {
                info.CampaignName = campaignName;
            }

            if (strings.TryGetValue(info.CampaignDifficulty, out var campaignDifficulty))
            {
                info.CampaignDifficulty = campaignDifficulty;
            }

            if (strings.TryGetValue(info.CampaignAuthor, out var campaignAuthor))
            {
                info.CampaignAuthor = campaignAuthor;
            }

            if (strings.TryGetValue(info.CampaignDescription, out var campaignDescription))
            {
                info.CampaignDescription = campaignDescription;
            }

            foreach (var mapButton in info.MapButtons)
            {
                if (strings.TryGetValue(mapButton.Chapter, out var chapter))
                {
                    mapButton.Chapter = chapter;
                }

                if (strings.TryGetValue(mapButton.Title, out var title))
                {
                    mapButton.Title = title;
                }
            }
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="CampaignExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using War3Net.Build.Info;
using War3Net.Build.Object;
using War3Net.Build.Script;
using War3Net.IO.Mpq;

namespace War3Net.Build.Extensions
{
    public static class CampaignExtensions
    {
        private static readonly Encoding _defaultEncoding = new UTF8Encoding(false, true);

        public static MpqFile GetInfoFile(this Campaign campaign, Encoding? encoding = null)
        {
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

            return MpqFile.New(memoryStream, CampaignAbilityObjectData.FileName);
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

            return MpqFile.New(memoryStream, CampaignBuffObjectData.FileName);
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

            return MpqFile.New(memoryStream, CampaignDestructableObjectData.FileName);
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

            return MpqFile.New(memoryStream, CampaignDoodadObjectData.FileName);
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

            return MpqFile.New(memoryStream, CampaignItemObjectData.FileName);
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

            return MpqFile.New(memoryStream, CampaignUnitObjectData.FileName);
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

            return MpqFile.New(memoryStream, CampaignUpgradeObjectData.FileName);
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

            return MpqFile.New(memoryStream, CampaignTriggerStrings.FileName);
        }

        public static void SetInfoFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.Info = reader.ReadCampaignInfo();
        }

        public static void SetAbilityObjectDataFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.AbilityObjectData = reader.ReadCampaignAbilityObjectData();
        }

        public static void SetBuffObjectDataFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.BuffObjectData = reader.ReadCampaignBuffObjectData();
        }

        public static void SetDestructableObjectDataFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.DestructableObjectData = reader.ReadCampaignDestructableObjectData();
        }

        public static void SetDoodadObjectDataFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.DoodadObjectData = reader.ReadCampaignDoodadObjectData();
        }

        public static void SetItemObjectDataFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.ItemObjectData = reader.ReadCampaignItemObjectData();
        }

        public static void SetUnitObjectDataFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.UnitObjectData = reader.ReadCampaignUnitObjectData();
        }

        public static void SetUpgradeObjectDataFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new BinaryReader(stream, encoding ?? _defaultEncoding, leaveOpen);
            campaign.UpgradeObjectData = reader.ReadCampaignUpgradeObjectData();
        }

        public static void SetTriggerStringsFile(this Campaign campaign, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            using var reader = new StreamReader(stream, encoding ?? _defaultEncoding, leaveOpen: leaveOpen);
            campaign.TriggerStrings = reader.ReadCampaignTriggerStrings();
        }

        /// <returns><see langword="true"/> if the file was recognized as a war3campaign file.</returns>
        public static bool SetFile(this Campaign campaign, string fileName, bool overwriteFile, Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            switch (fileName.ToLowerInvariant())
            {
#pragma warning disable IDE0011, SA1503
                case CampaignInfo.FileName: if (overwriteFile) campaign.SetInfoFile(stream, encoding, leaveOpen); break;
                case CampaignAbilityObjectData.FileName: if (campaign.AbilityObjectData is null || overwriteFile) campaign.SetAbilityObjectDataFile(stream, encoding, leaveOpen); break;
                case CampaignBuffObjectData.FileName: if (campaign.BuffObjectData is null || overwriteFile) campaign.SetBuffObjectDataFile(stream, encoding, leaveOpen); break;
                case CampaignDestructableObjectData.FileName: if (campaign.DestructableObjectData is null || overwriteFile) campaign.SetDestructableObjectDataFile(stream, encoding, leaveOpen); break;
                case CampaignDoodadObjectData.FileName: if (campaign.DoodadObjectData is null || overwriteFile) campaign.SetDoodadObjectDataFile(stream, encoding, leaveOpen); break;
                case CampaignItemObjectData.FileName: if (campaign.ItemObjectData is null || overwriteFile) campaign.SetItemObjectDataFile(stream, encoding, leaveOpen); break;
                case CampaignUnitObjectData.FileName: if (campaign.UnitObjectData is null || overwriteFile) campaign.SetUnitObjectDataFile(stream, encoding, leaveOpen); break;
                case CampaignUpgradeObjectData.FileName: if (campaign.UpgradeObjectData is null || overwriteFile) campaign.SetUpgradeObjectDataFile(stream, encoding, leaveOpen); break;
                case CampaignTriggerStrings.FileName: if (campaign.TriggerStrings is null || overwriteFile) campaign.SetTriggerStringsFile(stream, encoding, leaveOpen); break;
#pragma warning restore IDE0011, SA1503

                default: return false;
            }

            return true;
        }
    }
}
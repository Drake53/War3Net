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
using War3Net.Common.Providers;
using War3Net.IO.Mpq;

namespace War3Net.Build.Extensions
{
    public static class CampaignExtensions
    {
        private static readonly Encoding _defaultEncoding = UTF8EncodingProvider.StrictUTF8;

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
                case CampaignInfo.FileName: if (overwriteFile) campaign.SetInfoFile(stream, encoding, leaveOpen); break;
                case AbilityObjectData.CampaignFileName: if (campaign.AbilityObjectData is null || overwriteFile) campaign.SetAbilityObjectDataFile(stream, encoding, leaveOpen); break;
                case BuffObjectData.CampaignFileName: if (campaign.BuffObjectData is null || overwriteFile) campaign.SetBuffObjectDataFile(stream, encoding, leaveOpen); break;
                case DestructableObjectData.CampaignFileName: if (campaign.DestructableObjectData is null || overwriteFile) campaign.SetDestructableObjectDataFile(stream, encoding, leaveOpen); break;
                case DoodadObjectData.CampaignFileName: if (campaign.DoodadObjectData is null || overwriteFile) campaign.SetDoodadObjectDataFile(stream, encoding, leaveOpen); break;
                case ItemObjectData.CampaignFileName: if (campaign.ItemObjectData is null || overwriteFile) campaign.SetItemObjectDataFile(stream, encoding, leaveOpen); break;
                case UnitObjectData.CampaignFileName: if (campaign.UnitObjectData is null || overwriteFile) campaign.SetUnitObjectDataFile(stream, encoding, leaveOpen); break;
                case UpgradeObjectData.CampaignFileName: if (campaign.UpgradeObjectData is null || overwriteFile) campaign.SetUpgradeObjectDataFile(stream, encoding, leaveOpen); break;
                case TriggerStrings.CampaignFileName: if (campaign.TriggerStrings is null || overwriteFile) campaign.SetTriggerStringsFile(stream, encoding, leaveOpen); break;
#pragma warning restore IDE0011, SA1503

                default: return false;
            }

            return true;
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="MpqArchiveBuilderExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Linq;
using System.Text;

using War3Net.Build.Info;
using War3Net.IO.Mpq;

namespace War3Net.Build.Extensions
{
    public static class MpqArchiveBuilderExtensions
    {
        private static readonly ulong CampaignInfoHashedFileName = MpqHash.GetHashedFileName(CampaignInfo.FileName);
        private static readonly ulong MapInfoHashedFileName = MpqHash.GetHashedFileName(MapInfo.FileName);

        public static void SaveWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, bool leaveOpen = false)
        {
            var mpqFiles = mpqArchiveBuilder.ToArray();
            var campaignInfoFile = mpqFiles.SingleOrDefault(file => file.Name == CampaignInfoHashedFileName);
            if (campaignInfoFile is not null)
            {
                using var reader = new BinaryReader(campaignInfoFile.MpqStream, new UTF8Encoding(false, true), true);
                var campaignInfo = reader.ReadCampaignInfo();
                campaignInfoFile.MpqStream.Position = 0;
                mpqArchiveBuilder.SaveWithPreArchiveData(stream, campaignInfo, leaveOpen);
            }
            else
            {
                var mapInfoFile = mpqFiles.SingleOrDefault(file => file.Name == MapInfoHashedFileName);
                if (mapInfoFile is not null)
                {
                    using var reader = new BinaryReader(mapInfoFile.MpqStream, new UTF8Encoding(false, true), true);
                    var mapInfo = reader.ReadMapInfo();
                    mapInfoFile.MpqStream.Position = 0;
                    mpqArchiveBuilder.SaveWithPreArchiveData(stream, mapInfo, leaveOpen);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to find {CampaignInfo.FileName} or {MapInfo.FileName} file to use as source for pre-archive data.");
                }
            }
        }

        public static void SaveWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, MpqArchiveCreateOptions createOptions, bool leaveOpen = false)
        {
            var mpqFiles = mpqArchiveBuilder.ToArray();
            var campaignInfoFile = mpqFiles.SingleOrDefault(file => file.Name == CampaignInfoHashedFileName);
            if (campaignInfoFile is not null)
            {
                using var reader = new BinaryReader(campaignInfoFile.MpqStream, new UTF8Encoding(false, true), true);
                var campaignInfo = reader.ReadCampaignInfo();
                campaignInfoFile.MpqStream.Position = 0;
                mpqArchiveBuilder.SaveWithPreArchiveData(stream, createOptions, campaignInfo, leaveOpen);
            }
            else
            {
                var mapInfoFile = mpqFiles.SingleOrDefault(file => file.Name == MapInfoHashedFileName);
                if (mapInfoFile is not null)
                {
                    using var reader = new BinaryReader(mapInfoFile.MpqStream, new UTF8Encoding(false, true), true);
                    var mapInfo = reader.ReadMapInfo();
                    mapInfoFile.MpqStream.Position = 0;
                    mpqArchiveBuilder.SaveWithPreArchiveData(stream, createOptions, mapInfo, leaveOpen);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to find {CampaignInfo.FileName} or {MapInfo.FileName} file to use as source for pre-archive data.");
                }
            }
        }

        public static void SaveWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, CampaignInfo campaignInfo, bool leaveOpen = false)
        {
            campaignInfo.WriteArchiveHeaderToStream(stream);
            mpqArchiveBuilder.SaveTo(stream, leaveOpen);
        }

        public static void SaveWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, MpqArchiveCreateOptions createOptions, CampaignInfo campaignInfo, bool leaveOpen = false)
        {
            campaignInfo.WriteArchiveHeaderToStream(stream);
            mpqArchiveBuilder.SaveTo(stream, createOptions, leaveOpen);
        }

        public static void SaveWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, MapInfo mapInfo, bool leaveOpen = false)
        {
            mapInfo.WriteArchiveHeaderToStream(stream);
            mpqArchiveBuilder.SaveTo(stream, leaveOpen);
        }

        public static void SaveWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, MpqArchiveCreateOptions createOptions, MapInfo mapInfo, bool leaveOpen = false)
        {
            mapInfo.WriteArchiveHeaderToStream(stream);
            mpqArchiveBuilder.SaveTo(stream, createOptions, leaveOpen);
        }
    }
}
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
using War3Net.Build.Script;
using War3Net.Common.Providers;
using War3Net.IO.Mpq;

namespace War3Net.Build.Extensions
{
    public static class MpqArchiveBuilderExtensions
    {
        private static readonly ulong CampaignInfoHashedFileName = MpqHash.GetHashedFileName(CampaignInfo.FileName);
        private static readonly ulong CampaignTriggerStringsHashedFileName = MpqHash.GetHashedFileName(CampaignTriggerStrings.FileName);
        private static readonly ulong MapInfoHashedFileName = MpqHash.GetHashedFileName(MapInfo.FileName);
        private static readonly ulong MapTriggerStringsHashedFileName = MpqHash.GetHashedFileName(MapTriggerStrings.FileName);

        private static readonly Encoding _defaultEncoding = UTF8EncodingProvider.StrictUTF8;

        public static void SaveWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, string fileName)
        {
            var mpqFiles = mpqArchiveBuilder.ToArray();
            var campaignInfoFile = mpqFiles.FirstOrDefault(file => file.Name == CampaignInfoHashedFileName);
            if (campaignInfoFile is not null)
            {
                using var reader = new BinaryReader(campaignInfoFile.MpqStream, _defaultEncoding, true);
                var campaignInfo = reader.ReadCampaignInfo();
                campaignInfoFile.MpqStream.Position = 0;

                var campaignTriggerStringsFile = mpqFiles.FirstOrDefault(file => file.Name == CampaignTriggerStringsHashedFileName);
                if (campaignTriggerStringsFile is not null)
                {
                    using var triggerStringsReader = new StreamReader(campaignTriggerStringsFile.MpqStream, _defaultEncoding, leaveOpen: true);
                    var campaignTriggerStrings = triggerStringsReader.ReadCampaignTriggerStrings();
                    campaignTriggerStringsFile.MpqStream.Position = 0;

                    mpqArchiveBuilder.SaveCampaignWithPreArchiveData(fileName, campaignInfo, campaignTriggerStrings);
                }
                else
                {
                    mpqArchiveBuilder.SaveCampaignWithPreArchiveData(fileName, campaignInfo);
                }
            }
            else
            {
                var mapInfoFile = mpqFiles.FirstOrDefault(file => file.Name == MapInfoHashedFileName);
                if (mapInfoFile is not null)
                {
                    using var reader = new BinaryReader(mapInfoFile.MpqStream, _defaultEncoding, true);
                    var mapInfo = reader.ReadMapInfo();
                    mapInfoFile.MpqStream.Position = 0;

                    var mapTriggerStringsFile = mpqFiles.FirstOrDefault(file => file.Name == MapTriggerStringsHashedFileName);
                    if (mapTriggerStringsFile is not null)
                    {
                        using var triggerStringsReader = new StreamReader(mapTriggerStringsFile.MpqStream, _defaultEncoding, leaveOpen: true);
                        var mapTriggerStrings = triggerStringsReader.ReadMapTriggerStrings();
                        mapTriggerStringsFile.MpqStream.Position = 0;

                        mpqArchiveBuilder.SaveMapWithPreArchiveData(fileName, mapInfo, mapTriggerStrings);
                    }
                    else
                    {
                        mpqArchiveBuilder.SaveMapWithPreArchiveData(fileName, mapInfo);
                    }
                }
                else
                {
                    throw new FileNotFoundException($"Unable to find {CampaignInfo.FileName} or {MapInfo.FileName} file to use as source for pre-archive data.");
                }
            }
        }

        public static void SaveWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, string fileName, MpqArchiveCreateOptions createOptions)
        {
            var mpqFiles = mpqArchiveBuilder.ToArray();
            var campaignInfoFile = mpqFiles.FirstOrDefault(file => file.Name == CampaignInfoHashedFileName);
            if (campaignInfoFile is not null)
            {
                using var reader = new BinaryReader(campaignInfoFile.MpqStream, _defaultEncoding, true);
                var campaignInfo = reader.ReadCampaignInfo();
                campaignInfoFile.MpqStream.Position = 0;

                var campaignTriggerStringsFile = mpqFiles.FirstOrDefault(file => file.Name == CampaignTriggerStringsHashedFileName);
                if (campaignTriggerStringsFile is not null)
                {
                    using var triggerStringsReader = new StreamReader(campaignTriggerStringsFile.MpqStream, _defaultEncoding, leaveOpen: true);
                    var campaignTriggerStrings = triggerStringsReader.ReadCampaignTriggerStrings();
                    campaignTriggerStringsFile.MpqStream.Position = 0;

                    mpqArchiveBuilder.SaveCampaignWithPreArchiveData(fileName, createOptions, campaignInfo, campaignTriggerStrings);
                }
                else
                {
                    mpqArchiveBuilder.SaveCampaignWithPreArchiveData(fileName, createOptions, campaignInfo);
                }
            }
            else
            {
                var mapInfoFile = mpqFiles.FirstOrDefault(file => file.Name == MapInfoHashedFileName);
                if (mapInfoFile is not null)
                {
                    using var reader = new BinaryReader(mapInfoFile.MpqStream, _defaultEncoding, true);
                    var mapInfo = reader.ReadMapInfo();
                    mapInfoFile.MpqStream.Position = 0;

                    var mapTriggerStringsFile = mpqFiles.FirstOrDefault(file => file.Name == MapTriggerStringsHashedFileName);
                    if (mapTriggerStringsFile is not null)
                    {
                        using var triggerStringsReader = new StreamReader(mapTriggerStringsFile.MpqStream, _defaultEncoding, leaveOpen: true);
                        var mapTriggerStrings = triggerStringsReader.ReadMapTriggerStrings();
                        mapTriggerStringsFile.MpqStream.Position = 0;

                        mpqArchiveBuilder.SaveMapWithPreArchiveData(fileName, createOptions, mapInfo, mapTriggerStrings);
                    }
                    else
                    {
                        mpqArchiveBuilder.SaveMapWithPreArchiveData(fileName, createOptions, mapInfo);
                    }
                }
                else
                {
                    throw new FileNotFoundException($"Unable to find {CampaignInfo.FileName} or {MapInfo.FileName} file to use as source for pre-archive data.");
                }
            }
        }

        public static void SaveWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, string fileName, Encoding encoding)
        {
            var mpqFiles = mpqArchiveBuilder.ToArray();
            var campaignInfoFile = mpqFiles.FirstOrDefault(file => file.Name == CampaignInfoHashedFileName);
            if (campaignInfoFile is not null)
            {
                using var reader = new BinaryReader(campaignInfoFile.MpqStream, encoding, true);
                var campaignInfo = reader.ReadCampaignInfo();
                campaignInfoFile.MpqStream.Position = 0;

                var campaignTriggerStringsFile = mpqFiles.FirstOrDefault(file => file.Name == CampaignTriggerStringsHashedFileName);
                if (campaignTriggerStringsFile is not null)
                {
                    using var triggerStringsReader = new StreamReader(campaignTriggerStringsFile.MpqStream, encoding, leaveOpen: true);
                    var campaignTriggerStrings = triggerStringsReader.ReadCampaignTriggerStrings();
                    campaignTriggerStringsFile.MpqStream.Position = 0;

                    mpqArchiveBuilder.SaveCampaignWithPreArchiveData(fileName, campaignInfo, campaignTriggerStrings, encoding);
                }
                else
                {
                    mpqArchiveBuilder.SaveCampaignWithPreArchiveData(fileName, campaignInfo, encoding);
                }
            }
            else
            {
                var mapInfoFile = mpqFiles.FirstOrDefault(file => file.Name == MapInfoHashedFileName);
                if (mapInfoFile is not null)
                {
                    using var reader = new BinaryReader(mapInfoFile.MpqStream, encoding, true);
                    var mapInfo = reader.ReadMapInfo();
                    mapInfoFile.MpqStream.Position = 0;

                    var mapTriggerStringsFile = mpqFiles.FirstOrDefault(file => file.Name == MapTriggerStringsHashedFileName);
                    if (mapTriggerStringsFile is not null)
                    {
                        using var triggerStringsReader = new StreamReader(mapTriggerStringsFile.MpqStream, encoding, leaveOpen: true);
                        var mapTriggerStrings = triggerStringsReader.ReadMapTriggerStrings();
                        mapTriggerStringsFile.MpqStream.Position = 0;

                        mpqArchiveBuilder.SaveMapWithPreArchiveData(fileName, mapInfo, mapTriggerStrings, encoding);
                    }
                    else
                    {
                        mpqArchiveBuilder.SaveMapWithPreArchiveData(fileName, mapInfo, encoding);
                    }
                }
                else
                {
                    throw new FileNotFoundException($"Unable to find {CampaignInfo.FileName} or {MapInfo.FileName} file to use as source for pre-archive data.");
                }
            }
        }

        public static void SaveWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, string fileName, MpqArchiveCreateOptions createOptions, Encoding encoding)
        {
            var mpqFiles = mpqArchiveBuilder.ToArray();
            var campaignInfoFile = mpqFiles.FirstOrDefault(file => file.Name == CampaignInfoHashedFileName);
            if (campaignInfoFile is not null)
            {
                using var reader = new BinaryReader(campaignInfoFile.MpqStream, encoding, true);
                var campaignInfo = reader.ReadCampaignInfo();
                campaignInfoFile.MpqStream.Position = 0;

                var campaignTriggerStringsFile = mpqFiles.FirstOrDefault(file => file.Name == CampaignTriggerStringsHashedFileName);
                if (campaignTriggerStringsFile is not null)
                {
                    using var triggerStringsReader = new StreamReader(campaignTriggerStringsFile.MpqStream, encoding, leaveOpen: true);
                    var campaignTriggerStrings = triggerStringsReader.ReadCampaignTriggerStrings();
                    campaignTriggerStringsFile.MpqStream.Position = 0;

                    mpqArchiveBuilder.SaveCampaignWithPreArchiveData(fileName, createOptions, campaignInfo, campaignTriggerStrings, encoding);
                }
                else
                {
                    mpqArchiveBuilder.SaveCampaignWithPreArchiveData(fileName, createOptions, campaignInfo, encoding);
                }
            }
            else
            {
                var mapInfoFile = mpqFiles.FirstOrDefault(file => file.Name == MapInfoHashedFileName);
                if (mapInfoFile is not null)
                {
                    using var reader = new BinaryReader(mapInfoFile.MpqStream, encoding, true);
                    var mapInfo = reader.ReadMapInfo();
                    mapInfoFile.MpqStream.Position = 0;

                    var mapTriggerStringsFile = mpqFiles.FirstOrDefault(file => file.Name == MapTriggerStringsHashedFileName);
                    if (mapTriggerStringsFile is not null)
                    {
                        using var triggerStringsReader = new StreamReader(mapTriggerStringsFile.MpqStream, encoding, leaveOpen: true);
                        var mapTriggerStrings = triggerStringsReader.ReadMapTriggerStrings();
                        mapTriggerStringsFile.MpqStream.Position = 0;

                        mpqArchiveBuilder.SaveMapWithPreArchiveData(fileName, createOptions, mapInfo, mapTriggerStrings, encoding);
                    }
                    else
                    {
                        mpqArchiveBuilder.SaveMapWithPreArchiveData(fileName, createOptions, mapInfo, encoding);
                    }
                }
                else
                {
                    throw new FileNotFoundException($"Unable to find {CampaignInfo.FileName} or {MapInfo.FileName} file to use as source for pre-archive data.");
                }
            }
        }

        public static void SaveWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, bool leaveOpen = false)
        {
            var mpqFiles = mpqArchiveBuilder.ToArray();
            var campaignInfoFile = mpqFiles.FirstOrDefault(file => file.Name == CampaignInfoHashedFileName);
            if (campaignInfoFile is not null)
            {
                using var reader = new BinaryReader(campaignInfoFile.MpqStream, _defaultEncoding, true);
                var campaignInfo = reader.ReadCampaignInfo();
                campaignInfoFile.MpqStream.Position = 0;

                var campaignTriggerStringsFile = mpqFiles.FirstOrDefault(file => file.Name == CampaignTriggerStringsHashedFileName);
                if (campaignTriggerStringsFile is not null)
                {
                    using var triggerStringsReader = new StreamReader(campaignTriggerStringsFile.MpqStream, _defaultEncoding, leaveOpen: true);
                    var campaignTriggerStrings = triggerStringsReader.ReadCampaignTriggerStrings();
                    campaignTriggerStringsFile.MpqStream.Position = 0;

                    mpqArchiveBuilder.SaveCampaignWithPreArchiveData(stream, campaignInfo, campaignTriggerStrings, leaveOpen);
                }
                else
                {
                    mpqArchiveBuilder.SaveCampaignWithPreArchiveData(stream, campaignInfo, leaveOpen);
                }
            }
            else
            {
                var mapInfoFile = mpqFiles.FirstOrDefault(file => file.Name == MapInfoHashedFileName);
                if (mapInfoFile is not null)
                {
                    using var reader = new BinaryReader(mapInfoFile.MpqStream, _defaultEncoding, true);
                    var mapInfo = reader.ReadMapInfo();
                    mapInfoFile.MpqStream.Position = 0;

                    var mapTriggerStringsFile = mpqFiles.FirstOrDefault(file => file.Name == MapTriggerStringsHashedFileName);
                    if (mapTriggerStringsFile is not null)
                    {
                        using var triggerStringsReader = new StreamReader(mapTriggerStringsFile.MpqStream, _defaultEncoding, leaveOpen: true);
                        var mapTriggerStrings = triggerStringsReader.ReadMapTriggerStrings();
                        mapTriggerStringsFile.MpqStream.Position = 0;

                        mpqArchiveBuilder.SaveMapWithPreArchiveData(stream, mapInfo, mapTriggerStrings, leaveOpen);
                    }
                    else
                    {
                        mpqArchiveBuilder.SaveMapWithPreArchiveData(stream, mapInfo, leaveOpen);
                    }
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
            var campaignInfoFile = mpqFiles.FirstOrDefault(file => file.Name == CampaignInfoHashedFileName);
            if (campaignInfoFile is not null)
            {
                using var reader = new BinaryReader(campaignInfoFile.MpqStream, _defaultEncoding, true);
                var campaignInfo = reader.ReadCampaignInfo();
                campaignInfoFile.MpqStream.Position = 0;

                var campaignTriggerStringsFile = mpqFiles.FirstOrDefault(file => file.Name == CampaignTriggerStringsHashedFileName);
                if (campaignTriggerStringsFile is not null)
                {
                    using var triggerStringsReader = new StreamReader(campaignTriggerStringsFile.MpqStream, _defaultEncoding, leaveOpen: true);
                    var campaignTriggerStrings = triggerStringsReader.ReadCampaignTriggerStrings();
                    campaignTriggerStringsFile.MpqStream.Position = 0;

                    mpqArchiveBuilder.SaveCampaignWithPreArchiveData(stream, createOptions, campaignInfo, campaignTriggerStrings, leaveOpen);
                }
                else
                {
                    mpqArchiveBuilder.SaveCampaignWithPreArchiveData(stream, createOptions, campaignInfo, leaveOpen);
                }
            }
            else
            {
                var mapInfoFile = mpqFiles.FirstOrDefault(file => file.Name == MapInfoHashedFileName);
                if (mapInfoFile is not null)
                {
                    using var reader = new BinaryReader(mapInfoFile.MpqStream, _defaultEncoding, true);
                    var mapInfo = reader.ReadMapInfo();
                    mapInfoFile.MpqStream.Position = 0;

                    var mapTriggerStringsFile = mpqFiles.FirstOrDefault(file => file.Name == MapTriggerStringsHashedFileName);
                    if (mapTriggerStringsFile is not null)
                    {
                        using var triggerStringsReader = new StreamReader(mapTriggerStringsFile.MpqStream, _defaultEncoding, leaveOpen: true);
                        var mapTriggerStrings = triggerStringsReader.ReadMapTriggerStrings();
                        mapTriggerStringsFile.MpqStream.Position = 0;

                        mpqArchiveBuilder.SaveMapWithPreArchiveData(stream, createOptions, mapInfo, mapTriggerStrings, leaveOpen);
                    }
                    else
                    {
                        mpqArchiveBuilder.SaveMapWithPreArchiveData(stream, createOptions, mapInfo, leaveOpen);
                    }
                }
                else
                {
                    throw new FileNotFoundException($"Unable to find {CampaignInfo.FileName} or {MapInfo.FileName} file to use as source for pre-archive data.");
                }
            }
        }

        public static void SaveWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, Encoding encoding, bool leaveOpen = false)
        {
            var mpqFiles = mpqArchiveBuilder.ToArray();
            var campaignInfoFile = mpqFiles.FirstOrDefault(file => file.Name == CampaignInfoHashedFileName);
            if (campaignInfoFile is not null)
            {
                using var reader = new BinaryReader(campaignInfoFile.MpqStream, encoding, true);
                var campaignInfo = reader.ReadCampaignInfo();
                campaignInfoFile.MpqStream.Position = 0;

                var campaignTriggerStringsFile = mpqFiles.FirstOrDefault(file => file.Name == CampaignTriggerStringsHashedFileName);
                if (campaignTriggerStringsFile is not null)
                {
                    using var triggerStringsReader = new StreamReader(campaignTriggerStringsFile.MpqStream, encoding, leaveOpen: true);
                    var campaignTriggerStrings = triggerStringsReader.ReadCampaignTriggerStrings();
                    campaignTriggerStringsFile.MpqStream.Position = 0;

                    mpqArchiveBuilder.SaveCampaignWithPreArchiveData(stream, campaignInfo, campaignTriggerStrings, encoding, leaveOpen);
                }
                else
                {
                    mpqArchiveBuilder.SaveCampaignWithPreArchiveData(stream, campaignInfo, encoding, leaveOpen);
                }
            }
            else
            {
                var mapInfoFile = mpqFiles.FirstOrDefault(file => file.Name == MapInfoHashedFileName);
                if (mapInfoFile is not null)
                {
                    using var reader = new BinaryReader(mapInfoFile.MpqStream, encoding, true);
                    var mapInfo = reader.ReadMapInfo();
                    mapInfoFile.MpqStream.Position = 0;

                    var mapTriggerStringsFile = mpqFiles.FirstOrDefault(file => file.Name == MapTriggerStringsHashedFileName);
                    if (mapTriggerStringsFile is not null)
                    {
                        using var triggerStringsReader = new StreamReader(mapTriggerStringsFile.MpqStream, encoding, leaveOpen: true);
                        var mapTriggerStrings = triggerStringsReader.ReadMapTriggerStrings();
                        mapTriggerStringsFile.MpqStream.Position = 0;

                        mpqArchiveBuilder.SaveMapWithPreArchiveData(stream, mapInfo, mapTriggerStrings, encoding, leaveOpen);
                    }
                    else
                    {
                        mpqArchiveBuilder.SaveMapWithPreArchiveData(stream, mapInfo, encoding, leaveOpen);
                    }
                }
                else
                {
                    throw new FileNotFoundException($"Unable to find {CampaignInfo.FileName} or {MapInfo.FileName} file to use as source for pre-archive data.");
                }
            }
        }

        public static void SaveWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, MpqArchiveCreateOptions createOptions, Encoding encoding, bool leaveOpen = false)
        {
            var mpqFiles = mpqArchiveBuilder.ToArray();
            var campaignInfoFile = mpqFiles.FirstOrDefault(file => file.Name == CampaignInfoHashedFileName);
            if (campaignInfoFile is not null)
            {
                using var reader = new BinaryReader(campaignInfoFile.MpqStream, encoding, true);
                var campaignInfo = reader.ReadCampaignInfo();
                campaignInfoFile.MpqStream.Position = 0;

                var campaignTriggerStringsFile = mpqFiles.FirstOrDefault(file => file.Name == CampaignTriggerStringsHashedFileName);
                if (campaignTriggerStringsFile is not null)
                {
                    using var triggerStringsReader = new StreamReader(campaignTriggerStringsFile.MpqStream, encoding, leaveOpen: true);
                    var campaignTriggerStrings = triggerStringsReader.ReadCampaignTriggerStrings();
                    campaignTriggerStringsFile.MpqStream.Position = 0;

                    mpqArchiveBuilder.SaveCampaignWithPreArchiveData(stream, createOptions, campaignInfo, campaignTriggerStrings, encoding, leaveOpen);
                }
                else
                {
                    mpqArchiveBuilder.SaveCampaignWithPreArchiveData(stream, createOptions, campaignInfo, encoding, leaveOpen);
                }
            }
            else
            {
                var mapInfoFile = mpqFiles.FirstOrDefault(file => file.Name == MapInfoHashedFileName);
                if (mapInfoFile is not null)
                {
                    using var reader = new BinaryReader(mapInfoFile.MpqStream, encoding, true);
                    var mapInfo = reader.ReadMapInfo();
                    mapInfoFile.MpqStream.Position = 0;

                    var mapTriggerStringsFile = mpqFiles.FirstOrDefault(file => file.Name == MapTriggerStringsHashedFileName);
                    if (mapTriggerStringsFile is not null)
                    {
                        using var triggerStringsReader = new StreamReader(mapTriggerStringsFile.MpqStream, encoding, leaveOpen: true);
                        var mapTriggerStrings = triggerStringsReader.ReadMapTriggerStrings();
                        mapTriggerStringsFile.MpqStream.Position = 0;

                        mpqArchiveBuilder.SaveMapWithPreArchiveData(stream, createOptions, mapInfo, mapTriggerStrings, encoding, leaveOpen);
                    }
                    else
                    {
                        mpqArchiveBuilder.SaveMapWithPreArchiveData(stream, createOptions, mapInfo, encoding, leaveOpen);
                    }
                }
                else
                {
                    throw new FileNotFoundException($"Unable to find {CampaignInfo.FileName} or {MapInfo.FileName} file to use as source for pre-archive data.");
                }
            }
        }

        public static void SaveCampaignWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, string fileName, CampaignInfo campaignInfo)
        {
            using (var stream = FileProvider.CreateFileAndFolder(fileName))
            {
                campaignInfo.WriteArchiveHeaderToStream(stream);
                mpqArchiveBuilder.SaveTo(stream);
            }
        }

        public static void SaveCampaignWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, string fileName, MpqArchiveCreateOptions createOptions, CampaignInfo campaignInfo)
        {
            using (var stream = FileProvider.CreateFileAndFolder(fileName))
            {
                campaignInfo.WriteArchiveHeaderToStream(stream);
                mpqArchiveBuilder.SaveTo(stream, createOptions);
            }
        }

        public static void SaveCampaignWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, string fileName, CampaignInfo campaignInfo, CampaignTriggerStrings? campaignTriggerStrings)
        {
            using (var stream = FileProvider.CreateFileAndFolder(fileName))
            {
                campaignInfo.WriteArchiveHeaderToStream(stream, campaignTriggerStrings);
                mpqArchiveBuilder.SaveTo(stream);
            }
        }

        public static void SaveCampaignWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, string fileName, MpqArchiveCreateOptions createOptions, CampaignInfo campaignInfo, CampaignTriggerStrings? campaignTriggerStrings)
        {
            using (var stream = FileProvider.CreateFileAndFolder(fileName))
            {
                campaignInfo.WriteArchiveHeaderToStream(stream, campaignTriggerStrings);
                mpqArchiveBuilder.SaveTo(stream, createOptions);
            }
        }

        public static void SaveCampaignWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, string fileName, CampaignInfo campaignInfo, Encoding encoding)
        {
            using (var stream = FileProvider.CreateFileAndFolder(fileName))
            {
                campaignInfo.WriteArchiveHeaderToStream(stream, encoding);
                mpqArchiveBuilder.SaveTo(stream);
            }
        }

        public static void SaveCampaignWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, string fileName, MpqArchiveCreateOptions createOptions, CampaignInfo campaignInfo, Encoding encoding)
        {
            using (var stream = FileProvider.CreateFileAndFolder(fileName))
            {
                campaignInfo.WriteArchiveHeaderToStream(stream, encoding);
                mpqArchiveBuilder.SaveTo(stream, createOptions);
            }
        }

        public static void SaveCampaignWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, string fileName, CampaignInfo campaignInfo, CampaignTriggerStrings? campaignTriggerStrings, Encoding encoding)
        {
            using (var stream = FileProvider.CreateFileAndFolder(fileName))
            {
                campaignInfo.WriteArchiveHeaderToStream(stream, campaignTriggerStrings, encoding);
                mpqArchiveBuilder.SaveTo(stream);
            }
        }

        public static void SaveCampaignWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, string fileName, MpqArchiveCreateOptions createOptions, CampaignInfo campaignInfo, CampaignTriggerStrings? campaignTriggerStrings, Encoding encoding)
        {
            using (var stream = FileProvider.CreateFileAndFolder(fileName))
            {
                campaignInfo.WriteArchiveHeaderToStream(stream, campaignTriggerStrings, encoding);
                mpqArchiveBuilder.SaveTo(stream, createOptions);
            }
        }

        public static void SaveCampaignWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, CampaignInfo campaignInfo, bool leaveOpen = false)
        {
            campaignInfo.WriteArchiveHeaderToStream(stream);
            mpqArchiveBuilder.SaveTo(stream, leaveOpen);
        }

        public static void SaveCampaignWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, MpqArchiveCreateOptions createOptions, CampaignInfo campaignInfo, bool leaveOpen = false)
        {
            campaignInfo.WriteArchiveHeaderToStream(stream);
            mpqArchiveBuilder.SaveTo(stream, createOptions, leaveOpen);
        }

        public static void SaveCampaignWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, CampaignInfo campaignInfo, CampaignTriggerStrings? campaignTriggerStrings, bool leaveOpen = false)
        {
            campaignInfo.WriteArchiveHeaderToStream(stream, campaignTriggerStrings);
            mpqArchiveBuilder.SaveTo(stream, leaveOpen);
        }

        public static void SaveCampaignWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, MpqArchiveCreateOptions createOptions, CampaignInfo campaignInfo, CampaignTriggerStrings? campaignTriggerStrings, bool leaveOpen = false)
        {
            campaignInfo.WriteArchiveHeaderToStream(stream, campaignTriggerStrings);
            mpqArchiveBuilder.SaveTo(stream, createOptions, leaveOpen);
        }

        public static void SaveCampaignWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, CampaignInfo campaignInfo, Encoding encoding, bool leaveOpen = false)
        {
            campaignInfo.WriteArchiveHeaderToStream(stream, encoding);
            mpqArchiveBuilder.SaveTo(stream, leaveOpen);
        }

        public static void SaveCampaignWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, MpqArchiveCreateOptions createOptions, CampaignInfo campaignInfo, Encoding encoding, bool leaveOpen = false)
        {
            campaignInfo.WriteArchiveHeaderToStream(stream, encoding);
            mpqArchiveBuilder.SaveTo(stream, createOptions, leaveOpen);
        }

        public static void SaveCampaignWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, CampaignInfo campaignInfo, CampaignTriggerStrings? campaignTriggerStrings, Encoding encoding, bool leaveOpen = false)
        {
            campaignInfo.WriteArchiveHeaderToStream(stream, campaignTriggerStrings, encoding);
            mpqArchiveBuilder.SaveTo(stream, leaveOpen);
        }

        public static void SaveCampaignWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, MpqArchiveCreateOptions createOptions, CampaignInfo campaignInfo, CampaignTriggerStrings? campaignTriggerStrings, Encoding encoding, bool leaveOpen = false)
        {
            campaignInfo.WriteArchiveHeaderToStream(stream, campaignTriggerStrings, encoding);
            mpqArchiveBuilder.SaveTo(stream, createOptions, leaveOpen);
        }

        public static void SaveMapWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, string fileName, MapInfo mapInfo)
        {
            using (var stream = FileProvider.CreateFileAndFolder(fileName))
            {
                mapInfo.WriteArchiveHeaderToStream(stream);
                mpqArchiveBuilder.SaveTo(stream);
            }
        }

        public static void SaveMapWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, string fileName, MpqArchiveCreateOptions createOptions, MapInfo mapInfo)
        {
            using (var stream = FileProvider.CreateFileAndFolder(fileName))
            {
                mapInfo.WriteArchiveHeaderToStream(stream);
                mpqArchiveBuilder.SaveTo(stream, createOptions);
            }
        }

        public static void SaveMapWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, string fileName, MapInfo mapInfo, MapTriggerStrings? mapTriggerStrings)
        {
            using (var stream = FileProvider.CreateFileAndFolder(fileName))
            {
                mapInfo.WriteArchiveHeaderToStream(stream, mapTriggerStrings);
                mpqArchiveBuilder.SaveTo(stream);
            }
        }

        public static void SaveMapWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, string fileName, MpqArchiveCreateOptions createOptions, MapInfo mapInfo, MapTriggerStrings? mapTriggerStrings)
        {
            using (var stream = FileProvider.CreateFileAndFolder(fileName))
            {
                mapInfo.WriteArchiveHeaderToStream(stream, mapTriggerStrings);
                mpqArchiveBuilder.SaveTo(stream, createOptions);
            }
        }

        public static void SaveMapWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, string fileName, MapInfo mapInfo, Encoding encoding)
        {
            using (var stream = FileProvider.CreateFileAndFolder(fileName))
            {
                mapInfo.WriteArchiveHeaderToStream(stream, encoding);
                mpqArchiveBuilder.SaveTo(stream);
            }
        }

        public static void SaveMapWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, string fileName, MpqArchiveCreateOptions createOptions, MapInfo mapInfo, Encoding encoding)
        {
            using (var stream = FileProvider.CreateFileAndFolder(fileName))
            {
                mapInfo.WriteArchiveHeaderToStream(stream, encoding);
                mpqArchiveBuilder.SaveTo(stream, createOptions);
            }
        }

        public static void SaveMapWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, string fileName, MapInfo mapInfo, MapTriggerStrings? mapTriggerStrings, Encoding encoding)
        {
            using (var stream = FileProvider.CreateFileAndFolder(fileName))
            {
                mapInfo.WriteArchiveHeaderToStream(stream, mapTriggerStrings, encoding);
                mpqArchiveBuilder.SaveTo(stream);
            }
        }

        public static void SaveMapWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, string fileName, MpqArchiveCreateOptions createOptions, MapInfo mapInfo, MapTriggerStrings? mapTriggerStrings, Encoding encoding)
        {
            using (var stream = FileProvider.CreateFileAndFolder(fileName))
            {
                mapInfo.WriteArchiveHeaderToStream(stream, mapTriggerStrings, encoding);
                mpqArchiveBuilder.SaveTo(stream, createOptions);
            }
        }

        public static void SaveMapWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, MapInfo mapInfo, bool leaveOpen = false)
        {
            mapInfo.WriteArchiveHeaderToStream(stream);
            mpqArchiveBuilder.SaveTo(stream, leaveOpen);
        }

        public static void SaveMapWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, MpqArchiveCreateOptions createOptions, MapInfo mapInfo, bool leaveOpen = false)
        {
            mapInfo.WriteArchiveHeaderToStream(stream);
            mpqArchiveBuilder.SaveTo(stream, createOptions, leaveOpen);
        }

        public static void SaveMapWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, MapInfo mapInfo, MapTriggerStrings? mapTriggerStrings, bool leaveOpen = false)
        {
            mapInfo.WriteArchiveHeaderToStream(stream, mapTriggerStrings);
            mpqArchiveBuilder.SaveTo(stream, leaveOpen);
        }

        public static void SaveMapWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, MpqArchiveCreateOptions createOptions, MapInfo mapInfo, MapTriggerStrings? mapTriggerStrings, bool leaveOpen = false)
        {
            mapInfo.WriteArchiveHeaderToStream(stream, mapTriggerStrings);
            mpqArchiveBuilder.SaveTo(stream, createOptions, leaveOpen);
        }

        public static void SaveMapWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, MapInfo mapInfo, Encoding encoding, bool leaveOpen = false)
        {
            mapInfo.WriteArchiveHeaderToStream(stream, encoding);
            mpqArchiveBuilder.SaveTo(stream, leaveOpen);
        }

        public static void SaveMapWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, MpqArchiveCreateOptions createOptions, MapInfo mapInfo, Encoding encoding, bool leaveOpen = false)
        {
            mapInfo.WriteArchiveHeaderToStream(stream, encoding);
            mpqArchiveBuilder.SaveTo(stream, createOptions, leaveOpen);
        }

        public static void SaveMapWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, MapInfo mapInfo, MapTriggerStrings? mapTriggerStrings, Encoding encoding, bool leaveOpen = false)
        {
            mapInfo.WriteArchiveHeaderToStream(stream, mapTriggerStrings, encoding);
            mpqArchiveBuilder.SaveTo(stream, leaveOpen);
        }

        public static void SaveMapWithPreArchiveData(this MpqArchiveBuilder mpqArchiveBuilder, Stream stream, MpqArchiveCreateOptions createOptions, MapInfo mapInfo, MapTriggerStrings? mapTriggerStrings, Encoding encoding, bool leaveOpen = false)
        {
            mapInfo.WriteArchiveHeaderToStream(stream, mapTriggerStrings, encoding);
            mpqArchiveBuilder.SaveTo(stream, createOptions, leaveOpen);
        }
    }
}
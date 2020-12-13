// ------------------------------------------------------------------------------
// <copyright file="MpqArchiveExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.IO;

using War3Net.Build.Audio;
using War3Net.Build.Environment;
using War3Net.Build.Info;
using War3Net.Build.Object;
using War3Net.Build.Script;
using War3Net.Build.Widget;
using War3Net.IO.Mpq;

namespace War3Net.Build.Extensions
{
    public static class MpqArchiveExtensions
    {
        public static void DiscoverFileNames(this MpqArchive archive)
        {
            archive.AddFilename(Attributes.FileName);
            archive.AddFilename(ListFile.FileName);
            archive.AddFilename(@"(signature)");
            archive.AddFilename(@"(user data)");

            archive.AddFilename(MapSounds.FileName);

            archive.AddFilename(MapCameras.FileName);
            archive.AddFilename(MapEnvironment.FileName);
            archive.AddFilename(MapPathingMap.FileName);
            archive.AddFilename(MapPreviewIcons.FileName);
            archive.AddFilename(MapRegions.FileName);
            archive.AddFilename(MapShadowMap.FileName);
            archive.AddFilename(@"war3mapMap.blp");
            archive.AddFilename(@"war3mapMap.tga");
            archive.AddFilename(@"war3mapPath.tga");
            archive.AddFilename(@"war3mapPreview.blp");
            archive.AddFilename(@"war3mapPreview.tga");

            archive.AddFilename(CampaignInfo.FileName);
            archive.AddFilename(MapInfo.FileName);

            archive.AddFilename(CampaignAbilityObjectData.FileName);
            archive.AddFilename(CampaignBuffObjectData.FileName);
            archive.AddFilename(CampaignDestructableObjectData.FileName);
            archive.AddFilename(CampaignDoodadObjectData.FileName);
            archive.AddFilename(CampaignItemObjectData.FileName);
            archive.AddFilename(CampaignUnitObjectData.FileName);
            archive.AddFilename(CampaignUpgradeObjectData.FileName);
            archive.AddFilename(MapAbilityObjectData.FileName);
            archive.AddFilename(MapBuffObjectData.FileName);
            archive.AddFilename(MapDestructableObjectData.FileName);
            archive.AddFilename(MapDoodadObjectData.FileName);
            archive.AddFilename(MapItemObjectData.FileName);
            archive.AddFilename(MapUnitObjectData.FileName);
            archive.AddFilename(MapUpgradeObjectData.FileName);

            archive.AddFilename(CampaignTriggerStrings.FileName);
            archive.AddFilename(MapCustomTextTriggers.FileName);
            archive.AddFilename(MapTriggers.FileName);
            archive.AddFilename(MapTriggerStrings.FileName);
            archive.AddFilename(@"scripts\war3map.j");
            archive.AddFilename(@"war3map.j");
            archive.AddFilename(@"war3map.lua");

            archive.AddFilename(MapDoodads.FileName);
            archive.AddFilename(MapUnits.FileName);

            archive.AddFilename(@"conversation.json");
            archive.AddFilename(@"war3mapExtra.txt");
            archive.AddFilename(@"war3mapMisc.txt");
            archive.AddFilename(@"war3mapSkin.txt");
            archive.AddFilename(@"war3map.imp");

            if (archive.IsCampaignArchive(out var campaignInfo))
            {
                for (var i = 0; i < campaignInfo.Maps.Count; i++)
                {
                    archive.AddFilename(campaignInfo.Maps[i].MapFilePath);
                }
            }
        }

        public static bool IsCampaignArchive(this MpqArchive archive, [NotNullWhen(true)] out CampaignInfo? campaignInfo)
        {
            if (archive.TryAddFilename(CampaignInfo.FileName))
            {
                using var campaignInfoFileStream = archive.OpenFile(CampaignInfo.FileName);
                using var reader = new BinaryReader(campaignInfoFileStream);
                campaignInfo = reader.ReadCampaignInfo();
                return true;
            }

            campaignInfo = null;
            return false;
        }
    }
}
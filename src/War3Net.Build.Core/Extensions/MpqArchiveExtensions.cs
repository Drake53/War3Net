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
using War3Net.Build.Import;
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
            archive.AddFileName(Attributes.FileName);
            archive.AddFileName(ListFile.FileName);
            archive.AddFileName(Signature.FileName);
            archive.AddFileName(UserData.FileName);

            archive.AddFileName(MapSounds.FileName);

            archive.AddFileName(MapCameras.FileName);
            archive.AddFileName(MapEnvironment.FileName);
            archive.AddFileName(MapPathingMap.FileName);
            archive.AddFileName(MapPreviewIcons.FileName);
            archive.AddFileName(MapRegions.FileName);
            archive.AddFileName(MapShadowMap.FileName);
            archive.AddFileName(@"war3mapMap.blp");
            archive.AddFileName(@"war3mapMap.tga");
            archive.AddFileName(@"war3mapPath.tga");
            archive.AddFileName(@"war3mapPreview.blp");
            archive.AddFileName(@"war3mapPreview.tga");

            archive.AddFileName(CampaignInfo.FileName);
            archive.AddFileName(MapInfo.FileName);

            archive.AddFileName(CampaignAbilityObjectData.FileName);
            archive.AddFileName(CampaignBuffObjectData.FileName);
            archive.AddFileName(CampaignDestructableObjectData.FileName);
            archive.AddFileName(CampaignDoodadObjectData.FileName);
            archive.AddFileName(CampaignItemObjectData.FileName);
            archive.AddFileName(CampaignUnitObjectData.FileName);
            archive.AddFileName(CampaignUpgradeObjectData.FileName);
            archive.AddFileName(MapAbilityObjectData.FileName);
            archive.AddFileName(MapBuffObjectData.FileName);
            archive.AddFileName(MapDestructableObjectData.FileName);
            archive.AddFileName(MapDoodadObjectData.FileName);
            archive.AddFileName(MapItemObjectData.FileName);
            archive.AddFileName(MapUnitObjectData.FileName);
            archive.AddFileName(MapUpgradeObjectData.FileName);
            archive.AddFileName("war3mapSkin.w3a");
            archive.AddFileName("war3mapSkin.w3h");
            archive.AddFileName("war3mapSkin.w3b");
            archive.AddFileName("war3mapSkin.w3d");
            archive.AddFileName("war3mapSkin.w3t");
            archive.AddFileName("war3mapSkin.w3u");
            archive.AddFileName("war3mapSkin.w3q");

            archive.AddFileName(CampaignTriggerStrings.FileName);
            archive.AddFileName(MapCustomTextTriggers.FileName);
            archive.AddFileName(MapTriggers.FileName);
            archive.AddFileName(MapTriggerStrings.FileName);
            archive.AddFileName(JassMapScript.FileName);
            archive.AddFileName(JassMapScript.FullName);
            archive.AddFileName(LuaMapScript.FileName);
            archive.AddFileName(LuaMapScript.FullName);

            archive.AddFileName(MapDoodads.FileName);
            archive.AddFileName(MapUnits.FileName);

            archive.AddFileName(@"conversation.json");
            archive.AddFileName(@"war3mapExtra.txt");
            archive.AddFileName(@"war3mapMisc.txt");
            archive.AddFileName(@"war3mapSkin.txt");
            archive.AddFileName(CampaignImportedFiles.FileName);
            archive.AddFileName(MapImportedFiles.FileName);

            if (archive.IsCampaignArchive(out var campaignInfo))
            {
                for (var i = 0; i < campaignInfo.Maps.Count; i++)
                {
                    archive.AddFileName(campaignInfo.Maps[i].MapFilePath);
                }
            }
        }

        public static bool IsCampaignArchive(this MpqArchive archive, [NotNullWhen(true)] out CampaignInfo? campaignInfo)
        {
            if (archive.TryOpenFile(CampaignInfo.FileName, out var campaignInfoFileStream))
            {
                using var reader = new BinaryReader(campaignInfoFileStream);
                campaignInfo = reader.ReadCampaignInfo();
                return true;
            }

            campaignInfo = null;
            return false;
        }
    }
}
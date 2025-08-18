// ------------------------------------------------------------------------------
// <copyright file="TestDataFileProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Core.Tests
{
    public static class TestDataFileProvider
    {
        public static IEnumerable<object[]> GetFilePathsForTestDataType(TestDataFileType testDataFileType)
        {
            var parameters = TestDataFileParams.Get(testDataFileType);

            var result = TestDataProvider.GetDynamicData(
                parameters.SearchPattern,
                SearchOption.AllDirectories,
                parameters.FolderName);

            foreach (var knownFileName in parameters.KnownFileNames)
            {
                var folderName = knownFileName.StartsWith("war3campaign", StringComparison.Ordinal)
                    ? "Campaigns"
                    : "Maps";

                result = result.Concat(TestDataProvider.GetDynamicArchiveData(
                    knownFileName,
                    SearchOption.AllDirectories,
                    folderName));
            }

            return result;
        }

        public static IEnumerable<object[]> GetMapSoundsFilePaths() => GetFilePathsForTestDataType(TestDataFileType.MapSounds);

        public static IEnumerable<object[]> GetGameConfigurationFilePaths() => GetFilePathsForTestDataType(TestDataFileType.GameConfiguration);

        public static IEnumerable<object[]> GetMapCamerasFilePaths() => GetFilePathsForTestDataType(TestDataFileType.MapCameras);

        public static IEnumerable<object[]> GetMapEnvironmentFilePaths() => GetFilePathsForTestDataType(TestDataFileType.MapEnvironment);

        public static IEnumerable<object[]> GetMapPathingMapFilePaths() => GetFilePathsForTestDataType(TestDataFileType.MapPathingMap);

        public static IEnumerable<object[]> GetMapPreviewIconsFilePaths() => GetFilePathsForTestDataType(TestDataFileType.MapPreviewIcons);

        public static IEnumerable<object[]> GetMapRegionsFilePaths() => GetFilePathsForTestDataType(TestDataFileType.MapRegions);

        public static IEnumerable<object[]> GetMapShadowMapFilePaths() => GetFilePathsForTestDataType(TestDataFileType.MapShadowMap);

        public static IEnumerable<object[]> GetImportedFilesFilePaths() => GetFilePathsForTestDataType(TestDataFileType.ImportedFiles);

        public static IEnumerable<object[]> GetCampaignInfoFilePaths() => GetFilePathsForTestDataType(TestDataFileType.CampaignInfo);

        public static IEnumerable<object[]> GetMapInfoFilePaths() => GetFilePathsForTestDataType(TestDataFileType.MapInfo);

        public static IEnumerable<object[]> GetAbilityObjectDataFilePaths() => GetFilePathsForTestDataType(TestDataFileType.AbilityObjectData);

        public static IEnumerable<object[]> GetBuffObjectDataFilePaths() => GetFilePathsForTestDataType(TestDataFileType.BuffObjectData);

        public static IEnumerable<object[]> GetDestructableObjectDataFilePaths() => GetFilePathsForTestDataType(TestDataFileType.DestructableObjectData);

        public static IEnumerable<object[]> GetDoodadObjectDataFilePaths() => GetFilePathsForTestDataType(TestDataFileType.DoodadObjectData);

        public static IEnumerable<object[]> GetItemObjectDataFilePaths() => GetFilePathsForTestDataType(TestDataFileType.ItemObjectData);

        public static IEnumerable<object[]> GetUnitObjectDataFilePaths() => GetFilePathsForTestDataType(TestDataFileType.UnitObjectData);

        public static IEnumerable<object[]> GetUpgradeObjectDataFilePaths() => GetFilePathsForTestDataType(TestDataFileType.UpgradeObjectData);

        public static IEnumerable<object[]> GetObjectDataFilePaths() => GetFilePathsForTestDataType(TestDataFileType.ObjectData);

        public static IEnumerable<object[]> GetMapCustomTextTriggersFilePaths() => GetFilePathsForTestDataType(TestDataFileType.MapCustomTextTriggers);

        public static IEnumerable<object[]> GetMapTriggersFilePaths() => GetFilePathsForTestDataType(TestDataFileType.MapTriggers);

        public static IEnumerable<object[]> GetTriggerStringsFilePaths() => GetFilePathsForTestDataType(TestDataFileType.TriggerStrings);

        public static IEnumerable<object[]> GetMapDoodadsFilePaths() => GetFilePathsForTestDataType(TestDataFileType.MapDoodads);

        public static IEnumerable<object[]> GetMapUnitsFilePaths() => GetFilePathsForTestDataType(TestDataFileType.MapUnits);
    }
}
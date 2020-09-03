// ------------------------------------------------------------------------------
// <copyright file="MapFileHandlerTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Audio;
using War3Net.Build.Common;
using War3Net.Build.Environment;
using War3Net.Build.Info;
using War3Net.Build.Object;
using War3Net.Build.Widget;

namespace War3Net.Build.Tests
{
    [TestClass]
    public class MapFileHandlerTests
    {
        [TestMethod]
        public void TestMapFileClassesValid()
        {
            _ = new MapFileHandler<MapInfo>();
            _ = new MapFileHandler<MapEnvironment>();
            _ = new MapFileHandler<MapDoodads>();
            _ = new MapFileHandler<MapUnits>();
            _ = new MapFileHandler<MapRegions>();
            _ = new MapFileHandler<MapSounds>();
            _ = new MapFileHandler<MapPreviewIcons>();

            _ = new MapFileHandler<MapUnitObjectData>();
            _ = new MapFileHandler<MapItemObjectData>();
            _ = new MapFileHandler<MapDestructableObjectData>();
            _ = new MapFileHandler<MapDoodadObjectData>();
            _ = new MapFileHandler<MapUnitObjectData>();
            _ = new MapFileHandler<MapBuffObjectData>();
            _ = new MapFileHandler<MapUpgradeObjectData>();

            _ = new MapFileHandler<CampaignUnitObjectData>();
            _ = new MapFileHandler<CampaignItemObjectData>();
            _ = new MapFileHandler<CampaignDestructableObjectData>();
            _ = new MapFileHandler<CampaignDoodadObjectData>();
            _ = new MapFileHandler<CampaignUnitObjectData>();
            _ = new MapFileHandler<CampaignBuffObjectData>();
            _ = new MapFileHandler<CampaignUpgradeObjectData>();
        }
    }
}
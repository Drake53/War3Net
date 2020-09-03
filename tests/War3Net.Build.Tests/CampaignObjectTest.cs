// ------------------------------------------------------------------------------
// <copyright file="CampaignObjectTest.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Object;

namespace War3Net.Build.Tests
{
    [TestClass]
    public class CampaignObjectTest
    {
        [TestMethod]
        public void TestCreateNewObjectData()
        {
            var objectData = new CampaignObjectData(
                new CampaignUnitObjectData(Array.Empty<ObjectModification>()),
                new CampaignItemObjectData(Array.Empty<ObjectModification>()),
                new CampaignDestructableObjectData(Array.Empty<ObjectModification>()),
                new CampaignDoodadObjectData(Array.Empty<ObjectModification>()),
                new CampaignAbilityObjectData(Array.Empty<ObjectModification>()),
                new CampaignBuffObjectData(Array.Empty<ObjectModification>()),
                new CampaignUpgradeObjectData(Array.Empty<ObjectModification>()));

            using var memoryStream = new MemoryStream();
            objectData.SerializeTo(memoryStream, true);

            memoryStream.Position = 0;
            CampaignObjectData.Parse(memoryStream);
        }
    }
}
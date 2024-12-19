// ------------------------------------------------------------------------------
// <copyright file="MapUnitsDecompilerTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build;
using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.CodeAnalysis.Jass;
using War3Net.Common.Extensions;
using War3Net.TestTools.UnitTesting;

namespace War3Net.CodeAnalysis.Decompilers.Tests.Widget
{
    [TestClass]
    public class MapUnitsDecompilerTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestDecompileMapUnits(Map testMap)
        {
            var map = new JassScriptDecompiler(JassSyntaxFactory.ParseCompilationUnit(testMap.Script), new DecompileOptions() { mapWidgetsFormatVersion = testMap.Units.FormatVersion, mapWidgetsSubVersion = testMap.Units.SubVersion, mapWidgetsUseNewFormat = testMap.Units.UseNewFormat }, testMap.Info).DecompileObjectManagerData();
            var decompiledMapUnits = map.Units;

            var expectedMapUnits = map.Units.Units
#if true
                .Where(unit => !unit.IsRandomUnit())
                .Where(unit => !unit.IsRandomBuilding())
#endif
                .OrderBy(unit => unit.TypeId)
                .ThenBy(unit => unit.SkinId)
                .ThenBy(unit => unit.Position.X)
                .ThenBy(unit => unit.Position.Y)
                .ToList();

            var actualMapUnits = decompiledMapUnits.Units
                .OrderBy(unit => unit.TypeId)
                .ThenBy(unit => unit.SkinId)
                .ThenBy(unit => unit.Position.X)
                .ThenBy(unit => unit.Position.Y)
                .ToList();

            Assert.AreEqual(expectedMapUnits.Count, actualMapUnits.Count);
            for (var i = 0; i < decompiledMapUnits.Units.Count; i++)
            {
                var expectedUnit = expectedMapUnits[i];
                var actualUnit = actualMapUnits[i];

                const float deltaPrecision1 = 0.055f;
                const float deltaPrecision3 = 0.00055f;

                if (expectedUnit.IsPlayerStartLocation())
                {
                    Assert.IsTrue(actualUnit.IsPlayerStartLocation());

                    Assert.AreEqual(expectedUnit.Position.X, actualUnit.Position.X, 32f);
                    Assert.AreEqual(expectedUnit.Position.Y, actualUnit.Position.Y, 32f);
                    Assert.AreEqual(expectedUnit.OwnerId, actualUnit.OwnerId);
                }
                else
                {
                    Assert.AreEqual(expectedUnit.TypeId, actualUnit.TypeId, $"\r\nExpected: '{expectedUnit.TypeId.ToRawcode()}'. Actual: '{actualUnit.TypeId.ToRawcode()}'.");
                    Assert.AreEqual(expectedUnit.Position.X, actualUnit.Position.X, deltaPrecision1);
                    Assert.AreEqual(expectedUnit.Position.Y, actualUnit.Position.Y, deltaPrecision1);
                    Assert.AreEqual(expectedUnit.Rotation, actualUnit.Rotation, deltaPrecision3);
                    Assert.AreEqual(expectedUnit.Scale, actualUnit.Scale);
                    Assert.AreEqual(expectedUnit.Flags, actualUnit.Flags);
                    Assert.AreEqual(expectedUnit.OwnerId, actualUnit.OwnerId);
                    Assert.AreEqual(expectedUnit.Unk1, actualUnit.Unk1);
                    Assert.AreEqual(expectedUnit.Unk2, actualUnit.Unk2);
                    Assert.AreEqual(expectedUnit.HP, actualUnit.HP);
                    Assert.AreEqual(expectedUnit.MP, actualUnit.MP);
                    Assert.AreEqual(expectedUnit.TargetAcquisition, actualUnit.TargetAcquisition);
                    Assert.AreEqual(expectedUnit.HeroLevel, actualUnit.HeroLevel);
                    Assert.AreEqual(expectedUnit.HeroStrength, actualUnit.HeroStrength);
                    Assert.AreEqual(expectedUnit.HeroAgility, actualUnit.HeroAgility);
                    Assert.AreEqual(expectedUnit.HeroIntelligence, actualUnit.HeroIntelligence);
                    Assert.AreEqual(expectedUnit.WaygateDestinationRegionId, actualUnit.WaygateDestinationRegionId);

                    if (map.Units.UseNewFormat)
                    {
                        Assert.AreEqual(expectedUnit.SkinId, actualUnit.SkinId, $"\r\nExpected: '{expectedUnit.SkinId.ToRawcode()}'. Actual: '{actualUnit.SkinId.ToRawcode()}'.");
                    }
                }
            }
        }

        private static IEnumerable<object[]> GetTestData()
        {
            foreach (var mapPath in TestDataProvider.GetDynamicData("*", SearchOption.AllDirectories, "Maps"))
            {
                if (Map.TryOpen((string)mapPath[0], out var map, MapFiles.Info | MapFiles.Script | MapFiles.Units) &&
                    map.Info is not null &&
                    map.Units is not null &&
                    map.Units.Units.Count > 0 &&
                    map.Info.ScriptLanguage == ScriptLanguage.Jass &&
                    !string.IsNullOrEmpty(map.Script))
                {
                    yield return new[] { map };
                }
            }
        }
    }
}
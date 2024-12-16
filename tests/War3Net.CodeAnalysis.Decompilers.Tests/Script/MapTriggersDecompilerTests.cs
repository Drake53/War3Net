// ------------------------------------------------------------------------------
// <copyright file="MapTriggersDecompilerTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build;
using War3Net.Build.Info;
using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass;
using War3Net.TestTools.UnitTesting;

namespace War3Net.CodeAnalysis.Decompilers.Tests.Script
{
    [TestClass]
    public class MapTriggersDecompilerTests
    {
        [DataTestMethod]
        [DynamicData(nameof(GetTestData), DynamicDataSourceType.Method)]
        public void TestDecompileMapTriggers(Map map)
        {
            var decompiler = new JassScriptDecompiler(JassSyntaxFactory.ParseCompilationUnit(map.Script), new DecompileOptions(), map.Info);
            Assert.IsTrue(decompiler.TryDecompileMapTriggers(map.Triggers.FormatVersion, map.Triggers.SubVersion, out var decompiledMapTriggers), "Failed to decompile map triggers.");

            Assert.AreEqual(map.Triggers.Variables.Count, decompiledMapTriggers.Variables.Count);
            for (var i = 0; i < decompiledMapTriggers.Variables.Count; i++)
            {
                var expectedVariable = map.Triggers.Variables[i];
                var actualVariable = decompiledMapTriggers.Variables[i];

                // TODO
            }

            var expectedTriggerItems = map.Triggers.TriggerItems
                .Where(triggerItem => triggerItem is TriggerDefinition triggerDefinition && triggerDefinition.IsEnabled)
                .ToList();

            var actualTriggerItems = decompiledMapTriggers.TriggerItems
                .Where(triggerItem => triggerItem is TriggerDefinition triggerDefinition && triggerDefinition.IsEnabled)
                .ToList();

            Assert.AreEqual(expectedTriggerItems.Count, actualTriggerItems.Count);
            for (var i = 0; i < actualTriggerItems.Count; i++)
            {
                var expectedTrigger = expectedTriggerItems[i];
                var actualTrigger = actualTriggerItems[i];

                Assert.AreEqual(expectedTrigger.Type, actualTrigger.Type);
                // Assert.AreEqual(expectedTrigger.ParentId, actualTrigger.ParentId);

                if (expectedTrigger.Type == TriggerItemType.Gui)
                {
                    Assert.IsInstanceOfType(expectedTrigger, typeof(TriggerDefinition));
                    Assert.IsInstanceOfType(actualTrigger, typeof(TriggerDefinition));

                    var expectedTriggerDefinition = (TriggerDefinition)expectedTrigger;
                    var actualTriggerDefinition = (TriggerDefinition)actualTrigger;

                    TriggerAssert.AreEqual(expectedTriggerDefinition, actualTriggerDefinition);
                }
            }
        }

        private static IEnumerable<object[]> GetTestData()
        {
            foreach (var mapPath in TestDataProvider.GetDynamicData("*", SearchOption.AllDirectories, "Maps"))
            {
                if (Map.TryOpen((string)mapPath[0], out var map, MapFiles.Info | MapFiles.Script | MapFiles.Triggers) &&
                    map.Info is not null &&
                    map.Triggers is not null &&
                    map.Triggers.FormatVersion != MapTriggersFormatVersion.v3 &&
                    map.Triggers.FormatVersion != MapTriggersFormatVersion.v6 &&
                    (map.Triggers.Variables.Count > 0 || map.Triggers.TriggerItems.Any(triggerItem => triggerItem is not DeletedTriggerItem)) &&
                    map.Info.ScriptLanguage == ScriptLanguage.Jass &&
                    !string.IsNullOrEmpty(map.Script))
                {
                    yield return new[] { map };
                }
            }
        }
    }
}
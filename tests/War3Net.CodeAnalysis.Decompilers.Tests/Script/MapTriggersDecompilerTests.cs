// ------------------------------------------------------------------------------
// <copyright file="MapTriggersDecompilerTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build;
using War3Net.Build.Script;
using War3Net.TestTools.UnitTesting;

namespace War3Net.CodeAnalysis.Decompilers.Tests.Script
{
    [TestClass]
    public class MapTriggersDecompilerTests
    {
        private const MapFiles FilesToOpen = MapFiles.Info | MapFiles.Script | MapFiles.Triggers;

        [TestMethod]
        [FlakyDynamicTestData(FilesToOpen, "ExampleMap203.w3x", "142119.w3m", "306784.w3x", "310421.w3x", "310540.w3x")]
        public void TestDecompileMapTriggers(string mapFilePath)
        {
            var map = Map.Open(mapFilePath, FilesToOpen);

            Assert.IsTrue(new JassScriptDecompiler(map).TryDecompileMapTriggers(map.Triggers.FormatVersion, map.Triggers.SubVersion, out var decompiledMapTriggers), "Failed to decompile map triggers.");

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
    }
}
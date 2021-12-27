// ------------------------------------------------------------------------------
// <copyright file="MapScriptBuilderTests.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using War3Net.Build.Info;
using War3Net.CodeAnalysis.Jass;
using War3Net.TestTools.UnitTesting;

namespace War3Net.Build.Tests
{
    [TestClass]
    public partial class MapScriptBuilderTests
    {
        private static List<MapScriptBuilderTestData> _testData;

        [ClassInitialize]
        public static void Init(TestContext testContext)
        {
            _testData = new();
            foreach (var data in GetMapPaths())
            {
                var mapPath = (string)data[0];
                if (Map.TryOpen(mapPath, out var map) &&
                    map.Info is not null &&
                    map.Info.ScriptLanguage == ScriptLanguage.Jass &&
                    !string.IsNullOrEmpty(map.Script) &&
                    JassSyntaxFactory.TryParseCompilationUnit(map.Script, out var compilationUnit))
                {
                    _testData.Add(new MapScriptBuilderTestData(map, compilationUnit));
                }
            }
        }

        private static IEnumerable<object[]> GetUnobfuscatedTestData()
        {
            return _testData.Where(d => !d.IsObfuscated).Select(d => new object[] { d });
        }

        private static IEnumerable<object[]> GetMapPaths()
        {
            foreach (var data in TestDataProvider.GetDynamicData("*", SearchOption.AllDirectories, "Maps"))
            {
                yield return data;
            }
        }
    }
}
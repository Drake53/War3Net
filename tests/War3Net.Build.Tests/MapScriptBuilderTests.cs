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
            return TestDataProvider.GetDynamicData("*", SearchOption.AllDirectories, "Maps");
        }

        private static void AssertFunctionGeneratedCorrectly(
            MapScriptBuilderTestData testData,
            string functionName,
            Action<IndentedTextWriter> generateFunc)
        {
            using var stringWriter = new StringWriter();
            stringWriter.NewLine = JassSymbol.CarriageReturnLineFeed;
            using var writer = new IndentedTextWriter(stringWriter);

            generateFunc.Invoke(writer);

            var expected = testData.DeclaredFunctions[functionName];
            var actual = JassSyntaxFactory.ParseTopLevelDeclaration(stringWriter.ToString());

            SyntaxAssert.AreEqual(expected, actual);
        }
    }
}
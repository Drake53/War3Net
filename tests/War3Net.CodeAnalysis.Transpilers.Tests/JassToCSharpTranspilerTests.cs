namespace War3Net.CodeAnalysis.Transpilers.Tests
{
    [TestClass]
    public class JassToCSharpTranspilerTests
    {
        [TestMethod]
        [DynamicData(nameof(GetTranspilationFilePaths), DynamicDataSourceType.Method)]
        public void TestTranspileToCSharp(string inputJassFilePath, string expectedCSharpFilePath)
        {
            using var fileStream = File.OpenRead(inputJassFilePath);
            using var outputWriter = new StringWriter();

            GenerateTranspiledScript(fileStream, outputWriter);

            var expectedScript = File.ReadAllText(expectedCSharpFilePath);
            var actualScript = outputWriter.ToString();

            DiffAssert.AreEqual(expectedScript, actualScript);
        }

        private static IEnumerable<object?[]> GetTranspilationFilePaths()
        {
            foreach (var jassFilePath in Directory.EnumerateFiles(TestDataProvider.GetPath("Transpilation"), "*.j", SearchOption.TopDirectoryOnly))
            {
                var csharpFilePath = Path.ChangeExtension(jassFilePath, ".cs");
                if (File.Exists(csharpFilePath))
                {
                    yield return new object?[] { jassFilePath, csharpFilePath };
                }
            }
        }

        private void GenerateTranspiledScript(Stream inputFileStream, TextWriter outputWriter)
        {
            var transpiler = new JassToCSharpTranspiler();
            transpiler.ApplyCSharpLuaTemplateAttribute = false;

            using var mapScriptReader = new StreamReader(inputFileStream);
            var mapScript = mapScriptReader.ReadToEnd();
            var mapScriptSyntax = JassSyntaxFactory.ParseCompilationUnit(mapScript);
            var memberDeclarations = transpiler.Transpile(mapScriptSyntax);

            foreach (var memberDeclaration in memberDeclarations)
            {
                outputWriter.Write(memberDeclaration.ToFullString());
            }

            outputWriter.Write(transpiler.Transpile(mapScriptSyntax.EndOfFileToken).ToFullString());
        }
    }
}
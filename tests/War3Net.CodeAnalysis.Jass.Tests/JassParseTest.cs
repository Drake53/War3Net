// ------------------------------------------------------------------------------
// <copyright file="JassParseTest.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace War3Net.CodeAnalysis.Jass.Tests
{
    [TestClass]
    public class JassParseTest
    {
        [DataTestMethod]
        [DynamicData(nameof(GetOperatorPrecedenceTests), DynamicDataSourceType.Method)]
        public void TestOperatorPrecedence(string inputFilePath)
        {
            var usingDirective = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("War3Net.CodeAnalysis.Common"));

            var fileParser = new JassParser(new JassTokenizer(File.ReadAllText(inputFilePath)));
            var jassFile = fileParser.Parse();

            var compilationUnit = JassTranspiler.Transpile(
                jassFile,
                "JassTranspiledCode",
                "Common",
                false,
                usingDirective).NormalizeWhitespace();

            var cSharpCompilation = PrepareCompilation(
                compilationUnit,
                OutputKind.DynamicallyLinkedLibrary,
                "War3Net.CodeAnalysis.CSharp.Output.Test",
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(Assembly.Load("System.Runtime, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a").Location));

            using var commonPeStream = new MemoryStream();
            using var commonMetadataStream = new MemoryStream();

            var emitOptions = new EmitOptions(false, includePrivateMembers: false);
            var commonEmit = cSharpCompilation.Emit(commonPeStream, metadataPEStream: commonMetadataStream, options: emitOptions);

            Assert.IsTrue(commonEmit.Success, GenerateDiagnosticReport(commonEmit));
        }

        /// <summary>
        /// Copied from <see cref="CSharp.CompilationHelper.PrepareCompilation(CompilationUnitSyntax, OutputKind, string, MetadataReference[])"/>.
        /// Adjusted to not use the workaround for incorrect operator precedence parsing.
        /// </summary>
        private static CSharpCompilation PrepareCompilation(CompilationUnitSyntax compilationUnit, OutputKind outputKind, string assemblyName, params MetadataReference[] references)
        {
            var syntaxTree = SyntaxFactory.SyntaxTree(compilationUnit);
            var compilationOptions = new CSharpCompilationOptions(outputKind);

            return CSharpCompilation.Create(assemblyName, new[] { syntaxTree }, references, compilationOptions);
        }

        private static IEnumerable<object[]> GetOperatorPrecedenceTests()
        {
            yield return new[] { "TestData/OperatorPrecedenceTest1.j" };
        }

        private static string GenerateDiagnosticReport(EmitResult emit, int diagnosticLimit = 10)
        {
            var message = new StringBuilder("Compilation failed with {0} errors and {1} warnings:\r\n");

            var errorCount = 0;
            var warningCount = 0;
            foreach (var diagnostic in emit.Diagnostics)
            {
                if (errorCount + warningCount < diagnosticLimit)
                {
                    message.AppendLine(diagnostic.ToString());
                }

                if (diagnostic.Severity == DiagnosticSeverity.Error)
                {
                    errorCount++;
                }

                if (diagnostic.Severity == DiagnosticSeverity.Warning)
                {
                    warningCount++;
                }
            }

            if (errorCount + warningCount > diagnosticLimit)
            {
                message.Append($"{errorCount + warningCount - diagnosticLimit} more diagnostics have been omitted.");
            }

            return string.Format(message.ToString(), errorCount, warningCount);
        }
    }
}
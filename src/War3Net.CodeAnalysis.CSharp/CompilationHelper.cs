// ------------------------------------------------------------------------------
// <copyright file="CompilationHelper.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;
using System.Text;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace War3Net.CodeAnalysis.CSharp
{
    public static class CompilationHelper
    {
        public static CSharpCompilation PrepareCompilation(CompilationUnitSyntax compilationUnit, OutputKind outputKind, string assemblyName, params MetadataReference[] references)
        {
            // Parse the compilationUnit's text instead of passing it directly to the SyntaxTree method, so the tree gets rebuilt with correct operator precedence.
            // var syntaxTree = SyntaxFactory.SyntaxTree(compilationUnit);
            var syntaxTree = SyntaxFactory.ParseSyntaxTree(compilationUnit.GetText());
            var compilationOptions = new CSharpCompilationOptions(outputKind);

            return CSharpCompilation.Create(assemblyName, new[] { syntaxTree }, references, compilationOptions);
        }

        public static void SerializeTo(CompilationUnitSyntax compilationUnit, Stream stream, bool normalizeWhitespace = true, bool leaveOpen = false)
        {
            using (var streamWriter = new StreamWriter(stream, new UTF8Encoding(false, true), 1024, leaveOpen))
            {
                (normalizeWhitespace ? compilationUnit.NormalizeWhitespace() : compilationUnit).WriteTo(streamWriter);
            }
        }
    }
}
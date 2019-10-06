// ------------------------------------------------------------------------------
// <copyright file="JassTranspiler.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;

using War3Net.CodeAnalysis.CSharp;
using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.CodeAnalysis.Jass.Transpilers;

namespace War3Net.CodeAnalysis.Jass
{
    public static class JassTranspiler
    {
        // If bool nativeAttributes is true, assume only the API is needed, so in that case transform FunctionSyntax into NativeFunctionDeclarationSyntax.
        public static CompilationUnitSyntax Transpile(FileSyntax file, string namespaceName, string className, bool nativeAttributes, params UsingDirectiveSyntax[] usingDirectives)
        {
#pragma warning disable SA1116 // Split parameters should start on line after declaration
            return JassTranspilerHelper.GetCompilationUnit(new SyntaxList<UsingDirectiveSyntax>(usingDirectives),
                   JassTranspilerHelper.GetNamespaceDeclaration(namespaceName,
                   JassTranspilerHelper.GetClassDeclaration(className, file.Transpile(nativeAttributes), nativeAttributes)));
#pragma warning restore SA1116 // Split parameters should start on line after declaration
        }

        public static void TranspileTypesToLua(FileSyntax file, string namespaceAndClassPrefix, string outputPath)
        {
            new FileInfo(outputPath).Directory.Create();
            using (var fileStream = File.OpenWrite(outputPath))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.WriteLine("local define = System.defStc");
                    streamWriter.WriteLine("local setmetatable = setmetatable");
                    streamWriter.WriteLine();

                    WriteDeclr(streamWriter, "handle", null);

                    foreach (var declaration in file.DeclarationList)
                    {
                        var typeDef = declaration.Declaration.TypeDefinition;
                        if (typeDef != null)
                        {
                            WriteDeclr(streamWriter, typeDef.NewTypeNameNode.ValueText, typeDef.BaseTypeNode.HandleIdentifierNode.ValueText);
                        }
                    }
                }
            }

            void WriteDeclr(TextWriter writer, string newType, string baseType)
            {
                writer.WriteLine($"local {newType} = define(\"{namespaceAndClassPrefix}.{newType}\", {{");

                if (baseType != null)
                {
                    writer.WriteLine($"  __inherits__ = {{ {baseType} }}");
                }

                writer.WriteLine("})");
            }
        }

        public static bool CompileCSharpFromJass(
            string filePath,
            string assemblyName,
            string namespaceName,
            string className,
            MetadataReference[] metadataReferences,
            UsingDirectiveSyntax[] usingDirectives,
            out MetadataReference outReference,
            out UsingDirectiveSyntax outDirective,
            out EmitResult emitResult,
            OutputKind outputKind,
            bool applyNativeMemberAttributes = false,
            // TODO: create object that can retrieve these instead of forcing the output to be written to a file
            string outputSource = null,
            string outputEmit = null,
            string outputLuaTypes = null)
        {
            var directiveName = $"{namespaceName}.{className}";
            outDirective = SyntaxFactory.UsingDirective(SyntaxFactory.Token(SyntaxKind.StaticKeyword), null, SyntaxFactory.ParseName(directiveName));

            if (outputLuaTypes != null)
            {
                // These two are incompatible, because enums can't freely inherit from other types (example: igamestate and fgamestate enums inherit from gamestate).
                TranspileToEnumHandler.Reset();
            }

            var fileSyntax = JassParser.ParseFile(filePath);

            if (outputLuaTypes != null)
            {
                // Output lua source code for JASS type definitions.
                TranspileTypesToLua(fileSyntax, $"{namespaceName}{className}", outputLuaTypes);
            }

            var compilationUnit = Transpile(
                fileSyntax,
                namespaceName,
                className,
                applyNativeMemberAttributes,
                usingDirectives).NormalizeWhitespace();

            if (outputSource != null)
            {
                new FileInfo(outputSource).Directory.Create();

                // Output C# source code.
#pragma warning disable CA2000 // Dispose objects before losing scope
                // TODO: fix existing file not overwritten properly (if new length shorter than old length)
                CompilationHelper.SerializeTo(compilationUnit, File.OpenWrite(outputSource), false);
#pragma warning restore CA2000 // Dispose objects before losing scope
            }

            var compilation = CompilationHelper.PrepareCompilation(
                compilationUnit,
                outputKind,
                assemblyName ?? directiveName,
                metadataReferences);

            if (outputEmit is null)
            {
                var peStream = new MemoryStream();
                emitResult = compilation.Emit(peStream, options: new EmitOptions(metadataOnly: true)); // TODO: set metadataOnly to applyNativeMemberAttributes?
                peStream.Seek(0, SeekOrigin.Begin);

                if (emitResult.Success)
                {
                    outReference = MetadataReference.CreateFromStream(peStream);
                    return true;
                }

                peStream.Dispose();
            }
            else
            {
                new FileInfo(outputEmit).Directory.Create();

                // Output .dll file.
                emitResult = compilation.Emit(outputEmit);

                if (emitResult.Success)
                {
                    outReference = MetadataReference.CreateFromFile(outputEmit);
                    return true;
                }
            }

            outReference = null;
            return false;
        }

        public static bool GetReferencesAndUsingDirectives(
            out MetadataReference[] metadataReferences,
            out UsingDirectiveSyntax[] usingDirectives,
            out EmitResult emitResult,
            bool referenceCommon,
            bool referenceBlizzard)
        {
            if (!referenceCommon && referenceBlizzard)
            {
                throw new ArgumentException("Referencing Blizzard.j requires that common.j is also referenced.");
            }

            metadataReferences = GetBasicReferences().ToArray();
            usingDirectives = null;

            if (referenceCommon)
            {
                const string ApiNamespaceName = "War3Api";
                const string CommonClassName = "Common";
                const string BlizzardClassName = "Blizzard";

                var cSharpDirective = GetCSharpDirective();

                if (!referenceBlizzard)
                {
                    TranspileToEnumHandler.DefineEnumTypes(CommonEnumTypesProvider.GetEnumTypes());
                }

                if (!CompileCSharpFromJass(
                    @"JassApi\common.j",
                    null,
                    ApiNamespaceName,
                    CommonClassName,
                    metadataReferences,
                    new[] { cSharpDirective },
                    out var commonReference,
                    out var commonDirective,
                    out emitResult,
                    OutputKind.DynamicallyLinkedLibrary,
                    true,
                    null,
                    null,
                    null))
                {
                    return false;
                }

                metadataReferences = metadataReferences.Append(commonReference).ToArray();

                if (referenceBlizzard)
                {
                    if (!CompileCSharpFromJass(
                        @"JassApi\Blizzard.j",
                        null,
                        ApiNamespaceName,
                        BlizzardClassName,
                        metadataReferences,
                        new[] { cSharpDirective, commonDirective },
                        out var blizzardReference,
                        out var blizzardDirective,
                        out emitResult,
                        OutputKind.DynamicallyLinkedLibrary,
                        true,
                        null,
                        null,
                        null))
                    {
                        return false;
                    }

                    metadataReferences = metadataReferences.Append(blizzardReference).ToArray();

                    usingDirectives = new[] { commonDirective, blizzardDirective };
                    return true;
                }

                usingDirectives = new[] { commonDirective };
                return true;
            }

            usingDirectives = Array.Empty<UsingDirectiveSyntax>();
            emitResult = null;
            return true;
        }

        private static IEnumerable<MetadataReference> GetBasicReferences()
        {
            yield return MetadataReference.CreateFromFile(typeof(object).Assembly.Location);
            yield return MetadataReference.CreateFromFile(Assembly.Load("netstandard, Version=2.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51").Location);
            yield return MetadataReference.CreateFromFile(typeof(CSharp.Attributes.NativeLuaMemberAttribute).Assembly.Location);
        }

        private static UsingDirectiveSyntax GetCSharpDirective()
        {
            return SyntaxFactory.UsingDirective(
                SyntaxFactory.ParseName("War3Net.CodeAnalysis.CSharp.Attributes")) // TODO: not hardcoded name
                .WithLeadingTrivia(SyntaxFactory.Trivia(
                    SyntaxFactory.PragmaWarningDirectiveTrivia(
                        SyntaxFactory.Token(SyntaxKind.DisableKeyword),
                        default(SeparatedSyntaxList<Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax>).AddRange(new[]
                        {
                            SyntaxFactory.ParseExpression("IDE0052"),
                            SyntaxFactory.ParseExpression("IDE1006"),
                            SyntaxFactory.ParseExpression("CS0626"),
                        }),
                        true)));
        }
    }
}
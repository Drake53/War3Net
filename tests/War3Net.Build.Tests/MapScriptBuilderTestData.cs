// ------------------------------------------------------------------------------
// <copyright file="MapScriptBuilderTestData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.Build.Tests
{
    public class MapScriptBuilderTestData
    {
        public MapScriptBuilderTestData(Map map, JassCompilationUnitSyntax compilationUnit)
        {
            Map = map;
            CompilationUnit = compilationUnit;

            MapScriptBuilder = new MapScriptBuilder();
            MapScriptBuilder.SetDefaultOptionsForMap(map);

            var builder = ImmutableDictionary.CreateBuilder<string, JassFunctionDeclarationSyntax>(StringComparer.Ordinal);
            foreach (var declaration in CompilationUnit.Declarations)
            {
                if (declaration is JassFunctionDeclarationSyntax functionDeclaration)
                {
                    builder.Add(functionDeclaration.FunctionDeclarator.IdentifierName.Name, functionDeclaration);
                }
            }

            DeclaredFunctions = builder.ToImmutable();

            IsObfuscated = !Map.Script!.StartsWith("//===========================================================================", StringComparison.Ordinal);
        }

        public Map Map { get; }

        public JassCompilationUnitSyntax CompilationUnit { get; }

        public MapScriptBuilder MapScriptBuilder { get; }

        public ImmutableDictionary<string, JassFunctionDeclarationSyntax> DeclaredFunctions { get; }

        public bool IsObfuscated { get; }

        public override string? ToString() => Map.ToString();
    }
}
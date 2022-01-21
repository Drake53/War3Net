// ------------------------------------------------------------------------------
// <copyright file="MapScriptBuilderTestData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Immutable;

using War3Net.Build.Info;
using War3Net.Build.Script;
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

            if (Map.Info is not null && Map.Info.MapFlags.HasFlag(MapFlags.MeleeMap))
            {
                IsMeleeWithoutTrigger = true;
                if (Map.Triggers is not null)
                {
                    foreach (var triggerItem in Map.Triggers.TriggerItems)
                    {
                        if (triggerItem is TriggerDefinition triggerDefinition)
                        {
                            if (triggerDefinition.Functions.Count > 0)
                            {
                                IsMeleeWithoutTrigger = false;
                            }
                        }
                    }
                }
            }
        }

        public Map Map { get; }

        public JassCompilationUnitSyntax CompilationUnit { get; }

        public MapScriptBuilder MapScriptBuilder { get; }

        public ImmutableDictionary<string, JassFunctionDeclarationSyntax> DeclaredFunctions { get; }

        public bool IsObfuscated { get; }

        public bool IsMeleeWithoutTrigger { get; }

        public override string? ToString() => Map.ToString();
    }
}
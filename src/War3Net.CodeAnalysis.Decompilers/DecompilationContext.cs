// ------------------------------------------------------------------------------
// <copyright file="DecompilationContext.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Immutable;

using War3Net.Build;
using War3Net.Build.Info;
using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    internal sealed class DecompilationContext
    {
        public DecompilationContext(Map map, Campaign? campaign, TriggerData? triggerData)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (map.Info is null)
            {
                throw new Exception();
            }

            if (map.Info.ScriptLanguage != ScriptLanguage.Jass)
            {
                throw new Exception();
            }

            if (string.IsNullOrEmpty(map.Script))
            {
                throw new Exception();
            }

            ObjectData = new ObjectDataContext(map, campaign);
            TriggerData = new TriggerDataContext(triggerData);

            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(map.Script);

            var functionDeclarationsBuilder = ImmutableDictionary.CreateBuilder<string, FunctionDeclarationContext>(StringComparer.Ordinal);
            var variableDeclarationsBuilder = ImmutableDictionary.CreateBuilder<string, VariableDeclarationContext>(StringComparer.Ordinal);

            foreach (var declaration in compilationUnit.Declarations)
            {
                if (declaration is JassFunctionDeclarationSyntax functionDeclaration)
                {
                    functionDeclarationsBuilder.Add(functionDeclaration.FunctionDeclarator.IdentifierName.Token.Text, new FunctionDeclarationContext(functionDeclaration));
                }
                else if (declaration is JassGlobalsDeclarationSyntax globalsDeclaration)
                {
                    foreach (var globalDeclaration in globalsDeclaration.GlobalDeclarations)
                    {
                        if (globalDeclaration is JassGlobalVariableDeclarationSyntax globalVariableDeclaration)
                        {
                            var variableDeclarationContext = new VariableDeclarationContext(globalVariableDeclaration);
                            variableDeclarationsBuilder.Add(variableDeclarationContext.Name, variableDeclarationContext);
                        }
                    }
                }
            }

            FunctionDeclarations = functionDeclarationsBuilder.ToImmutable();
            VariableDeclarations = variableDeclarationsBuilder.ToImmutable();

            ImportedFileNames = new(StringComparer.OrdinalIgnoreCase);

            MaxPlayerSlots = map.Info.EditorVersion >= EditorVersion.v6060 ? 24 : 12;
        }

        public ObjectDataContext ObjectData { get; }

        public TriggerDataContext TriggerData { get; }

        public ImmutableDictionary<string, FunctionDeclarationContext> FunctionDeclarations { get; }

        public ImmutableDictionary<string, VariableDeclarationContext> VariableDeclarations { get; }

        public HashSet<string> ImportedFileNames { get; }

        public int MaxPlayerSlots { get; }
    }
}
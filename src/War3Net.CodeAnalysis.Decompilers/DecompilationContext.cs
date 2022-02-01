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
    internal class DecompilationContext
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

            var comments = new List<JassCommentSyntax>();
            var functionDeclarationsBuilder = ImmutableDictionary.CreateBuilder<string, FunctionDeclarationContext>(StringComparer.Ordinal);
            var variableDeclarationsBuilder = ImmutableDictionary.CreateBuilder<string, VariableDeclarationContext>(StringComparer.Ordinal);

            foreach (var declaration in compilationUnit.Declarations)
            {
                if (declaration is JassCommentSyntax comment)
                {
                    comments.Add(comment);
                }
                else
                {
                    if (declaration is JassFunctionDeclarationSyntax functionDeclaration)
                    {
                        functionDeclarationsBuilder.Add(functionDeclaration.FunctionDeclarator.IdentifierName.Name, new FunctionDeclarationContext(functionDeclaration, comments));
                    }
                    else if (declaration is JassGlobalDeclarationListSyntax globalDeclarationList)
                    {
                        foreach (var declaration2 in globalDeclarationList.Globals)
                        {
                            if (declaration2 is JassGlobalDeclarationSyntax globalDeclaration)
                            {
                                variableDeclarationsBuilder.Add(globalDeclaration.Declarator.IdentifierName.Name, new VariableDeclarationContext(globalDeclaration));
                            }
                        }
                    }

                    comments.Clear();
                }
            }

            FunctionDeclarations = functionDeclarationsBuilder.ToImmutable();
            VariableDeclarations = variableDeclarationsBuilder.ToImmutable();

            ImportedFileNames = new(StringComparer.OrdinalIgnoreCase);
        }

        public ObjectDataContext ObjectData { get; }

        public TriggerDataContext TriggerData { get; }

        public ImmutableDictionary<string, FunctionDeclarationContext> FunctionDeclarations { get; }

        public ImmutableDictionary<string, VariableDeclarationContext> VariableDeclarations { get; }

        public HashSet<string> ImportedFileNames { get; }
    }
}
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
            TriggerData = triggerData ?? TriggerData.Default;

            var compilationUnit = JassSyntaxFactory.ParseCompilationUnit(map.Script);

            var comments = new List<JassCommentDeclarationSyntax>();
            var functionDeclarationsBuilder = ImmutableDictionary.CreateBuilder<string, FunctionDeclarationContext>(StringComparer.Ordinal);
            var variableDeclarationsBuilder = ImmutableDictionary.CreateBuilder<string, VariableDeclarationContext>(StringComparer.Ordinal);

            foreach (var declaration in compilationUnit.Declarations)
            {
                if (declaration is JassCommentDeclarationSyntax commentDeclaration)
                {
                    comments.Add(commentDeclaration);
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
        }

        public ObjectDataContext ObjectData { get; }

        public TriggerData TriggerData { get; }

        public ImmutableDictionary<string, FunctionDeclarationContext> FunctionDeclarations { get; }

        public ImmutableDictionary<string, VariableDeclarationContext> VariableDeclarations { get; }
    }
}
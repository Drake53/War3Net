// ------------------------------------------------------------------------------
// <copyright file="IdentifiersProvider.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.CodeAnalysis.Jass
{
    public static class IdentifiersProvider
    {
        public static IEnumerable<string> GetIdentifiers(string filePath)
        {
            var fileSyntax = LegacyJassParser.ParseFile( filePath );

            foreach (var declarationNode in fileSyntax.DeclarationList)
            {
                var declr = declarationNode.Declaration;
                if (declr.GlobalsBlock != null)
                {
                    foreach (var globalDeclaration in declr.GlobalsBlock.GlobalDeclarationListNode)
                    {
                        yield return globalDeclaration.ConstantDeclarationNode?.IdentifierNameNode.ValueText
                                  ?? globalDeclaration.VariableDeclarationNode.DeclarationNode.VariableDefinitionNode?.IdentifierNameNode.ValueText
                                  ?? globalDeclaration.VariableDeclarationNode.DeclarationNode.ArrayDefinitionNode.IdentifierNameNode.ValueText;
                    }
                }
                else if (declr.TypeDefinition != null)
                {
                    yield return declr.TypeDefinition.NewTypeNameNode.ValueText;
                }
                else
                {
                    yield return declr.NativeFunctionDeclaration.FunctionDeclarationNode.IdentifierNode.ValueText;
                }
            }

            foreach (var functionNode in fileSyntax.FunctionList)
            {
                yield return functionNode.FunctionDeclarationNode.IdentifierNode.ValueText;
            }
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="JassToLuaTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Reflection;

using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        private readonly Dictionary<string, SyntaxTokenType> _functionReturnTypes;
        private readonly Dictionary<string, SyntaxTokenType> _globalTypes;
        private readonly Dictionary<string, SyntaxTokenType> _localTypes;

        public JassToLuaTranspiler()
        {
            _functionReturnTypes = new(StringComparer.Ordinal);
            _globalTypes = new(StringComparer.Ordinal);
            _localTypes = new(StringComparer.Ordinal);
        }

        public void RegisterJassFile(FileSyntax file)
        {
            foreach (var declaration in file.DeclarationList)
            {
                if (declaration.Declaration.GlobalsBlock is not null)
                {
                    foreach (var globalDeclaration in declaration.Declaration.GlobalsBlock.GlobalDeclarationListNode)
                    {
                        if (globalDeclaration.ConstantDeclarationNode is not null)
                        {
                            RegisterGlobalVariableType(globalDeclaration.ConstantDeclarationNode);
                        }
                        else if (globalDeclaration.VariableDeclarationNode is not null)
                        {
                            if (globalDeclaration.VariableDeclarationNode.DeclarationNode.VariableDefinitionNode is not null)
                            {
                                RegisterVariableType(globalDeclaration.VariableDeclarationNode.DeclarationNode.VariableDefinitionNode, false);
                            }
                            else if (globalDeclaration.VariableDeclarationNode.DeclarationNode.ArrayDefinitionNode is not null)
                            {
                                RegisterVariableType(globalDeclaration.VariableDeclarationNode.DeclarationNode.ArrayDefinitionNode, false);
                            }
                        }
                    }
                }
                else if (declaration.Declaration.NativeFunctionDeclaration is not null)
                {
                    RegisterFunctionReturnType(declaration.Declaration.NativeFunctionDeclaration.FunctionDeclarationNode);
                }
            }

            foreach (var function in file.FunctionList)
            {
                RegisterFunctionReturnType(function.FunctionDeclarationNode);
            }
        }

        public void RegisterFunctionReturnType(MethodInfo method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            _functionReturnTypes.Add(method.Name, method.ReturnType.ToJassTypeKeyword());
        }

        public void RegisterGlobalVariableType(FieldInfo field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            _globalTypes.Add(field.Name, field.FieldType.ToJassTypeKeyword());
        }

        private void RegisterFunctionReturnType(FunctionDeclarationSyntax functionDeclaration)
        {
            _functionReturnTypes.Add(functionDeclaration.IdentifierNode.ValueText, functionDeclaration.ReturnTypeNode.TypeNameNode?.TypeNameToken.TokenType ?? SyntaxTokenType.NothingKeyword);
        }

        private void RegisterGlobalVariableType(GlobalConstantDeclarationSyntax globalConstantDeclaration)
        {
            _globalTypes.Add(globalConstantDeclaration.IdentifierNameNode.ValueText, globalConstantDeclaration.TypeNameNode.TypeNameToken.TokenType);
        }

        private void RegisterLocalVariableType(TypeReferenceSyntax typeReference)
        {
            _localTypes.Add(typeReference.TypeReferenceNameToken.ValueText, typeReference.TypeNameNode.TypeNameToken.TokenType);
        }

        private void RegisterVariableType(ArrayDefinitionSyntax arrayDefinition, bool isLocalDeclaration)
        {
            (isLocalDeclaration ? _localTypes : _globalTypes).Add(arrayDefinition.IdentifierNameNode.ValueText, arrayDefinition.TypeNameNode.TypeNameToken.TokenType);
        }

        private void RegisterVariableType(VariableDefinitionSyntax variableDefinition, bool isLocalDeclaration)
        {
            (isLocalDeclaration ? _localTypes : _globalTypes).Add(variableDefinition.IdentifierNameNode.ValueText, variableDefinition.TypeNameNode.TypeNameToken.TokenType);
        }

        private SyntaxTokenType GetFunctionReturnType(string functionName)
        {
            return _functionReturnTypes.TryGetValue(functionName, out var type)
                ? type
#if DEBUG
                : SyntaxTokenType.NullKeyword;
#else
                : throw new KeyNotFoundException($"Function '{functionName}' could not be found.");
#endif
        }

        private SyntaxTokenType GetVariableType(string variableName)
        {
            return (_localTypes.TryGetValue(variableName, out var type) || _globalTypes.TryGetValue(variableName, out type))
                ? type
#if DEBUG
                : SyntaxTokenType.NullKeyword;
#else
                : throw new KeyNotFoundException($"Variable '{variableName}' could not be found.");
#endif
        }
    }
}
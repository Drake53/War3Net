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
        private readonly Dictionary<JassIdentifierNameSyntax, JassTypeSyntax> _functionReturnTypes;
        private readonly Dictionary<JassIdentifierNameSyntax, JassTypeSyntax> _globalTypes;
        private readonly Dictionary<JassIdentifierNameSyntax, JassTypeSyntax> _localTypes;

        public JassToLuaTranspiler()
        {
            _functionReturnTypes = new();
            _globalTypes = new();
            _localTypes = new();
        }

        public void RegisterJassFile(JassCompilationUnitSyntax compilationUnit)
        {
            foreach (var declaration in compilationUnit.Declarations)
            {
                if (declaration is JassGlobalDeclarationListSyntax globalDeclarationList)
                {
                    foreach (var global in globalDeclarationList.Globals)
                    {
                        if (global is JassGlobalDeclarationSyntax globalDeclaration)
                        {
                            RegisterVariableType(globalDeclaration.Declarator, false);
                        }
                    }
                }
                else if (declaration is JassNativeFunctionDeclarationSyntax nativeFunctionDeclaration)
                {
                    RegisterFunctionReturnType(nativeFunctionDeclaration.FunctionDeclarator);
                }
                else if (declaration is JassFunctionDeclarationSyntax functionDeclaration)
                {
                    RegisterFunctionReturnType(functionDeclaration.FunctionDeclarator);
                }
            }
        }

        public void RegisterFunctionReturnType(MethodInfo method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            _functionReturnTypes.Add(JassSyntaxFactory.ParseIdentifierName(method.Name), method.ReturnType.ToJassType());
        }

        public void RegisterGlobalVariableType(FieldInfo field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            _globalTypes.Add(JassSyntaxFactory.ParseIdentifierName(field.Name), field.FieldType.ToJassType());
        }

        private void RegisterFunctionReturnType(JassFunctionDeclaratorSyntax functionDeclarator)
        {
            _functionReturnTypes.Add(functionDeclarator.IdentifierName, functionDeclarator.ReturnType);
        }

        private void RegisterVariableType(IVariableDeclarator declarator, bool isLocalDeclaration)
        {
            switch (declarator)
            {
                case JassArrayDeclaratorSyntax arrayDeclarator: (isLocalDeclaration ? _localTypes : _globalTypes).Add(arrayDeclarator.IdentifierName, arrayDeclarator.Type); break;
                case JassVariableDeclaratorSyntax variableDeclarator: (isLocalDeclaration ? _localTypes : _globalTypes).Add(variableDeclarator.IdentifierName, variableDeclarator.Type); break;
            }
        }

        private void RegisterLocalVariableType(JassParameterSyntax parameter)
        {
            _localTypes.Add(parameter.IdentifierName, parameter.Type);
        }

        private JassTypeSyntax GetFunctionReturnType(JassIdentifierNameSyntax functionName)
        {
            return _functionReturnTypes.TryGetValue(functionName, out var type)
                ? type
#if DEBUG
                : JassTypeSyntax.Nothing;
#else
                : throw new KeyNotFoundException($"Function '{functionName}' could not be found.");
#endif
        }

        private JassTypeSyntax GetVariableType(JassIdentifierNameSyntax variableName)
        {
            return (_localTypes.TryGetValue(variableName, out var type) || _globalTypes.TryGetValue(variableName, out type))
                ? type
#if DEBUG
                : JassTypeSyntax.Nothing;
#else
                : throw new KeyNotFoundException($"Variable '{variableName}' could not be found.");
#endif
        }
    }
}
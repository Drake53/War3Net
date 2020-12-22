// ------------------------------------------------------------------------------
// <copyright file="FunctionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public partial class JassToLuaTranspiler
    {
        public IEnumerable<LuaStatementSyntax> Transpile(FunctionSyntax function)
        {
            _ = function ?? throw new ArgumentNullException(nameof(function));

            var parameters = Transpile(function.FunctionDeclarationNode.ParameterListReferenceNode);
            var functionIdentifier = TranspileIdentifier(function.FunctionDeclarationNode.IdentifierNode);

            RegisterFunctionReturnType(function.FunctionDeclarationNode);

            var functionExpression = new LuaFunctionExpressionSyntax();
            functionExpression.AddParameters(parameters);
            functionExpression.Body.Statements.AddRange(Transpile(function.DeclarationLineDelimiterNode));
            functionExpression.Body.Statements.AddRange(Transpile(function.LocalVariableListNode));
            functionExpression.Body.Statements.AddRange(Transpile(function.StatementListNode));
            functionExpression.RenderAsFunctionDefinition = true;

            _localTypes.Clear();

            var functionDeclaration = new LuaVariableDeclaratorSyntax(functionIdentifier, functionExpression);
            functionDeclaration.IsLocalDeclaration = false;

            var declaration = new LuaVariableListDeclarationSyntax();
            declaration.Variables.Add(functionDeclaration);

            return new LuaStatementSyntax[] { new LuaLocalDeclarationStatementSyntax(declaration) }.Concat(Transpile(function.LastLineDelimiterNode));
        }
    }
}
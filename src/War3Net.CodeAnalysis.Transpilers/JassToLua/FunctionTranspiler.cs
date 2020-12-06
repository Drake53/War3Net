// ------------------------------------------------------------------------------
// <copyright file="FunctionTranspiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CSharpLua.LuaAst;

using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Transpilers
{
    public static partial class JassToLuaTranspiler
    {
        [Obsolete]
        public static void Transpile(this FunctionSyntax functionNode, ref StringBuilder sb)
        {
            _ = functionNode ?? throw new ArgumentNullException(nameof(functionNode));

            functionNode.FunctionDeclarationNode.Transpile(ref sb);
            functionNode.DeclarationLineDelimiterNode.Transpile(ref sb);
            functionNode.LocalVariableListNode.Transpile(ref sb);
            functionNode.StatementListNode.Transpile(ref sb);
            sb.Append("end");
            functionNode.LastLineDelimiterNode.Transpile(ref sb);

            TranspileStringConcatenationHandler.ResetLocalVariables();
        }

        public static IEnumerable<LuaStatementSyntax> TranspileToLua(this FunctionSyntax functionNode)
        {
            _ = functionNode ?? throw new ArgumentNullException(nameof(functionNode));

            var parameters = functionNode.FunctionDeclarationNode.ParameterListReferenceNode.TranspileToLua();
            var functionIdentifier = functionNode.FunctionDeclarationNode.IdentifierNode.TranspileIdentifierToLua();

            if (functionNode.FunctionDeclarationNode.ReturnTypeNode.TypeNameNode?.TypeNameToken.TokenType == Jass.SyntaxTokenType.StringKeyword)
            {
                TranspileStringConcatenationHandler.RegisterFunctionWithStringReturnType(functionNode.FunctionDeclarationNode.IdentifierNode.ValueText);
            }

            var functionExpression = new LuaFunctionExpressionSyntax();
            functionExpression.AddParameters(parameters);
            functionExpression.Body.Statements.AddRange(functionNode.LocalVariableListNode.Select(localVariableNode
                => new LuaLocalDeclarationStatementSyntax(localVariableNode.VariableDeclarationNode.TranspileToLua(true))));
            functionExpression.Body.Statements.AddRange(functionNode.StatementListNode.TranspileToLua());
            functionExpression.RenderAsFunctionDefinition = true;

            TranspileStringConcatenationHandler.ResetLocalVariables();

            var functionDeclaration = new LuaVariableDeclaratorSyntax(functionIdentifier, functionExpression);
            functionDeclaration.IsLocalDeclaration = false;

            var declaration = new LuaVariableListDeclarationSyntax();
            declaration.Variables.Add(functionDeclaration);

            yield return new LuaLocalDeclarationStatementSyntax(declaration);

#if false
            foreach (var eolNode in functionNode.LastLineDelimiterNode)
            {
                if (eolNode.NewlineToken is not null)
                {
                    yield return LuaBlankLinesStatement.One;
                }
            }
#else
            if (functionNode.LastLineDelimiterNode.Count() > 1)
            {
                yield return LuaBlankLinesStatement.One;
            }
#endif
        }
    }
}
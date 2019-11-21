// ------------------------------------------------------------------------------
// <copyright file="LuaFunctionBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using CSharpLua.LuaAst;

using War3Net.Build.Providers;

namespace War3Net.Build.Script
{
    internal sealed class LuaFunctionBuilder : FunctionBuilder<LuaVariableListDeclarationSyntax, LuaStatementSyntax, LuaExpressionSyntax>
    {
        public LuaFunctionBuilder(FunctionBuilderData data)
            : base(data)
        {
        }

        public sealed override LuaVariableListDeclarationSyntax Build(
            string functionName,
            params LuaStatementSyntax[] statements)
        {
            var functionSyntax = new LuaFunctionExpressionSyntax();
            functionSyntax.AddStatements(statements);

            var mainFunctionDeclarator = new LuaVariableDeclaratorSyntax(functionName, functionSyntax);
            mainFunctionDeclarator.IsLocalDeclaration = false;

            var globalFunctionSyntax = new LuaVariableListDeclarationSyntax();
            globalFunctionSyntax.Variables.Add(mainFunctionDeclarator);

            return globalFunctionSyntax;
        }

        public sealed override LuaVariableListDeclarationSyntax BuildMainFunction()
        {
            return Build(
                MainFunctionProvider.FunctionName,
                MainFunctionStatementsProvider<LuaFunctionBuilder, LuaVariableListDeclarationSyntax, LuaStatementSyntax, LuaExpressionSyntax>.GetStatements(this).ToArray());
        }

        public sealed override LuaVariableListDeclarationSyntax BuildConfigFunction()
        {
            return Build(
                ConfigFunctionProvider.FunctionName,
                ConfigFunctionStatementsProvider<LuaFunctionBuilder, LuaVariableListDeclarationSyntax, LuaStatementSyntax, LuaExpressionSyntax>.GetStatements(this).ToArray());
        }

        public sealed override LuaStatementSyntax GenerateLocalDeclarationStatement(string variableName)
        {
            var variableList = new LuaVariableListDeclarationSyntax();
            variableList.Variables.Add(new LuaVariableDeclaratorSyntax(variableName));
            return variableList;
        }

        public sealed override LuaStatementSyntax GenerateInvocationStatement(string functionName, params LuaExpressionSyntax[] args)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(functionName, args));
        }

        public sealed override LuaExpressionSyntax GenerateIntegerLiteralExpression(int value)
        {
            return new LuaFloatLiteralExpressionSyntax(value);
        }

        public sealed override LuaExpressionSyntax GenerateBooleanLiteralExpression(bool value)
        {
            return value ? LuaIdentifierLiteralExpressionSyntax.True : LuaIdentifierLiteralExpressionSyntax.False;
        }

        public sealed override LuaExpressionSyntax GenerateStringLiteralExpression(string value)
        {
            return new LuaStringLiteralExpressionSyntax(value);
        }

        public sealed override LuaExpressionSyntax GenerateFloatLiteralExpression(float value)
        {
            return new LuaFloatLiteralExpressionSyntax(value);
        }

        public sealed override LuaExpressionSyntax GenerateNullLiteralExpression()
        {
            return new LuaIdentifierLiteralExpressionSyntax(LuaIdentifierNameSyntax.Nil);
        }

        public sealed override LuaExpressionSyntax GenerateVariableExpression(string variableName)
        {
            return variableName;
        }

        public sealed override LuaExpressionSyntax GenerateInvocationExpression(string functionName, params LuaExpressionSyntax[] args)
        {
            return new LuaInvocationExpressionSyntax(functionName, args);
        }

        public sealed override LuaExpressionSyntax GenerateFourCCExpression(string fourCC)
        {
            return new LuaInvocationExpressionSyntax(nameof(War3Api.Common.FourCC), new LuaStringLiteralExpressionSyntax(fourCC));
        }

        public sealed override LuaExpressionSyntax GenerateBinaryAdditionExpression(LuaExpressionSyntax left, LuaExpressionSyntax right)
        {
            return new LuaBinaryExpressionSyntax(left, LuaSyntaxNode.Tokens.Plus, right);
        }

        public sealed override LuaExpressionSyntax GenerateBinarySubtractionExpression(LuaExpressionSyntax left, LuaExpressionSyntax right)
        {
            return new LuaBinaryExpressionSyntax(left, LuaSyntaxNode.Tokens.Sub, right);
        }
    }
}
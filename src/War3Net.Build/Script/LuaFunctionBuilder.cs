// ------------------------------------------------------------------------------
// <copyright file="LuaFunctionBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using CSharpLua.LuaAst;

namespace War3Net.Build.Script
{
    internal abstract class LuaFunctionBuilder : FunctionBuilder<LuaVariableListDeclarationSyntax, LuaStatementSyntax, LuaExpressionSyntax>
    {
        public LuaFunctionBuilder(FunctionBuilderData data)
            : base(data)
        {
        }

        public abstract LuaVariableListDeclarationSyntax Build();

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

        public sealed override LuaStatementSyntax GenerateInvocationStatementWithoutArguments(
            string functionName)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(functionName));
        }

        public sealed override LuaStatementSyntax GenerateInvocationStatementWithIntegerArgument(
            string functionName,
            int argument)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(functionName, new LuaFloatLiteralExpressionSyntax(argument)));
        }

        public sealed override LuaStatementSyntax GenerateInvocationStatementWithBooleanArgument(
            string functionName,
            bool argument)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(functionName, argument ? LuaIdentifierLiteralExpressionSyntax.True : LuaIdentifierLiteralExpressionSyntax.False));
        }

        public sealed override LuaStatementSyntax GenerateInvocationStatementWithStringArgument(
            string functionName,
            string argument)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(functionName, new LuaStringLiteralExpressionSyntax(argument)));
        }

        public sealed override LuaStatementSyntax GenerateInvocationStatementWithFloatArgument(
            string functionName,
            float argument)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(functionName, new LuaFloatLiteralExpressionSyntax(argument)));
        }

        public sealed override LuaStatementSyntax GenerateInvocationStatementWithVariableArgument(
            string functionName,
            string variableName)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(functionName, variableName));
        }

        public sealed override LuaStatementSyntax GenerateInvocationStatementWithVariableAndIntegerArgument(string functionName, string variableName, int argument)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                functionName,
                variableName,
                new LuaFloatLiteralExpressionSyntax(argument)));
        }
    }
}
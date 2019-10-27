// ------------------------------------------------------------------------------
// <copyright file="LuaFunctionBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using CSharpLua.LuaAst;

using War3Net.Build.Info;

namespace War3Net.Build.Script
{
    internal abstract class LuaFunctionBuilder : FunctionBuilder<LuaStatementSyntax, LuaVariableListDeclarationSyntax>
    {
        public LuaFunctionBuilder(MapInfo mapInfo)
            : base(mapInfo)
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
    }
}
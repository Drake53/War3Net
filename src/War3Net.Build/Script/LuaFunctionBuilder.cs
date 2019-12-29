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

        public override LuaStatementSyntax GenerateAssignmentStatement(string variableName, LuaExpressionSyntax value)
        {
            return new LuaExpressionStatementSyntax(new LuaAssignmentExpressionSyntax(variableName, value));
        }

        public sealed override LuaStatementSyntax GenerateInvocationStatement(string functionName, params LuaExpressionSyntax[] args)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(functionName, args));
        }

        public override LuaStatementSyntax GenerateIfStatement(LuaExpressionSyntax condition, params LuaStatementSyntax[] ifBody)
        {
            var ifStatement = new LuaIfStatementSyntax(condition);
            ifStatement.Body.Statements.AddRange(ifBody);
            return ifStatement;
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

        public override LuaExpressionSyntax GenerateBinaryExpression(BinaryOperator @operator, LuaExpressionSyntax left, LuaExpressionSyntax right)
        {
            var operatorToken = @operator switch
            {
                BinaryOperator.Addition => LuaSyntaxNode.Tokens.Plus,
                BinaryOperator.Subtraction => LuaSyntaxNode.Tokens.Sub,
                BinaryOperator.Multiplication => LuaSyntaxNode.Tokens.Multiply,
                BinaryOperator.Division => LuaSyntaxNode.Tokens.Div,
                BinaryOperator.Equals => LuaSyntaxNode.Tokens.EqualsEquals,
                BinaryOperator.NotEquals => LuaSyntaxNode.Tokens.NotEquals,

                _ => throw new System.ArgumentException($"Binary operator {@operator} is not supported, or not defined", nameof(@operator)),
            };

            return new LuaBinaryExpressionSyntax(left, operatorToken, right);
        }
    }
}
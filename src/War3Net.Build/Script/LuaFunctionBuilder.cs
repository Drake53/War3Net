// ------------------------------------------------------------------------------
// <copyright file="LuaFunctionBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using CSharpLua.LuaAst;

namespace War3Net.Build.Script
{
    internal sealed class LuaFunctionBuilder : FunctionBuilder<LuaVariableListDeclarationSyntax, LuaStatementSyntax, LuaExpressionSyntax>
    {
        public LuaFunctionBuilder(FunctionBuilderData data)
            : base(data)
        {
        }

        public override LuaVariableListDeclarationSyntax Build(string functionName, IEnumerable<(string type, string name)> locals, IEnumerable<LuaStatementSyntax> statements)
        {
            var variableList = new List<LuaVariableListDeclarationSyntax>();
            if (locals != null)
            {
                // variableList.Variables.AddRange(locals.Select(localDeclaration => new LuaVariableDeclaratorSyntax(localDeclaration.name)));
                variableList = locals.Select(localDeclaration =>
                {
                    var variableDeclaration = new LuaVariableListDeclarationSyntax();
                    variableDeclaration.Variables.Add(new LuaVariableDeclaratorSyntax(localDeclaration.name));
                    return variableDeclaration;
                }).ToList();
            }

            var functionSyntax = new LuaFunctionExpressionSyntax();
            functionSyntax.AddStatements(variableList.Concat(statements));

            var mainFunctionDeclarator = new LuaVariableDeclaratorSyntax(functionName, functionSyntax);
            mainFunctionDeclarator.IsLocalDeclaration = false;

            var globalFunctionSyntax = new LuaVariableListDeclarationSyntax();
            globalFunctionSyntax.Variables.Add(mainFunctionDeclarator);

            return globalFunctionSyntax;
        }

        public override LuaVariableListDeclarationSyntax Build(string functionName, IEnumerable<(string type, string name, LuaExpressionSyntax value)> locals, IEnumerable<LuaStatementSyntax> statements)
        {
            throw new System.NotImplementedException();
        }

        public sealed override IEnumerable<LuaVariableListDeclarationSyntax> BuildMainFunction()
        {
            return Main.MainFunctionGenerator<LuaFunctionBuilder, LuaVariableListDeclarationSyntax, LuaStatementSyntax, LuaExpressionSyntax>.GetFunctions(this);
        }

        public sealed override IEnumerable<LuaVariableListDeclarationSyntax> BuildConfigFunction()
        {
            return Config.ConfigFunctionGenerator<LuaFunctionBuilder, LuaVariableListDeclarationSyntax, LuaStatementSyntax, LuaExpressionSyntax>.GetFunctions(this);
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

        public override LuaStatementSyntax GenerateElseClause(LuaStatementSyntax ifStatement, LuaExpressionSyntax condition, params LuaStatementSyntax[] elseBody)
        {
            throw new System.NotImplementedException();
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

        public override LuaExpressionSyntax GenerateFloatLiteralExpression(float value, int decimalPlaces)
        {
            throw new System.NotImplementedException();
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

        public override LuaExpressionSyntax GenerateFunctionExpression(string functionName)
        {
            throw new System.NotImplementedException();
        }

        public override LuaExpressionSyntax GenerateUnaryExpression(UnaryOperator @operator, LuaExpressionSyntax expression)
        {
            throw new System.NotImplementedException();
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
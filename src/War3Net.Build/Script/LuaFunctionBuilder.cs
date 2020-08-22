// ------------------------------------------------------------------------------
// <copyright file="LuaFunctionBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

using CSharpLua.LuaAst;

namespace War3Net.Build.Script
{
    internal sealed class LuaFunctionBuilder : FunctionBuilder<LuaVariableListDeclarationSyntax, LuaVariableListDeclarationSyntax, LuaStatementSyntax, LuaExpressionSyntax>
    {
        public LuaFunctionBuilder(FunctionBuilderData data)
            : base(data)
        {
        }

        public override string GetTypeName(BuiltinType type)
        {
            return null;
        }

        public override LuaVariableListDeclarationSyntax? GenerateGlobalDeclaration(string typeName, string name, bool isArray)
        {
            return GenerateGlobalDeclaration(
                typeName,
                name,
                isArray ? GenerateInvocationExpression("__jarray", GenerateIntegerLiteralExpression(0)) : null);
        }

        public override LuaVariableListDeclarationSyntax? GenerateGlobalDeclaration(string typeName, string name, LuaExpressionSyntax? value)
        {
            if (value is null)
            {
                return null;
            }

            var declarator = new LuaVariableDeclaratorSyntax(name, value);
            declarator.IsLocalDeclaration = false;

            var result = new LuaVariableListDeclarationSyntax();
            result.Variables.Add(declarator);
            return result;
        }

        public override LuaVariableListDeclarationSyntax Build(string functionName, IEnumerable<(string typeName, string name)> locals, IEnumerable<LuaStatementSyntax> statements)
        {
            var variableList = new List<LuaLocalVariableDeclaratorSyntax>();
            if (locals != null)
            {
                variableList = locals.Select(localDeclaration
                    => new LuaLocalVariableDeclaratorSyntax(new LuaVariableDeclaratorSyntax(localDeclaration.name))).ToList();
            }

            var functionSyntax = new LuaFunctionExpressionSyntax();
            functionSyntax.AddStatements(variableList.Concat(statements));

            var mainFunctionDeclarator = new LuaVariableDeclaratorSyntax(functionName, functionSyntax);
            mainFunctionDeclarator.IsLocalDeclaration = false;

            var globalFunctionSyntax = new LuaVariableListDeclarationSyntax();
            globalFunctionSyntax.Variables.Add(mainFunctionDeclarator);

            return globalFunctionSyntax;
        }

        public override LuaVariableListDeclarationSyntax Build(string functionName, IEnumerable<(string typeName, string name, LuaExpressionSyntax value)> locals, IEnumerable<LuaStatementSyntax> statements)
        {
            var variableList = new List<LuaLocalVariableDeclaratorSyntax>();
            if (locals != null)
            {
                variableList = locals.Select(localDeclaration
                    => new LuaLocalVariableDeclaratorSyntax(new LuaVariableDeclaratorSyntax(localDeclaration.name, localDeclaration.value))).ToList();
            }

            var functionSyntax = new LuaFunctionExpressionSyntax();
            functionSyntax.AddStatements(variableList.Concat(statements));

            var mainFunctionDeclarator = new LuaVariableDeclaratorSyntax(functionName, functionSyntax);
            mainFunctionDeclarator.IsLocalDeclaration = false;

            var globalFunctionSyntax = new LuaVariableListDeclarationSyntax();
            globalFunctionSyntax.Variables.Add(mainFunctionDeclarator);

            return globalFunctionSyntax;
        }

        public override IEnumerable<LuaVariableListDeclarationSyntax?> BuildGlobalDeclarations()
        {
            return GlobalDeclarationsGenerator<LuaFunctionBuilder, LuaVariableListDeclarationSyntax, LuaVariableListDeclarationSyntax, LuaStatementSyntax, LuaExpressionSyntax>.GetGlobals(this);
        }

        public sealed override IEnumerable<LuaVariableListDeclarationSyntax> BuildMainFunction()
        {
            return Main.MainFunctionGenerator<LuaFunctionBuilder, LuaVariableListDeclarationSyntax, LuaVariableListDeclarationSyntax, LuaStatementSyntax, LuaExpressionSyntax>.GetFunctions(this);
        }

        public sealed override IEnumerable<LuaVariableListDeclarationSyntax> BuildConfigFunction()
        {
            return Config.ConfigFunctionGenerator<LuaFunctionBuilder, LuaVariableListDeclarationSyntax, LuaVariableListDeclarationSyntax, LuaStatementSyntax, LuaExpressionSyntax>.GetFunctions(this);
        }

        public override LuaStatementSyntax GenerateAssignmentStatement(string variableName, LuaExpressionSyntax value)
        {
            return new LuaExpressionStatementSyntax(new LuaAssignmentExpressionSyntax(variableName, value));
        }

        public override LuaStatementSyntax GenerateAssignmentStatement(string variableName, LuaExpressionSyntax arrayIndex, LuaExpressionSyntax value)
        {
            return new LuaExpressionStatementSyntax(new LuaAssignmentExpressionSyntax(new LuaTableIndexAccessExpressionSyntax(variableName, arrayIndex), value));
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
            if (!(ifStatement is LuaIfStatementSyntax ifNode))
            {
                throw new ArgumentException($"{nameof(ifStatement)} must be of type {nameof(LuaIfStatementSyntax)}.", nameof(ifStatement));
            }

            if (ifNode.Else != null)
            {
                throw new ArgumentException("Cannot extend the if statement, because it already contains an else node.", nameof(ifStatement));
            }

            if (condition is null)
            {
                ifNode.Else = new LuaElseClauseSyntax();
                ifNode.Else.Body.Statements.AddRange(elseBody);
            }
            else
            {
                var elseif = new LuaElseIfStatementSyntax(condition);
                elseif.Body.Statements.AddRange(elseBody);
                ifNode.ElseIfStatements.Add(elseif);
            }

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

        public override LuaExpressionSyntax GenerateFloatLiteralExpression(float value, int decimalPlaces)
        {
            // ...
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

        public override LuaExpressionSyntax GenerateFunctionReferenceExpression(string functionName)
        {
            return GenerateVariableExpression(functionName);
        }

        public override LuaExpressionSyntax GenerateArrayReferenceExpression(string variableName, LuaExpressionSyntax index)
        {
            return new LuaTableIndexAccessExpressionSyntax(variableName, index);
        }

        public override LuaExpressionSyntax GenerateUnaryExpression(UnaryOperator @operator, LuaExpressionSyntax expression)
        {
            var operatorToken = @operator switch
            {
                UnaryOperator.Not => LuaSyntaxNode.Keyword.Not,

                _ => throw new ArgumentException($"Unary operator {@operator} is not supported, or not defined", nameof(@operator)),
            };

            return new LuaPrefixUnaryExpressionSyntax(expression, operatorToken);
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
                BinaryOperator.And => LuaSyntaxNode.Keyword.And,
                BinaryOperator.Or => LuaSyntaxNode.Keyword.Or,

                _ => throw new ArgumentException($"Binary operator {@operator} is not supported, or not defined", nameof(@operator)),
            };

            return new LuaBinaryExpressionSyntax(left, operatorToken, right);
        }
    }
}
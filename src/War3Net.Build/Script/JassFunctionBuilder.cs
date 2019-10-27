// ------------------------------------------------------------------------------
// <copyright file="JassFunctionBuilder.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Info;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.Build.Script
{
    internal abstract class JassFunctionBuilder : FunctionBuilder<NewStatementSyntax, FunctionSyntax>
    {
        public JassFunctionBuilder(MapInfo mapInfo)
            : base(mapInfo)
        {
        }

        public abstract FunctionSyntax Build();

        public sealed override FunctionSyntax Build(
            string functionName,
            params NewStatementSyntax[] statements)
        {
            return JassSyntaxFactory.Function(
                JassSyntaxFactory.FunctionDeclaration(functionName),
                statements);
        }

        public sealed override NewStatementSyntax GenerateInvocationStatementWithoutArguments(
            string functionName)
        {
            return JassSyntaxFactory.CallStatement(functionName);
        }

        public sealed override NewStatementSyntax GenerateInvocationStatementWithIntegerArgument(
            string functionName,
            int argument)
        {
            return JassSyntaxFactory.CallStatement(functionName, JassSyntaxFactory.ConstantExpression(argument));
        }

        public sealed override NewStatementSyntax GenerateInvocationStatementWithBooleanArgument(
            string functionName,
            bool argument)
        {
            return JassSyntaxFactory.CallStatement(functionName, JassSyntaxFactory.ConstantExpression(argument));
        }

        public sealed override NewStatementSyntax GenerateInvocationStatementWithStringArgument(
            string functionName,
            string argument)
        {
            return JassSyntaxFactory.CallStatement(functionName, JassSyntaxFactory.ConstantExpression(argument));
        }

        public sealed override NewStatementSyntax GenerateInvocationStatementWithFloatArgument(
            string functionName,
            float argument)
        {
            return JassSyntaxFactory.CallStatement(functionName, JassSyntaxFactory.ConstantExpression(argument));
        }

        public sealed override NewStatementSyntax GenerateInvocationStatementWithVariableArgument(
            string functionName,
            string variableName)
        {
            return JassSyntaxFactory.CallStatement(functionName, JassSyntaxFactory.VariableExpression(variableName));
        }
    }
}
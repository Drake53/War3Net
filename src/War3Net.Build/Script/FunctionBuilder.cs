// ------------------------------------------------------------------------------
// <copyright file="FunctionBuilder.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using War3Net.Build.Info;

namespace War3Net.Build.Script
{
    internal abstract class FunctionBuilder<TStatementSyntax, TFunctionSyntax>
    {
        private readonly MapInfo _mapInfo;

        public FunctionBuilder(MapInfo mapInfo)
        {
            _mapInfo = mapInfo;
        }

        public MapInfo MapInfo => _mapInfo;

        public abstract TFunctionSyntax Build(
            string functionName,
            params TStatementSyntax[] statements);

        public abstract TStatementSyntax GenerateInvocationStatementWithoutArguments(
            string functionName);

        public abstract TStatementSyntax GenerateInvocationStatementWithIntegerArgument(
            string functionName,
            int argument);

        public abstract TStatementSyntax GenerateInvocationStatementWithBooleanArgument(
            string functionName,
            bool argument);

        public abstract TStatementSyntax GenerateInvocationStatementWithStringArgument(
            string functionName,
            string argument);

        public abstract TStatementSyntax GenerateInvocationStatementWithFloatArgument(
            string functionName,
            float argument);

        public abstract TStatementSyntax GenerateInvocationStatementWithVariableArgument(
            string functionName,
            string variableName);
    }
}
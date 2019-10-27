// ------------------------------------------------------------------------------
// <copyright file="LuaMainFunctionBuilder.cs" company="Drake53">
// Copyright (c) 2019 Drake53. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using CSharpLua.LuaAst;

using War3Net.Build.Info;

using static War3Net.Build.Providers.MainFunctionStatementsProvider<
    War3Net.Build.Script.LuaMainFunctionBuilder,
    CSharpLua.LuaAst.LuaStatementSyntax,
    CSharpLua.LuaAst.LuaVariableListDeclarationSyntax>;

namespace War3Net.Build.Script
{
    internal sealed class LuaMainFunctionBuilder : LuaFunctionBuilder, IMainFunctionBuilder<LuaStatementSyntax>
    {
        public LuaMainFunctionBuilder(MapInfo mapInfo)
            : base(mapInfo)
        {
        }

        public bool EnableCSharp { get; set; }

        public override LuaVariableListDeclarationSyntax Build()
        {
            return Build(GetMainFunctionName, GetStatements(this).ToArray());
        }

        /// <inheritdoc/>
        public LuaStatementSyntax GenerateSetCameraBoundsStatement(
            string functionName,
            string marginFunctionName,
            string marginLeft,
            string marginRight,
            string marginTop,
            string marginBottom,
            float x1,
            float y1,
            float x2,
            float y2,
            float x3,
            float y3,
            float x4,
            float y4)
        {
            return new LuaExpressionStatementSyntax(
                new LuaInvocationExpressionSyntax(
                    functionName,
                    new LuaBinaryExpressionSyntax(
                        new LuaFloatLiteralExpressionSyntax(x1),
                        LuaSyntaxNode.Tokens.Plus,
                        new LuaInvocationExpressionSyntax(marginFunctionName, marginLeft)),
                    new LuaBinaryExpressionSyntax(
                        new LuaFloatLiteralExpressionSyntax(y1),
                        LuaSyntaxNode.Tokens.Plus,
                        new LuaInvocationExpressionSyntax(marginFunctionName, marginBottom)),
                    new LuaBinaryExpressionSyntax(
                        new LuaFloatLiteralExpressionSyntax(x2),
                        LuaSyntaxNode.Tokens.Sub,
                        new LuaInvocationExpressionSyntax(marginFunctionName, marginRight)),
                    new LuaBinaryExpressionSyntax(
                        new LuaFloatLiteralExpressionSyntax(y2),
                        LuaSyntaxNode.Tokens.Sub,
                        new LuaInvocationExpressionSyntax(marginFunctionName, marginTop)),
                    new LuaBinaryExpressionSyntax(
                        new LuaFloatLiteralExpressionSyntax(x3),
                        LuaSyntaxNode.Tokens.Plus,
                        new LuaInvocationExpressionSyntax(marginFunctionName, marginLeft)),
                    new LuaBinaryExpressionSyntax(
                        new LuaFloatLiteralExpressionSyntax(y3),
                        LuaSyntaxNode.Tokens.Sub,
                        new LuaInvocationExpressionSyntax(marginFunctionName, marginTop)),
                    new LuaBinaryExpressionSyntax(
                        new LuaFloatLiteralExpressionSyntax(x4),
                        LuaSyntaxNode.Tokens.Sub,
                        new LuaInvocationExpressionSyntax(marginFunctionName, marginRight)),
                    new LuaBinaryExpressionSyntax(
                        new LuaFloatLiteralExpressionSyntax(y4),
                        LuaSyntaxNode.Tokens.Plus,
                        new LuaInvocationExpressionSyntax(marginFunctionName, marginBottom))));
        }

        /// <inheritdoc/>
        public LuaStatementSyntax GenerateSetDayNightModelsStatement(
            string functionName,
            string terrainDNCFile,
            string unitDNCFile)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                functionName,
                new LuaStringLiteralExpressionSyntax(terrainDNCFile),
                new LuaStringLiteralExpressionSyntax(unitDNCFile)));
        }

        /// <inheritdoc/>
        public LuaStatementSyntax GenerateSetTerrainFogExStatement(
            string functionName,
            int fogStyle,
            float startZ,
            float endZ,
            float density,
            float red,
            float green,
            float blue)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                functionName,
                fogStyle,
                startZ,
                endZ,
                density,
                red,
                green,
                blue));
        }

        /// <inheritdoc/>
        public LuaStatementSyntax GenerateAddWeatherEffectStatement(
            string functionName,
            string enableFunctionName,
            string rectFunctionName,
            float left,
            float bottom,
            float right,
            float top,
            int weatherType)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
                enableFunctionName,
                new LuaInvocationExpressionSyntax(
                    functionName,
                    new LuaInvocationExpressionSyntax(
                        rectFunctionName,
                        new LuaFloatLiteralExpressionSyntax(left),
                        new LuaFloatLiteralExpressionSyntax(bottom),
                        new LuaFloatLiteralExpressionSyntax(right),
                        new LuaFloatLiteralExpressionSyntax(top)),
                    new LuaFloatLiteralExpressionSyntax(weatherType)),
                LuaIdentifierLiteralExpressionSyntax.True));
        }

        /// <inheritdoc/>
        public LuaStatementSyntax GenerateSetMapMusicStatement(
            string functionName,
            string musicName,
            bool random,
            int index)
        {
            return new LuaExpressionStatementSyntax(new LuaInvocationExpressionSyntax(
               functionName,
               new LuaStringLiteralExpressionSyntax(musicName),
               random ? LuaIdentifierLiteralExpressionSyntax.True : LuaIdentifierLiteralExpressionSyntax.False,
               new LuaFloatLiteralExpressionSyntax(index)));
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="JassMainFunctionBuilder.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Linq;

using War3Net.Build.Providers;
using War3Net.CodeAnalysis.Jass.Syntax;

using static War3Net.Build.Providers.MainFunctionStatementsProvider<
    War3Net.Build.Script.JassMainFunctionBuilder,
    War3Net.CodeAnalysis.Jass.Syntax.NewStatementSyntax,
    War3Net.CodeAnalysis.Jass.Syntax.FunctionSyntax>;

namespace War3Net.Build.Script
{
    internal sealed class JassMainFunctionBuilder : JassFunctionBuilder, IMainFunctionBuilder<NewStatementSyntax>
    {
        public JassMainFunctionBuilder(FunctionBuilderData data)
            : base(data)
        {
        }

        public bool EnableCSharp
        {
            get => false;
            set { }
        }

        public override FunctionSyntax Build()
        {
            return Build(GetMainFunctionName, GetStatements(this).ToArray());
        }

        protected override LocalVariableListSyntax GetLocalDeclarations()
        {
            return JassSyntaxFactory.LocalVariableList(GenerateLocalDeclaration(nameof(War3Api.Common.unit), MainFunctionProvider.LocalUnitVariableName));
        }

        /// <inheritdoc/>
        public NewStatementSyntax GenerateSetCameraBoundsStatement(
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
            return JassSyntaxFactory.CallStatement(
                functionName,
                JassSyntaxFactory.ArgumentList(
                    JassSyntaxFactory.BinaryAdditionExpression(
                        JassSyntaxFactory.ConstantExpression(x1),
                        JassSyntaxFactory.InvocationExpression(
                            marginFunctionName,
                            JassSyntaxFactory.ArgumentList(
                                JassSyntaxFactory.VariableExpression(marginLeft)))),
                    JassSyntaxFactory.BinaryAdditionExpression(
                        JassSyntaxFactory.ConstantExpression(y1),
                        JassSyntaxFactory.InvocationExpression(
                            marginFunctionName,
                            JassSyntaxFactory.ArgumentList(
                                JassSyntaxFactory.VariableExpression(marginBottom)))),
                    JassSyntaxFactory.BinarySubtractionExpression(
                        JassSyntaxFactory.ConstantExpression(x2),
                        JassSyntaxFactory.InvocationExpression(
                            marginFunctionName,
                            JassSyntaxFactory.ArgumentList(
                                JassSyntaxFactory.VariableExpression(marginRight)))),
                    JassSyntaxFactory.BinarySubtractionExpression(
                        JassSyntaxFactory.ConstantExpression(y2),
                        JassSyntaxFactory.InvocationExpression(
                            marginFunctionName,
                            JassSyntaxFactory.ArgumentList(
                                JassSyntaxFactory.VariableExpression(marginTop)))),
                    JassSyntaxFactory.BinaryAdditionExpression(
                        JassSyntaxFactory.ConstantExpression(x3),
                        JassSyntaxFactory.InvocationExpression(
                            marginFunctionName,
                            JassSyntaxFactory.ArgumentList(
                                JassSyntaxFactory.VariableExpression(marginLeft)))),
                    JassSyntaxFactory.BinarySubtractionExpression(
                        JassSyntaxFactory.ConstantExpression(y3),
                        JassSyntaxFactory.InvocationExpression(
                            marginFunctionName,
                            JassSyntaxFactory.ArgumentList(
                                JassSyntaxFactory.VariableExpression(marginTop)))),
                    JassSyntaxFactory.BinarySubtractionExpression(
                        JassSyntaxFactory.ConstantExpression(x4),
                        JassSyntaxFactory.InvocationExpression(
                            marginFunctionName,
                            JassSyntaxFactory.ArgumentList(
                                JassSyntaxFactory.VariableExpression(marginRight)))),
                    JassSyntaxFactory.BinaryAdditionExpression(
                        JassSyntaxFactory.ConstantExpression(y4),
                        JassSyntaxFactory.InvocationExpression(
                            marginFunctionName,
                            JassSyntaxFactory.ArgumentList(
                                JassSyntaxFactory.VariableExpression(marginBottom))))));
        }

        /// <inheritdoc/>
        public NewStatementSyntax GenerateSetDayNightModelsStatement(
            string functionName,
            string terrainDNCFile,
            string unitDNCFile)
        {
            return JassSyntaxFactory.CallStatement(
                functionName,
                JassSyntaxFactory.ArgumentList(
                    JassSyntaxFactory.ConstantExpression(terrainDNCFile),
                    JassSyntaxFactory.ConstantExpression(unitDNCFile)));
        }

        /// <inheritdoc/>
        public NewStatementSyntax GenerateSetTerrainFogExStatement(
            string functionName,
            int fogStyle,
            float startZ,
            float endZ,
            float density,
            float red,
            float green,
            float blue)
        {
            return JassSyntaxFactory.CallStatement(
                functionName,
                JassSyntaxFactory.ConstantExpression(fogStyle),
                JassSyntaxFactory.ConstantExpression(startZ),
                JassSyntaxFactory.ConstantExpression(endZ),
                JassSyntaxFactory.ConstantExpression(density),
                JassSyntaxFactory.ConstantExpression(red),
                JassSyntaxFactory.ConstantExpression(green),
                JassSyntaxFactory.ConstantExpression(blue));
        }

        /// <inheritdoc/>
        public NewStatementSyntax GenerateAddWeatherEffectStatement(
            string functionName,
            string enableFunctionName,
            string rectFunctionName,
            float left,
            float bottom,
            float right,
            float top,
            int weatherType)
        {
            return JassSyntaxFactory.CallStatement(
                enableFunctionName,
                JassSyntaxFactory.InvocationExpression(
                    functionName,
                    JassSyntaxFactory.InvocationExpression(
                        rectFunctionName,
                        JassSyntaxFactory.ConstantExpression(left),
                        JassSyntaxFactory.ConstantExpression(bottom),
                        JassSyntaxFactory.ConstantExpression(right),
                        JassSyntaxFactory.ConstantExpression(top)),
                    JassSyntaxFactory.ConstantExpression(weatherType)),
                JassSyntaxFactory.ConstantExpression(true));
        }

        /// <inheritdoc/>
        public NewStatementSyntax GenerateSetMapMusicStatement(
            string functionName,
            string musicName,
            bool random,
            int index)
        {
            return JassSyntaxFactory.CallStatement(
                functionName,
                JassSyntaxFactory.ArgumentList(
                    JassSyntaxFactory.ConstantExpression(musicName),
                    JassSyntaxFactory.ConstantExpression(random),
                    JassSyntaxFactory.ConstantExpression(index)));
        }

        public NewStatementSyntax GenerateCreateUnitStatement(
            string functionName,
            string playerFunctionName,
            int owner,
            string unitId,
            float x,
            float y,
            float facing)
        {
            return JassSyntaxFactory.SetStatement(
                MainFunctionProvider.LocalUnitVariableName,
                JassSyntaxFactory.EqualsValueClause(
                    JassSyntaxFactory.InvocationExpression(
                        functionName,
                        JassSyntaxFactory.ArgumentList(
                            JassSyntaxFactory.InvocationExpression(
                                playerFunctionName,
                                JassSyntaxFactory.ConstantExpression(owner)),
                            JassSyntaxFactory.FourCCExpression(unitId),
                            JassSyntaxFactory.ConstantExpression(x),
                            JassSyntaxFactory.ConstantExpression(y),
                            JassSyntaxFactory.ConstantExpression(facing)))));
        }
    }
}
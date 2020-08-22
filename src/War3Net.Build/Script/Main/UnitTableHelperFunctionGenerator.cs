// ------------------------------------------------------------------------------
// <copyright file="UnitTableHelperFunctionGenerator.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Providers;

namespace War3Net.Build.Script.Main
{
    internal static partial class MainFunctionGenerator<TBuilder, TGlobalDeclarationSyntax, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
    {
        private const string LocalCurrentSetVariableName = "curset";

        private static TFunctionSyntax GenerateUnitTableHelperFunction(TBuilder builder)
        {
            var locals = new List<(string, string)>()
            {
                (builder.GetTypeName(BuiltinType.Int32), LocalCurrentSetVariableName),
            };

            return builder.Build("InitRandomGroups", locals, GetUnitTableHelperFunctionStatements(builder));
        }

        private static IEnumerable<TStatementSyntax> GetUnitTableHelperFunctionStatements(TBuilder builder)
        {
            var mapInfo = builder.Data.MapInfo;
            for (var i = 0; i < mapInfo.RandomUnitTableCount; i++)
            {
                var unitTable = mapInfo.GetUnitTable(i);

                yield return builder.GenerateInvocationStatement(nameof(War3Api.Blizzard.RandomDistReset));

                var summedChance = 0; // TODO?
                for (var j = 0; j < unitTable.UnitSetCount; j++)
                {
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Blizzard.RandomDistAddItem),
                        builder.GenerateIntegerLiteralExpression(j),
                        builder.GenerateIntegerLiteralExpression(unitTable.GetSet(j).Chance));
                }

                yield return builder.GenerateAssignmentStatement(
                    LocalCurrentSetVariableName,
                    builder.GenerateInvocationExpression(nameof(War3Api.Blizzard.RandomDistChoose)));

                var groupVarName = $"gg_rg_{unitTable.Index:D3}";
                var bodyStatements = new TStatementSyntax[unitTable.UnitSetCount + 1][];
                for (var setIndex = 0; setIndex <= unitTable.UnitSetCount; setIndex++)
                {
                    var set = unitTable.GetSet(setIndex);
                    bodyStatements[setIndex] = new TStatementSyntax[unitTable.Positions];
                    for (var position = 0; position < unitTable.Positions; position++)
                    {
                        var id = set?.GetId(position) ?? new[] { '\0', '\0', '\0', '\0' };
                        var unitTypeExpression = RandomUnitProvider.IsRandomUnit(id, out var level)
                            ? builder.GenerateInvocationExpression(
                                nameof(War3Api.Common.ChooseRandomCreep),
                                builder.GenerateIntegerLiteralExpression(level))
                            : new string(id) == "\0\0\0\0"
                                ? builder.GenerateIntegerLiteralExpression(-1)
                                : builder.GenerateFourCCExpression(new string(id));

                        bodyStatements[setIndex][position] = builder.GenerateAssignmentStatement(
                            groupVarName,
                            builder.GenerateIntegerLiteralExpression(position),
                            unitTypeExpression);
                    }
                }

                var ifStatement = builder.GenerateIfStatement(
                    builder.GenerateBinaryExpression(
                        BinaryOperator.Equals,
                        builder.GenerateVariableExpression(LocalCurrentSetVariableName),
                        builder.GenerateIntegerLiteralExpression(0)),
                    bodyStatements[0]);

                for (var setIndex = 1; setIndex < unitTable.UnitSetCount; setIndex++)
                {
                    ifStatement = builder.GenerateElseClause(
                        ifStatement,
                        builder.GenerateBinaryExpression(
                            BinaryOperator.Equals,
                            builder.GenerateVariableExpression(LocalCurrentSetVariableName),
                            builder.GenerateIntegerLiteralExpression(setIndex)),
                        bodyStatements[setIndex]);
                }

                ifStatement = builder.GenerateElseClause(
                    ifStatement,
                    null,
                    bodyStatements[unitTable.UnitSetCount]);

                yield return ifStatement;
            }
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="CreateAllItemsHelperFunctionGenerator.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Providers;

namespace War3Net.Build.Script.Main
{
    internal static partial class MainFunctionGenerator<TBuilder, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
    {
        private static TFunctionSyntax GenerateCreateAllItemsHelperFunction(TBuilder builder)
        {
            var locals = new List<(string, string)>()
            {
                ("integer", LocalItemIdVariableName),
            };

            return builder.Build("CreateAllItems", locals, GetCreateAllItemsHelperFunctionStatements(builder));
        }

        private static IEnumerable<TStatementSyntax> GetCreateAllItemsHelperFunctionStatements(TBuilder builder)
        {
            foreach (var item in builder.Data.MapUnits.Where(mapUnit => mapUnit.IsItem))
            {
                if (item.TypeId == "sloc")
                {
                    continue;
                }

                if (item.IsRandomItem)
                {
                    var randomData = item.RandomData;
                    switch (randomData.Mode)
                    {
                        case 0:
                            yield return builder.GenerateAssignmentStatement(
                                LocalItemIdVariableName,
                                builder.GenerateInvocationExpression(
                                    nameof(War3Api.Common.ChooseRandomItemEx),
                                    builder.GenerateInvocationExpression(
                                        nameof(War3Api.Common.ConvertItemType),
                                        builder.GenerateIntegerLiteralExpression(randomData.ItemClass)),
                                    builder.GenerateIntegerLiteralExpression(randomData.ItemLevel)));
                            break;

                        case 2:
                            yield return builder.GenerateInvocationStatement(nameof(War3Api.Blizzard.RandomDistReset));
                            var summedChance = 0;
                            foreach (var randomItemOption in randomData)
                            {
                                summedChance += randomItemOption.chance;

                                var itemTypeExpression = RandomItemProvider.IsRandomItem(randomItemOption.id, out var level, out var @class)
                                        ? builder.GenerateInvocationExpression(
                                            nameof(War3Api.Common.ChooseRandomItemEx),
                                            builder.GenerateInvocationExpression(
                                                nameof(War3Api.Common.ConvertItemType),
                                                builder.GenerateIntegerLiteralExpression(@class)),
                                            builder.GenerateIntegerLiteralExpression(level))
                                        : builder.GenerateFourCCExpression(new string(randomItemOption.id));

                                yield return builder.GenerateInvocationStatement(
                                    nameof(War3Api.Blizzard.RandomDistAddItem),
                                    itemTypeExpression,
                                    builder.GenerateIntegerLiteralExpression(randomItemOption.chance));
                            }

                            if (summedChance < 100)
                            {
                                yield return builder.GenerateInvocationStatement(
                                    nameof(War3Api.Blizzard.RandomDistAddItem),
                                    builder.GenerateIntegerLiteralExpression(-1),
                                    builder.GenerateIntegerLiteralExpression(100 - summedChance));
                            }

                            yield return builder.GenerateAssignmentStatement(
                                LocalItemIdVariableName,
                                builder.GenerateInvocationExpression(nameof(War3Api.Blizzard.RandomDistChoose)));
                            break;

                        default:
                            break;
                    }

                    yield return builder.GenerateIfStatement(
                        builder.GenerateBinaryExpression(
                            BinaryOperator.NotEquals,
                            builder.GenerateVariableExpression(LocalItemIdVariableName),
                            builder.GenerateIntegerLiteralExpression(-1)),
                        builder.GenerateInvocationStatement(
                            nameof(War3Api.Common.CreateItem),
                            builder.GenerateVariableExpression(LocalItemIdVariableName),
                            builder.GenerateFloatLiteralExpression(item.PositionX, 1),
                            builder.GenerateFloatLiteralExpression(item.PositionY, 1)));
                }
                else
                {
                    yield return builder.GenerateInvocationStatement(
                        nameof(War3Api.Common.CreateItem),
                        builder.GenerateFourCCExpression(item.TypeId),
                        builder.GenerateFloatLiteralExpression(item.PositionX, 1),
                        builder.GenerateFloatLiteralExpression(item.PositionY, 1));
                }
            }
        }
    }
}
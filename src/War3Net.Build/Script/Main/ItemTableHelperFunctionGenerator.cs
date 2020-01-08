// ------------------------------------------------------------------------------
// <copyright file="ItemTableHelperFunctionGenerator.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.Build.Widget;

namespace War3Net.Build.Script.Main
{
    internal static partial class MainFunctionGenerator<TBuilder, TGlobalDeclarationSyntax, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
    {
        private const string LocalTrigWidgetVariableName = "trigWidget";
        private const string LocalTrigUnitVariableName = "trigUnit";
        private const string LocalCanDropVariableName = "canDrop";

        private static TFunctionSyntax GenerateItemTableHelperFunction(TBuilder builder, int tableIndex)
        {
            var table = builder.Data.MapInfo.GetItemTable(tableIndex);
            var internalTableIndex = table.Index;

            var locals = new List<(string, string, TExpressionSyntax)>()
            {
                (nameof(War3Api.Common.widget), LocalTrigWidgetVariableName, builder.GenerateNullLiteralExpression()),
                (nameof(War3Api.Common.unit), LocalTrigUnitVariableName, builder.GenerateNullLiteralExpression()),
                (builder.GetTypeName(BuiltinType.Int32), LocalItemIdVariableName, builder.GenerateIntegerLiteralExpression(0)),
                (builder.GetTypeName(BuiltinType.Boolean), LocalCanDropVariableName, builder.GenerateBooleanLiteralExpression(true)),
            };

            return builder.Build($"ItemTable{internalTableIndex.ToString("D6")}_DropItems", locals, GetItemTableHelperFunctionStatements(builder, table.ItemSets));
        }

        private static TFunctionSyntax GenerateItemTableHelperFunction(TBuilder builder, MapDoodadData doodad)
        {
            var locals = new List<(string, string, TExpressionSyntax)>()
            {
                (nameof(War3Api.Common.widget), LocalTrigWidgetVariableName, builder.GenerateNullLiteralExpression()),
                (nameof(War3Api.Common.unit), LocalTrigUnitVariableName, builder.GenerateNullLiteralExpression()),
                ("integer", LocalItemIdVariableName, builder.GenerateIntegerLiteralExpression(0)),
                ("boolean", LocalCanDropVariableName, builder.GenerateBooleanLiteralExpression(true)),
            };

            return builder.Build($"Doodad{doodad.CreationNumber.ToString("D6")}_DropItems", locals, GetItemTableHelperFunctionStatements(builder, doodad.DroppedItemSets));
        }

        private static TFunctionSyntax GenerateItemTableHelperFunction(TBuilder builder, MapUnitData unit)
        {
            var locals = new List<(string, string, TExpressionSyntax)>()
            {
                (nameof(War3Api.Common.widget), LocalTrigWidgetVariableName, builder.GenerateNullLiteralExpression()),
                (nameof(War3Api.Common.unit), LocalTrigUnitVariableName, builder.GenerateNullLiteralExpression()),
                ("integer", LocalItemIdVariableName, builder.GenerateIntegerLiteralExpression(0)),
                ("boolean", LocalCanDropVariableName, builder.GenerateBooleanLiteralExpression(true)),
            };

            return builder.Build($"Unit{unit.CreationNumber.ToString("D6")}_DropItems", locals, GetItemTableHelperFunctionStatements(builder, unit.DroppedItemSets));
        }

        private static IEnumerable<TStatementSyntax> GetItemTableHelperFunctionStatements(TBuilder builder, params IEnumerable<(int chance, string id)>[] itemSets)
        {
            yield return builder.GenerateAssignmentStatement(LocalTrigWidgetVariableName, builder.GenerateVariableExpression(nameof(War3Api.Blizzard.bj_lastDyingWidget)));

            yield return builder.GenerateIfStatement(
                builder.GenerateBinaryExpression(
                    BinaryOperator.Equals,
                    builder.GenerateVariableExpression(LocalTrigWidgetVariableName),
                    builder.GenerateNullLiteralExpression()),
                builder.GenerateAssignmentStatement(
                    LocalTrigUnitVariableName,
                    builder.GenerateInvocationExpression(nameof(War3Api.Common.GetTriggerUnit))));

            yield return builder.GenerateIfStatement(
                builder.GenerateBinaryExpression(
                    BinaryOperator.NotEquals,
                    builder.GenerateVariableExpression(LocalTrigUnitVariableName),
                    builder.GenerateNullLiteralExpression()),
                builder.GenerateAssignmentStatement(
                    LocalCanDropVariableName,
                    builder.GenerateUnaryExpression(
                        UnaryOperator.Not,
                        builder.GenerateInvocationExpression(
                            nameof(War3Api.Common.IsUnitHidden),
                            builder.GenerateVariableExpression(LocalTrigUnitVariableName)))),
                builder.GenerateIfStatement(
                    builder.GenerateBinaryExpression(
                        BinaryOperator.And,
                        builder.GenerateVariableExpression(LocalCanDropVariableName),
                        builder.GenerateBinaryExpression(
                            BinaryOperator.NotEquals,
                            builder.GenerateInvocationExpression(nameof(War3Api.Common.GetChangingUnit)),
                            builder.GenerateNullLiteralExpression())),
                    builder.GenerateAssignmentStatement(
                        LocalCanDropVariableName,
                        builder.GenerateBinaryExpression(
                            BinaryOperator.Equals,
                            builder.GenerateInvocationExpression(nameof(War3Api.Common.GetChangingUnitPrevOwner)),
                            builder.GenerateInvocationExpression(
                                nameof(War3Api.Common.Player),
                                builder.GenerateVariableExpression(nameof(War3Api.Common.PLAYER_NEUTRAL_AGGRESSIVE)))))));

            var randomDistStatements = new List<TStatementSyntax>();
            for (var i = 0; i < itemSets.Length; i++)
            {
                var itemSet = itemSets[i];
                randomDistStatements.Add(builder.GenerateInvocationStatement(nameof(War3Api.Blizzard.RandomDistReset)));

                var summedChance = 0;
                foreach (var (chance, id) in itemSet)
                {
                    summedChance += chance;
                    randomDistStatements.Add(
                        builder.GenerateInvocationStatement(
                            nameof(War3Api.Blizzard.RandomDistAddItem),
                            builder.GenerateFourCCExpression(id),
                            builder.GenerateIntegerLiteralExpression(chance)));
                }

                if (summedChance < 100)
                {
                    randomDistStatements.Add(
                        builder.GenerateInvocationStatement(
                            nameof(War3Api.Blizzard.RandomDistAddItem),
                            builder.GenerateIntegerLiteralExpression(-1),
                            builder.GenerateIntegerLiteralExpression(100 - summedChance)));
                }

                randomDistStatements.Add(builder.GenerateAssignmentStatement(
                    LocalItemIdVariableName,
                    builder.GenerateInvocationExpression(nameof(War3Api.Blizzard.RandomDistChoose))));

                randomDistStatements.Add(builder.GenerateElseClause(
                    builder.GenerateIfStatement(
                        builder.GenerateBinaryExpression(
                            BinaryOperator.NotEquals,
                            builder.GenerateVariableExpression(LocalTrigUnitVariableName),
                            builder.GenerateNullLiteralExpression()),
                        builder.GenerateInvocationStatement(
                            nameof(War3Api.Blizzard.UnitDropItem),
                            builder.GenerateVariableExpression(LocalTrigUnitVariableName),
                            builder.GenerateVariableExpression(LocalItemIdVariableName))),
                    null,
                    builder.GenerateInvocationStatement(
                        nameof(War3Api.Blizzard.WidgetDropItem),
                        builder.GenerateVariableExpression(LocalTrigWidgetVariableName),
                        builder.GenerateVariableExpression(LocalItemIdVariableName))));
            }

            yield return builder.GenerateIfStatement(
                builder.GenerateVariableExpression(LocalCanDropVariableName),
                randomDistStatements.ToArray());

            yield return builder.GenerateAssignmentStatement(
                nameof(War3Api.Blizzard.bj_lastDyingWidget),
                builder.GenerateNullLiteralExpression());

            yield return builder.GenerateInvocationStatement(
                nameof(War3Api.Common.DestroyTrigger),
                builder.GenerateInvocationExpression(nameof(War3Api.Common.GetTriggeringTrigger)));
        }
    }
}
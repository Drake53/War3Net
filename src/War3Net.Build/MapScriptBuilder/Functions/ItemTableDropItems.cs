// ------------------------------------------------------------------------------
// <copyright file="ItemTableDropItems.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using War3Net.Build.Common;
using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.Build.Providers;
using War3Net.Build.Widget;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual JassFunctionDeclarationSyntax ItemTableDropItems(Map map, RandomItemTable table)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (table is null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(table.GetDropItemsFunctionName()), GetItemTableDropItemsStatements(map, table.ItemSets, false));
        }

        protected internal virtual JassFunctionDeclarationSyntax ItemTableDropItems(Map map, WidgetData widgetData, int id)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (widgetData is null)
            {
                throw new ArgumentNullException(nameof(widgetData));
            }

            var funcName = widgetData switch
            {
                DoodadData doodadData => doodadData.GetDropItemsFunctionName(id),
                UnitData unitData => unitData.GetDropItemsFunctionName(id),
            };

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(funcName), GetItemTableDropItemsStatements(map, widgetData.ItemTableSets, true));
        }

        protected internal virtual IEnumerable<IStatementSyntax> GetItemTableDropItemsStatements(Map map, IEnumerable<RandomItemSet> itemSets, bool chooseItemClass)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (itemSets is null)
            {
                throw new ArgumentNullException(nameof(itemSets));
            }

            var statements = new List<IStatementSyntax>();
            statements.Add(SyntaxFactory.LocalVariableDeclarationStatement(SyntaxFactory.ParseTypeName(TypeName.Widget), VariableName.TrigWidget, JassNullLiteralExpressionSyntax.Value));
            statements.Add(SyntaxFactory.LocalVariableDeclarationStatement(SyntaxFactory.ParseTypeName(TypeName.Unit), VariableName.TrigUnit, JassNullLiteralExpressionSyntax.Value));
            statements.Add(SyntaxFactory.LocalVariableDeclarationStatement(JassTypeSyntax.Integer, VariableName.ItemId, SyntaxFactory.LiteralExpression(0)));
            statements.Add(SyntaxFactory.LocalVariableDeclarationStatement(JassTypeSyntax.Boolean, VariableName.CanDrop, SyntaxFactory.LiteralExpression(true)));
            statements.Add(JassEmptyStatementSyntax.Value);

            statements.Add(SyntaxFactory.SetStatement(
                VariableName.TrigWidget,
                SyntaxFactory.VariableReferenceExpression(VariableName.BJLastDyingWidget)));

            statements.Add(SyntaxFactory.IfStatement(
                new JassParenthesizedExpressionSyntax(SyntaxFactory.BinaryEqualsExpression(SyntaxFactory.VariableReferenceExpression(VariableName.TrigWidget), JassNullLiteralExpressionSyntax.Value)),
                SyntaxFactory.SetStatement(VariableName.TrigUnit, SyntaxFactory.InvocationExpression(NativeName.GetTriggerUnit))));

            statements.Add(JassEmptyStatementSyntax.Value);

            var canDropConditionExpression = SyntaxFactory.UnaryNotExpression(SyntaxFactory.InvocationExpression(NativeName.IsUnitHidden, SyntaxFactory.VariableReferenceExpression(VariableName.TrigUnit)));

            var ifBody = new List<IStatementSyntax>()
            {
                SyntaxFactory.SetStatement(VariableName.CanDrop, canDropConditionExpression),
            };

            ifBody.Add(SyntaxFactory.IfStatement(
                new JassParenthesizedExpressionSyntax(SyntaxFactory.BinaryAndExpression(
                    SyntaxFactory.VariableReferenceExpression(VariableName.CanDrop),
                    SyntaxFactory.BinaryNotEqualsExpression(SyntaxFactory.InvocationExpression(NativeName.GetChangingUnit), JassNullLiteralExpressionSyntax.Value))),
                SyntaxFactory.SetStatement(
                    VariableName.CanDrop,
                    new JassParenthesizedExpressionSyntax(SyntaxFactory.BinaryEqualsExpression(
                        SyntaxFactory.InvocationExpression(NativeName.GetChangingUnitPrevOwner),
                        SyntaxFactory.InvocationExpression(NativeName.Player, SyntaxFactory.VariableReferenceExpression(GlobalVariableName.PlayerNeutralHostile)))))));

            statements.Add(SyntaxFactory.IfStatement(
                new JassParenthesizedExpressionSyntax(SyntaxFactory.BinaryNotEqualsExpression(SyntaxFactory.VariableReferenceExpression(VariableName.TrigUnit), JassNullLiteralExpressionSyntax.Value)),
                ifBody.ToArray()));
            statements.Add(JassEmptyStatementSyntax.Value);

            var i = 0;
            var randomDistStatements = new List<IStatementSyntax>();
            foreach (var itemSet in itemSets)
            {
                randomDistStatements.Add(new JassCommentStatementSyntax($" Item set {i}"));
                randomDistStatements.Add(SyntaxFactory.CallStatement(FunctionName.RandomDistReset));

                var summedChance = 0;
                foreach (var item in itemSet.Items)
                {
                    if (RandomItemProvider.IsRandomItem(item.ItemId, out var itemClass, out var level))
                    {
                        if (chooseItemClass)
                        {
                            randomDistStatements.Add(SyntaxFactory.CallStatement(
                                FunctionName.RandomDistAddItem,
                                SyntaxFactory.InvocationExpression(
                                    NativeName.ChooseRandomItemEx,
                                    SyntaxFactory.VariableReferenceExpression(itemClass.GetVariableName()),
                                    SyntaxFactory.LiteralExpression(level)),
                                SyntaxFactory.LiteralExpression(item.Chance)));
                        }
                        else
                        {
                            randomDistStatements.Add(SyntaxFactory.CallStatement(
                                FunctionName.RandomDistAddItem,
                                SyntaxFactory.InvocationExpression(NativeName.ChooseRandomItem, SyntaxFactory.LiteralExpression(level)),
                                SyntaxFactory.LiteralExpression(item.Chance)));
                        }
                    }
                    else
                    {
                        randomDistStatements.Add(SyntaxFactory.CallStatement(
                            FunctionName.RandomDistAddItem,
                            SyntaxFactory.FourCCLiteralExpression(item.ItemId),
                            SyntaxFactory.LiteralExpression(item.Chance)));
                    }

                    summedChance += item.Chance;
                }

                if (summedChance < 100)
                {
                    randomDistStatements.Add(SyntaxFactory.CallStatement(
                        FunctionName.RandomDistAddItem,
                        SyntaxFactory.LiteralExpression(-1),
                        SyntaxFactory.LiteralExpression(100 - summedChance)));
                }

                var unitDropItemStatement = SyntaxFactory.CallStatement(
                    FunctionName.UnitDropItem,
                    SyntaxFactory.VariableReferenceExpression(VariableName.TrigUnit),
                    SyntaxFactory.VariableReferenceExpression(VariableName.ItemId));

                randomDistStatements.Add(SyntaxFactory.SetStatement(VariableName.ItemId, SyntaxFactory.InvocationExpression(FunctionName.RandomDistChoose)));

                randomDistStatements.Add(SyntaxFactory.IfStatement(
                    new JassParenthesizedExpressionSyntax(SyntaxFactory.BinaryNotEqualsExpression(SyntaxFactory.VariableReferenceExpression(VariableName.TrigUnit), JassNullLiteralExpressionSyntax.Value)),
                    SyntaxFactory.StatementList(unitDropItemStatement),
                    new JassElseClauseSyntax(SyntaxFactory.StatementList(SyntaxFactory.CallStatement(
                        FunctionName.WidgetDropItem,
                        SyntaxFactory.VariableReferenceExpression(VariableName.TrigWidget),
                        SyntaxFactory.VariableReferenceExpression(VariableName.ItemId))))));

                randomDistStatements.Add(JassEmptyStatementSyntax.Value);

                i++;
            }

            statements.Add(SyntaxFactory.IfStatement(
                new JassParenthesizedExpressionSyntax(SyntaxFactory.VariableReferenceExpression(VariableName.CanDrop)),
                randomDistStatements.ToArray()));
            statements.Add(JassEmptyStatementSyntax.Value);

            statements.Add(SyntaxFactory.SetStatement(VariableName.BJLastDyingWidget, JassNullLiteralExpressionSyntax.Value));
            statements.Add(SyntaxFactory.CallStatement(NativeName.DestroyTrigger, SyntaxFactory.InvocationExpression(NativeName.GetTriggeringTrigger)));

            return statements;
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="CreateAllItems.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.Build.Providers;
using War3Net.Build.Widget;
using War3Net.CodeAnalysis.Jass.Syntax;

using static War3Api.Common;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected virtual JassFunctionDeclarationSyntax CreateAllItems(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapUnits = map.Units;
            if (mapUnits is null)
            {
                throw new ArgumentException($"Function '{nameof(CreateAllItems)}' cannot be generated without {nameof(MapUnits)}.", nameof(map));
            }

            var statements = new List<IStatementSyntax>();

            statements.Add(SyntaxFactory.LocalVariableDeclarationStatement(JassTypeSyntax.Integer, VariableName.ItemId));

            foreach (var item in mapUnits.Units.Where(item => item.IsItem() && !item.IsPlayerStartLocation()))
            {
                if (item.IsRandomItem())
                {
                    var randomData = item.RandomData;
                    switch (randomData)
                    {
                        case RandomUnitAny randomUnitAny:
                            statements.Add(SyntaxFactory.SetStatement(
                                VariableName.ItemId,
                                SyntaxFactory.InvocationExpression(
                                    nameof(ChooseRandomItemEx),
                                    SyntaxFactory.InvocationExpression(nameof(ConvertItemType), SyntaxFactory.LiteralExpression((int)randomUnitAny.Class)),
                                    SyntaxFactory.LiteralExpression(randomUnitAny.Level))));

                            break;

                        case RandomUnitGlobalTable randomUnitGlobalTable:
                            break;

                        case RandomUnitCustomTable randomUnitCustomTable:
                            statements.Add(SyntaxFactory.CallStatement(nameof(War3Api.Blizzard.RandomDistReset)));

                            var summedChance = 0;
                            foreach (var randomItem in randomUnitCustomTable.RandomUnits)
                            {
                                IExpressionSyntax id = RandomItemProvider.IsRandomItem(randomItem.UnitId, out var itemClass, out var level)
                                    ? SyntaxFactory.InvocationExpression(
                                        nameof(ChooseRandomItemEx),
                                        SyntaxFactory.InvocationExpression(nameof(ConvertItemType), SyntaxFactory.LiteralExpression((int)itemClass)),
                                        SyntaxFactory.LiteralExpression(level))
                                    : SyntaxFactory.FourCCLiteralExpression(randomItem.UnitId);

                                statements.Add(SyntaxFactory.CallStatement(
                                    nameof(War3Api.Blizzard.RandomDistAddItem),
                                    id,
                                    SyntaxFactory.LiteralExpression(randomItem.Chance)));

                                summedChance += randomItem.Chance;
                            }

                            if (summedChance < 100)
                            {
                                statements.Add(SyntaxFactory.CallStatement(
                                    nameof(War3Api.Blizzard.RandomDistAddItem),
                                    SyntaxFactory.LiteralExpression(-1),
                                    SyntaxFactory.LiteralExpression(100 - summedChance)));
                            }

                            statements.Add(SyntaxFactory.SetStatement(
                                VariableName.ItemId,
                                SyntaxFactory.InvocationExpression(nameof(War3Api.Blizzard.RandomDistChoose))));

                            break;

                        default:
                            break;
                    }

                    statements.Add(SyntaxFactory.IfStatement(
                        SyntaxFactory.BinaryNotEqualsExpression(SyntaxFactory.VariableReferenceExpression(VariableName.ItemId), SyntaxFactory.LiteralExpression(-1)),
                        SyntaxFactory.CallStatement(
                            nameof(CreateItem),
                            SyntaxFactory.VariableReferenceExpression(VariableName.ItemId),
                            SyntaxFactory.LiteralExpression(item.Position.X),
                            SyntaxFactory.LiteralExpression(item.Position.Y))));
                }
                else
                {
                    var args = new List<IExpressionSyntax>()
                    {
                        SyntaxFactory.FourCCLiteralExpression(item.TypeId),
                        SyntaxFactory.LiteralExpression(item.Position.X),
                        SyntaxFactory.LiteralExpression(item.Position.Y),
                    };

                    var hasSkin = item.SkinId != 0 && item.SkinId != item.TypeId;
                    if (hasSkin)
                    {
                        args.Add(SyntaxFactory.FourCCLiteralExpression(item.SkinId));
                    }

                    statements.Add(SyntaxFactory.CallStatement(hasSkin ? nameof(BlzCreateItemWithSkin) : nameof(CreateItem), args.ToArray()));
                }
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(CreateAllItems)), statements);
        }

        protected virtual bool CreateAllItemsCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Units is not null && (map.Info.FormatVersion == MapInfoFormatVersion.v8 || map.Units.Units.Any(item => item.IsItem() && !item.IsPlayerStartLocation()));
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="InitRandomGroups.cs" company="Drake53">
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
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual JassFunctionDeclarationSyntax InitRandomGroups(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var randomUnitTables = map.Info?.RandomUnitTables;
            if (randomUnitTables is null)
            {
                throw new ArgumentException($"Function '{nameof(InitRandomGroups)}' cannot be generated without {nameof(MapInfo.RandomUnitTables)}.", nameof(map));
            }

            var statements = new List<JassStatementSyntax>();

            statements.Add(SyntaxFactory.LocalVariableDeclarationStatement(JassPredefinedTypeSyntax.Integer, VariableName.CurrentSet));
            //statements.Add(JassEmptySyntax.Value);

            foreach (var unitTable in randomUnitTables)
            {
                //statements.Add(new JassCommentSyntax($" Group {unitTable.Index} - {unitTable.Name}"));
                statements.Add(SyntaxFactory.CallStatement(FunctionName.RandomDistReset));

                for (var i = 0; i < unitTable.UnitSets.Count; i++)
                {
                    statements.Add(SyntaxFactory.CallStatement(
                        FunctionName.RandomDistAddItem,
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(i)),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(unitTable.UnitSets[i].Chance))));
                }

                statements.Add(SyntaxFactory.SetStatement(VariableName.CurrentSet, SyntaxFactory.InvocationExpression(FunctionName.RandomDistChoose)));
                //statements.Add(JassEmptySyntax.Value);

                var groupVarName = unitTable.GetVariableName();
                var ifElseifBlocks = new List<(JassExpressionSyntax Condition, JassStatementSyntax[] Body)>();
                for (var setIndex = 0; setIndex < unitTable.UnitSets.Count; setIndex++)
                {
                    var set = unitTable.UnitSets[setIndex];

                    var condition = SyntaxFactory.BinaryEqualsExpression(SyntaxFactory.ParseIdentifierName(VariableName.CurrentSet), SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(setIndex)));
                    var bodyStatements = new List<JassStatementSyntax>();

                    for (var position = 0; position < unitTable.Types.Count; position++)
                    {
                        var id = set?.UnitIds[position] ?? 0;
                        var unitTypeExpression = RandomUnitProvider.IsRandomUnit(id, out var level)
                            ? SyntaxFactory.InvocationExpression(NativeName.ChooseRandomCreep, SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(level)))
                            : id == 0 ? SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(-1)) : SyntaxFactory.LiteralExpression(SyntaxFactory.FourCCLiteral(id));

                        bodyStatements.Add(SyntaxFactory.SetStatement(
                            groupVarName,
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(position)),
                            unitTypeExpression));
                    }

                    ifElseifBlocks.Add((SyntaxFactory.ParenthesizedExpression(condition), bodyStatements.ToArray()));
                }

                var elseClauseStatements = new List<JassStatementSyntax>();
                for (var position = 0; position < unitTable.Types.Count; position++)
                {
                    elseClauseStatements.Add(SyntaxFactory.SetStatement(
                        groupVarName,
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(position)),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(-1))));
                }

                statements.Add(SyntaxFactory.IfStatement(
                    ifElseifBlocks.First().Condition,
                    ifElseifBlocks.First().Body,
                    ifElseifBlocks.Skip(1).Select(elseIf => SyntaxFactory.ElseIfClause(elseIf.Condition, elseIf.Body)),
                    SyntaxFactory.ElseClause(elseClauseStatements)));

                //statements.Add(JassEmptySyntax.Value);
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(InitRandomGroups)), statements);
        }

        protected internal virtual bool InitRandomGroupsCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info?.RandomUnitTables is not null
                && map.Info.RandomUnitTables.Any();
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="InitGlobals.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Extensions;
using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual JassFunctionDeclarationSyntax InitGlobals(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapTriggers = map.Triggers;
            if (mapTriggers is null)
            {
                throw new ArgumentException($"Function '{nameof(InitGlobals)}' cannot be generated without {nameof(MapTriggers)}.", nameof(map));
            }

            var statements = new List<IStatementSyntax>();

            if (mapTriggers.Variables.Any(variable => variable.IsInitialized && variable.IsArray))
            {
                statements.Add(SyntaxFactory.LocalVariableDeclarationStatement(
                    JassTypeSyntax.Integer,
                    "i",
                    SyntaxFactory.LiteralExpression(0)));
            }

            foreach (var variable in mapTriggers.Variables)
            {
                if (!variable.IsInitialized)
                {
                    continue;
                }

                var initialValue = TriggerData.TryGetTriggerParamPresetValue(variable.Type, variable.InitialValue, out var codeText)
                    ? codeText
                    : variable.InitialValue;

                if (variable.IsArray)
                {
                    statements.Add(SyntaxFactory.SetStatement(
                        "i",
                        SyntaxFactory.LiteralExpression(0)));

                    statements.Add(SyntaxFactory.LoopStatement(
                        SyntaxFactory.ExitStatement(SyntaxFactory.ParenthesizedExpression(SyntaxFactory.BinaryGreaterThanExpression(
                            SyntaxFactory.VariableReferenceExpression("i"),
                            SyntaxFactory.LiteralExpression(variable.ArraySize)))),
                        SyntaxFactory.SetStatement(
                            variable.GetVariableName(),
                            SyntaxFactory.VariableReferenceExpression("i"),
                            SyntaxFactory.ParseExpression(initialValue)),
                        SyntaxFactory.SetStatement(
                            "i",
                            SyntaxFactory.BinaryAdditionExpression(
                                SyntaxFactory.VariableReferenceExpression("i"),
                                SyntaxFactory.LiteralExpression(1)))));

                    statements.Add(JassEmptyStatementSyntax.Value);
                }
                else
                {
                    statements.Add(SyntaxFactory.SetStatement(
                        variable.GetVariableName(),
                        SyntaxFactory.ParseExpression(initialValue)));
                }
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(InitGlobals)), statements);
        }

        protected internal virtual bool InitGlobalsCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Triggers is not null;
        }
    }
}
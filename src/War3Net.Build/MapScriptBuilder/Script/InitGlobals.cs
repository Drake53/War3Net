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
using War3Net.Build.Info;
using War3Net.Build.Providers;
using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass;
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

            var statements = new List<JassStatementSyntax>();

            if (mapTriggers.Variables.Any(variable =>
                (variable.IsArray ||
                 map.Info.FormatVersion >= MapInfoFormatVersion.v31) &&
                (variable.IsInitialized ||
                 TriggerData.TriggerTypeDefaults.TryGetValue(variable.Type, out _) ||
                 string.Equals(variable.Type, JassKeyword.String, StringComparison.Ordinal))))
            {
                statements.Add(SyntaxFactory.LocalVariableDeclarationStatement(
                    JassPredefinedTypeSyntax.Integer,
                    "i",
                    SyntaxFactory.LiteralExpression(0)));
            }

            foreach (var variable in mapTriggers.Variables)
            {
                if (variable.IsInitialized)
                {
                    var initialValue = TriggerData.TriggerParams.TryGetValue(variable.InitialValue, out var triggerParam) && string.Equals(triggerParam.VariableType, variable.Type, StringComparison.Ordinal)
                        ? triggerParam.ScriptText
                        : string.Equals(variable.Type, JassKeyword.String, StringComparison.Ordinal)
                            ? $"\"{EscapedStringProvider.GetEscapedString(variable.InitialValue)}\""
                            : variable.InitialValue;

                    statements.AddRange(InitGlobal(variable, SyntaxFactory.ParseExpression(initialValue)));
                }
                else if (TriggerData.TriggerTypeDefaults.TryGetValue(variable.Type, out var triggerTypeDefault))
                {
                    statements.AddRange(InitGlobal(variable, SyntaxFactory.ParseExpression(triggerTypeDefault.ScriptText)));
                }
                else if (string.Equals(variable.Type, JassKeyword.String, StringComparison.Ordinal))
                {
                    statements.AddRange(InitGlobal(variable, SyntaxFactory.LiteralExpression(string.Empty)));
                }
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(InitGlobals)), statements);
        }

        protected internal virtual IEnumerable<JassStatementSyntax> InitGlobal(VariableDefinition variable, JassExpressionSyntax expression)
        {
            if (variable is null)
            {
                throw new ArgumentNullException(nameof(variable));
            }

            if (variable.IsArray)
            {
                yield return SyntaxFactory.SetStatement(
                    "i",
                    SyntaxFactory.LiteralExpression(0));

                yield return SyntaxFactory.LoopStatement(
                    SyntaxFactory.ExitStatement(SyntaxFactory.ParenthesizedExpression(SyntaxFactory.BinaryGreaterThanExpression(
                        SyntaxFactory.ParseIdentifierName("i"),
                        SyntaxFactory.LiteralExpression(variable.ArraySize)))),
                    SyntaxFactory.SetStatement(
                        variable.GetVariableName(),
                        SyntaxFactory.ParseIdentifierName("i"),
                        expression),
                    SyntaxFactory.SetStatement(
                        "i",
                        SyntaxFactory.BinaryAdditionExpression(
                            SyntaxFactory.ParseIdentifierName("i"),
                            SyntaxFactory.LiteralExpression(1))));

                //yield return JassEmptySyntax.Value;
            }
            else
            {
                yield return SyntaxFactory.SetStatement(
                    variable.GetVariableName(),
                    expression);
            }
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
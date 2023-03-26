// ------------------------------------------------------------------------------
// <copyright file="RunInitializationTriggers.cs" company="Drake53">
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
        protected internal virtual JassFunctionDeclarationSyntax RunInitializationTriggers(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapTriggers = map.Triggers;
            if (mapTriggers is null)
            {
                throw new ArgumentException($"Function '{nameof(RunInitializationTriggers)}' cannot be generated without {nameof(MapTriggers)}.", nameof(map));
            }

            var statements = new List<JassStatementSyntax>();

            foreach (var trigger in mapTriggers.TriggerItems)
            {
                if (trigger is TriggerDefinition triggerDefinition &&
                    RunInitializationTriggersConditionSingleTrigger(map, triggerDefinition))
                {
                    statements.Add(SyntaxFactory.CallStatement(
                        NativeName.ConditionalTriggerExecute,
                        SyntaxFactory.ParseIdentifierName(triggerDefinition.GetVariableName())));
                }
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(RunInitializationTriggers)), statements);
        }

        protected internal virtual bool RunInitializationTriggersCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Triggers is not null
                && map.Triggers.TriggerItems.Any(trigger =>
                       trigger is TriggerDefinition triggerDefinition &&
                       RunInitializationTriggersConditionSingleTrigger(map, triggerDefinition));
        }

        protected internal virtual bool RunInitializationTriggersConditionSingleTrigger(Map map, TriggerDefinition triggerDefinition)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (triggerDefinition is null)
            {
                throw new ArgumentNullException(nameof(triggerDefinition));
            }

            return triggerDefinition.IsEnabled
                && triggerDefinition.IsInitiallyOn
                && triggerDefinition.Functions.Any(function =>
                       function.Type == TriggerFunctionType.Event &&
                       function.IsEnabled &&
                       string.Equals(function.Name, TriggerFunctionName.MapInitializationEvent, StringComparison.Ordinal));
        }
    }
}
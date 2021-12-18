// ------------------------------------------------------------------------------
// <copyright file="InitCustomTriggers.cs" company="Drake53">
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
        protected internal virtual JassFunctionDeclarationSyntax InitCustomTriggers(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapTriggers = map.Triggers;
            if (mapTriggers is null)
            {
                throw new ArgumentException($"Function '{nameof(InitCustomTriggers)}' cannot be generated without {nameof(MapTriggers)}.", nameof(map));
            }

            var statements = new List<IStatementSyntax>();

            foreach (var trigger in mapTriggers.TriggerItems)
            {
                if (trigger is TriggerDefinition triggerDefinition &&
                    triggerDefinition.IsEnabled)
                {
                    statements.Add(SyntaxFactory.CallStatement(triggerDefinition.GetInitTrigFunctionName()));
                }
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(InitCustomTriggers)), statements);
        }

        protected internal virtual bool InitCustomTriggersCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Triggers is not null
                && map.Triggers.TriggerItems.Any(trigger => trigger is TriggerDefinition triggerDefinition && triggerDefinition.IsEnabled);
        }
    }
}
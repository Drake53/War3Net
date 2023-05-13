// ------------------------------------------------------------------------------
// <copyright file="Triggers.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using War3Net.Build.Extensions;
using War3Net.Build.Script;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual IEnumerable<JassGlobalDeclarationSyntax> Triggers(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapTriggers = map.Triggers;
            if (mapTriggers is null)
            {
                yield break;
            }

            foreach (var trigger in mapTriggers.TriggerItems)
            {
                if (trigger is TriggerDefinition triggerDefinition &&
                    !triggerDefinition.IsComment)
                {
                    yield return SyntaxFactory.GlobalVariableDeclaration(
                        SyntaxFactory.ParseTypeName(TypeName.Trigger),
                        triggerDefinition.GetVariableName(),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(null)));
                }
            }
        }
    }
}
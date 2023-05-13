// ------------------------------------------------------------------------------
// <copyright file="Variables.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using War3Net.Build.Extensions;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual IEnumerable<JassGlobalDeclarationSyntax> Variables(Map map)
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

            foreach (var variable in mapTriggers.Variables)
            {
                var type = TriggerData.TriggerTypes.TryGetValue(variable.Type, out var triggerType) && !string.IsNullOrEmpty(triggerType.BaseType)
                    ? triggerType.BaseType
                    : variable.Type;

                if (variable.IsArray)
                {
                    yield return SyntaxFactory.GlobalArrayDeclaration(
                        SyntaxFactory.ParseTypeName(type),
                        variable.GetVariableName());
                }
                else if (string.Equals(variable.Type, JassKeyword.String, StringComparison.Ordinal))
                {
                    yield return SyntaxFactory.GlobalVariableDeclaration(
                        JassPredefinedTypeSyntax.String,
                        variable.GetVariableName());
                }
                else
                {
                    var value = variable.Type switch
                    {
                        JassKeyword.Integer => SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(0)),
                        JassKeyword.Real => SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(0)),
                        JassKeyword.Boolean => SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(false)),

                        _ => SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(null)),
                    };

                    yield return SyntaxFactory.GlobalVariableDeclaration(
                        SyntaxFactory.ParseTypeName(type),
                        variable.GetVariableName(),
                        value);
                }
            }
        }
    }
}
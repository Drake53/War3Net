// ------------------------------------------------------------------------------
// <copyright file="InitTrig.cs" company="Drake53">
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
        protected internal virtual JassFunctionDeclarationSyntax InitTrig(Map map, TriggerDefinition triggerDefinition)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (triggerDefinition is null)
            {
                throw new ArgumentNullException(nameof(triggerDefinition));
            }

            var triggerVariableName = triggerDefinition.GetVariableName();

            var statements = new List<IStatementSyntax>();

            statements.Add(SyntaxFactory.SetStatement(
                triggerVariableName,
                SyntaxFactory.InvocationExpression(NativeName.CreateTrigger)));

            if (!triggerDefinition.IsInitiallyOn)
            {
                statements.Add(SyntaxFactory.CallStatement(
                    NativeName.DisableTrigger,
                    SyntaxFactory.FunctionReferenceExpression(triggerVariableName)));
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(triggerDefinition.GetInitTrigFunctionName()), statements);
        }

        protected internal virtual bool InitTrigCondition(Map map, TriggerDefinition triggerDefinition)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (triggerDefinition is null)
            {
                throw new ArgumentNullException(nameof(triggerDefinition));
            }

            return triggerDefinition.IsEnabled;
        }
    }
}
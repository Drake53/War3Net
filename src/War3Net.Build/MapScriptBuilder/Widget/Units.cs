// ------------------------------------------------------------------------------
// <copyright file="Units.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.CodeAnalysis.CSharp.Syntax;

using War3Net.Build.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.CodeAnalysis.Transpilers;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        public virtual IEnumerable<MemberDeclarationSyntax> UnitsApi(Map map, JassToCSharpTranspiler transpiler)
        {
            if (transpiler is null)
            {
                throw new ArgumentNullException(nameof(transpiler));
            }

            return Units(map).Select(unit => transpiler.Transpile(unit));
        }

        protected internal virtual IEnumerable<JassGlobalDeclarationSyntax> Units(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapUnits = map.Units;
            if (mapUnits is null)
            {
                yield break;
            }

            foreach (var unit in mapUnits.Units.Where(unit => CreateAllUnitsConditionSingleUnit(map, unit)))
            {
                var unitVariableName = unit.GetVariableName();
                if (ForceGenerateGlobalUnitVariable || TriggerVariableReferences.ContainsKey(unitVariableName))
                {
                    yield return SyntaxFactory.GlobalDeclaration(
                        SyntaxFactory.ParseTypeName(TypeName.Unit),
                        unitVariableName,
                        JassNullLiteralExpressionSyntax.Value);
                }
            }
        }
    }
}
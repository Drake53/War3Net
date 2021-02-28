// ------------------------------------------------------------------------------
// <copyright file="CreateNeutralHostile.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;

using War3Net.Build.Extensions;
using War3Net.Build.Widget;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected virtual JassFunctionDeclarationSyntax CreateNeutralHostile(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapUnits = map.Units;
            if (mapUnits is null)
            {
                throw new ArgumentException($"Function '{nameof(CreateNeutralHostile)}' cannot be generated without {nameof(MapUnits)}.", nameof(map));
            }

            return SyntaxFactory.FunctionDeclaration(
                SyntaxFactory.FunctionDeclarator(nameof(CreateNeutralHostile)),
                CreateUnits(
                    map,
                    mapUnits.Units.IncludeId().Where(pair => CreateNeutralHostileConditionSingleUnit(map, pair.Obj)),
                    SyntaxFactory.VariableReferenceExpression(GlobalVariableName.PlayerNeutralHostile)));
        }

        protected virtual bool CreateNeutralHostileCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Units is not null
                && map.Units.Units.Any(unit => CreateNeutralHostileConditionSingleUnit(map, unit));
        }

        protected virtual bool CreateNeutralHostileConditionSingleUnit(Map map, UnitData unitData)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (unitData is null)
            {
                throw new ArgumentNullException(nameof(unitData));
            }

            var neutralHostileId = MaxPlayerSlots;
            return unitData.OwnerId == neutralHostileId && unitData.IsUnit();
        }
    }
}
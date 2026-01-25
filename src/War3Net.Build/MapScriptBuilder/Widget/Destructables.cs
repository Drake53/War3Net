// ------------------------------------------------------------------------------
// <copyright file="Destructables.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;

using War3Net.Build.Extensions;
using War3Net.Build.Widget;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateDestructableVariables(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var mapDoodads = map.Doodads;
            if (mapDoodads is null)
            {
                return;
            }

            foreach (var destructable in mapDoodads.Doodads.Where(ShouldGenerateDestructableVariable))
            {
                writer.WriteAlignedGlobal(
                    TypeName.Destructable,
                    destructable.GetVariableName(),
                    JassKeyword.Null);
            }
        }

        protected internal virtual bool ShouldGenerateDestructableVariables(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Doodads is not null
                && map.Doodads.Doodads.Any(ShouldGenerateDestructableVariable);
        }

        protected internal virtual bool ShouldGenerateDestructableVariable(DoodadData doodadData)
        {
            if (doodadData is null)
            {
                throw new ArgumentNullException(nameof(doodadData));
            }

            return ForceGenerateGlobalDestructableVariable
                || (TriggerVariableReferences.TryGetValue(doodadData.GetVariableName(), out var value) && value)
                || doodadData.HasItemTable();
        }
    }
}
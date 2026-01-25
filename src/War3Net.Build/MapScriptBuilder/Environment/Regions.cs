// ------------------------------------------------------------------------------
// <copyright file="Regions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

using War3Net.Build.Extensions;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateRegionVariables(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var mapRegions = map.Regions;
            if (mapRegions is null)
            {
                return;
            }

            foreach (var region in mapRegions.Regions)
            {
                writer.WriteAlignedGlobal(
                    TypeName.Rect,
                    region.GetVariableName(),
                    JassKeyword.Null);
            }
        }

        protected internal virtual bool ShouldGenerateRegionVariables(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Regions is not null
                && map.Regions.Regions.Count > 0;
        }
    }
}
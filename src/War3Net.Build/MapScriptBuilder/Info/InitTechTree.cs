// ------------------------------------------------------------------------------
// <copyright file="InitTechTree.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateInitTechTree(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            writer.WriteFunction(GeneratedFunctionName.InitTechTree);

            for (var i = 0; i < MaxPlayerSlots; i++)
            {
                if (ShouldGenerateInitTechTreeForPlayer(map, i))
                {
                    writer.WriteCall(GeneratedFunctionName.InitTechTreeForPlayer(i));
                }
            }

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateInitTechTree(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null
                && map.Info.TechData.Count > 0;
        }
    }
}
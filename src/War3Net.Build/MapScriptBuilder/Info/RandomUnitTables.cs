using System;
using War3Net.Build.Extensions;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateRandomUnitTables(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var randomUnitTables = map.Info?.RandomUnitTables;
            if (randomUnitTables is null)
            {
                return;
            }

            var id = 0;
            foreach (var randomUnitTable in randomUnitTables)
            {
                writer.WriteAlignedGlobal(
                    $"{JassKeyword.Integer} {JassKeyword.Array}",
                    randomUnitTable.GetVariableName(id));

                id++;
            }
        }

        protected internal virtual bool ShouldGenerateRandomUnitTables(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info?.RandomUnitTables is not null
                && map.Info.RandomUnitTables.Count > 0;
        }
    }
}
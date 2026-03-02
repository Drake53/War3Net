using System;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        public virtual void GenerateGlobals(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var shouldGenerateUserDefinedVariables = ShouldGenerateUserDefinedVariables(map);
            var shouldGenerateRegionVariables = ShouldGenerateRegionVariables(map);
            var shouldGenerateCameraVariables = ShouldGenerateCameraVariables(map);
            var shouldGenerateSoundVariables = ShouldGenerateSoundVariables(map);
            var shouldGenerateTriggerVariables = ShouldGenerateTriggerVariables(map);
            var shouldGenerateUnitVariables = ShouldGenerateUnitVariables(map);
            var shouldGenerateDestructableVariables = ShouldGenerateDestructableVariables(map);
            var shouldGenerateRandomUnitTables = ShouldGenerateRandomUnitTables(map);

            writer.WriteLine(JassKeyword.Globals);
            writer.Indent();

            if (shouldGenerateUserDefinedVariables)
            {
                writer.WriteComment("User-defined");
                GenerateUserDefinedVariables(map, writer);
            }

            if (shouldGenerateRegionVariables ||
                shouldGenerateCameraVariables ||
                shouldGenerateSoundVariables ||
                shouldGenerateTriggerVariables ||
                shouldGenerateUnitVariables ||
                shouldGenerateDestructableVariables ||
                shouldGenerateRandomUnitTables)
            {
                if (shouldGenerateUserDefinedVariables)
                {
                    writer.WriteLine();
                }

                writer.WriteComment("Generated");

                if (shouldGenerateRegionVariables)
                {
                    GenerateRegionVariables(map, writer);
                }

                if (shouldGenerateCameraVariables)
                {
                    GenerateCameraVariables(map, writer);
                }

                if (shouldGenerateSoundVariables)
                {
                    GenerateSoundVariables(map, writer);
                }

                if (shouldGenerateTriggerVariables)
                {
                    GenerateTriggerVariables(map, writer);
                }

                if (shouldGenerateUnitVariables)
                {
                    GenerateUnitVariables(map, writer);
                }

                if (shouldGenerateDestructableVariables)
                {
                    GenerateDestructableVariables(map, writer);
                }

                if (shouldGenerateRandomUnitTables)
                {
                    GenerateRandomUnitTables(map, writer);
                }
            }

            writer.Unindent();
            writer.WriteLine(JassKeyword.EndGlobals);
        }
    }
}
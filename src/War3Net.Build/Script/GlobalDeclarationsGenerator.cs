// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationsGenerator.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace War3Net.Build.Script
{
    internal static partial class GlobalDeclarationsGenerator<TBuilder, TGlobalDeclarationSyntax, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
        where TBuilder : FunctionBuilder<TGlobalDeclarationSyntax, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
        where TGlobalDeclarationSyntax : class
        where TExpressionSyntax : class
    {
        public static IEnumerable<TGlobalDeclarationSyntax?> GetGlobals(TBuilder builder)
        {
            var mapInfo = builder.Data.MapInfo;
            for (var i = 0; i < mapInfo.RandomUnitTableCount; i++)
            {
                yield return builder.GenerateGlobalDeclaration(
                    builder.GetTypeName(BuiltinType.Int32),
                    $"gg_rg_{mapInfo.GetUnitTable(i).Index:D3}",
                    true);
            }

            var mapSounds = builder.Data.MapSounds;
            if ((mapSounds?.Count ?? 0) > 0)
            {
                foreach (var sound in mapSounds)
                {
                    yield return builder.GenerateGlobalDeclaration(
                        nameof(War3Api.Common.sound),
                        sound.Name,
                        false);
                }
            }

            var mapRegions = builder.Data.MapRegions;
            if (mapRegions?.Where(region => region.WeatherId != "\0\0\0\0" || region.AmbientSound != null) != null)
            {
                foreach (var region in mapRegions)
                {
                    yield return builder.GenerateGlobalDeclaration(
                        nameof(War3Api.Common.rect),
                        $"gg_rct_{region.Name.Replace(' ', '_')}",
                        false);
                }
            }

            var mapUnits = builder.Data.MapUnits;
            if (mapUnits?.Where(mapUnit => mapUnit.IsUnit).FirstOrDefault() != null)
            {
                foreach (var unit in builder.Data.MapUnits.Where(mapUnit => mapUnit.IsUnit))
                {
                    yield return builder.GenerateGlobalDeclaration(
                        nameof(War3Api.Common.unit),
                        $"gg_unit_{unit.TypeId}_{unit.CreationNumber:D4}",
                        false);
                }
            }
        }
    }
}
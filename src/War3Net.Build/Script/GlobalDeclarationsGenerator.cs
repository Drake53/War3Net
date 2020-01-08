// ------------------------------------------------------------------------------
// <copyright file="GlobalDeclarationsGenerator.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Script
{
    internal static partial class GlobalDeclarationsGenerator<TBuilder, TGlobalDeclarationSyntax, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
        where TBuilder : FunctionBuilder<TGlobalDeclarationSyntax, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
        where TExpressionSyntax : class
    {
        public static IEnumerable<TGlobalDeclarationSyntax> GetGlobals(TBuilder builder)
        {
            var mapInfo = builder.Data.MapInfo;
            for (var i = 0; i < mapInfo.RandomUnitTableCount; i++)
            {
                yield return builder.GenerateGlobalDeclaration(
                    builder.GetTypeName(BuiltinType.Int32),
                    $"gg_rg_{mapInfo.GetUnitTable(i).Index.ToString("D3")}",
                    true);
            }
        }
    }
}
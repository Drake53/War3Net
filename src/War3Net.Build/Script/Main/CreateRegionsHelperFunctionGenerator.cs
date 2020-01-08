// ------------------------------------------------------------------------------
// <copyright file="CreateRegionsHelperFunctionGenerator.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace War3Net.Build.Script.Main
{
    internal static partial class MainFunctionGenerator<TBuilder, TGlobalDeclarationSyntax, TFunctionSyntax, TStatementSyntax, TExpressionSyntax>
    {
        private static TFunctionSyntax GenerateCreateRegionsHelperFunction(TBuilder builder)
        {
            return builder.Build("CreateRegions", GetCreateRegionsHelperFunctionStatements(builder));
        }

        private static IEnumerable<TStatementSyntax> GetCreateRegionsHelperFunctionStatements(TBuilder builder)
        {
            // TODO: assign rects to global gg_rct_ (or local) variable, so can perform multiple operations on them (ambient sound)
            foreach (var region in builder.Data.MapRegions.Where(region => region.WeatherId != "\0\0\0\0"))
            {
                yield return builder.GenerateInvocationStatement(
                    nameof(War3Api.Common.EnableWeatherEffect),
                    builder.GenerateInvocationExpression(
                        nameof(War3Api.Common.AddWeatherEffect),
                        builder.GenerateInvocationExpression(
                            nameof(War3Api.Common.Rect),
                            builder.GenerateFloatLiteralExpression(region.Left),
                            builder.GenerateFloatLiteralExpression(region.Bottom),
                            builder.GenerateFloatLiteralExpression(region.Right),
                            builder.GenerateFloatLiteralExpression(region.Top)),
                        builder.GenerateFourCCExpression(region.WeatherId)),
                    builder.GenerateBooleanLiteralExpression(true));
            }
        }
    }
}
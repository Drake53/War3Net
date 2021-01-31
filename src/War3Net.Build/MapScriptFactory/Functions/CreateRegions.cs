// ------------------------------------------------------------------------------
// <copyright file="CreateRegions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.Build.Environment;
using War3Net.Build.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

using static War3Api.Common;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public static partial class MapScriptFactory
    {
        public static JassFunctionDeclarationSyntax CreateRegions(MapRegions mapRegions)
        {
            var statements = new List<IStatementSyntax>();

            foreach (var region in mapRegions.Regions)
            {
                var regionName = region.GetVariableName();

                statements.Add(SyntaxFactory.SetStatement(
                    regionName,
                    SyntaxFactory.InvocationExpression(
                        nameof(Rect),
                        SyntaxFactory.LiteralExpression(region.Left, precision: 1),
                        SyntaxFactory.LiteralExpression(region.Bottom, precision: 1),
                        SyntaxFactory.LiteralExpression(region.Right, precision: 1),
                        SyntaxFactory.LiteralExpression(region.Top, precision: 1))));

                if (region.WeatherType != WeatherType.None)
                {
                    statements.Add(SyntaxFactory.CallStatement(
                        nameof(EnableWeatherEffect),
                        SyntaxFactory.InvocationExpression(
                            nameof(AddWeatherEffect),
                            SyntaxFactory.VariableReferenceExpression(regionName),
                            SyntaxFactory.FourCCLiteralExpression((int)region.WeatherType)),
                        SyntaxFactory.LiteralExpression(true)));
                }

                if (!string.IsNullOrEmpty(region.AmbientSound))
                {
                    statements.Add(SyntaxFactory.CallStatement(
                        nameof(SetSoundPosition),
                        SyntaxFactory.VariableReferenceExpression(region.AmbientSound),
                        SyntaxFactory.LiteralExpression(region.CenterX),
                        SyntaxFactory.LiteralExpression(region.CenterY),
                        SyntaxFactory.LiteralExpression(0f)));

                    statements.Add(SyntaxFactory.CallStatement(
                        nameof(RegisterStackedSound),
                        SyntaxFactory.VariableReferenceExpression(region.AmbientSound),
                        SyntaxFactory.LiteralExpression(true),
                        SyntaxFactory.LiteralExpression(region.Width),
                        SyntaxFactory.LiteralExpression(region.Height)));
                }
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(CreateRegions)), statements);
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="CreateRegions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using War3Net.Build.Common;
using War3Net.Build.Environment;
using War3Net.Build.Extensions;
using War3Net.Build.Info;
using War3Net.CodeAnalysis.Jass.Syntax;

using SyntaxFactory = War3Net.CodeAnalysis.Jass.JassSyntaxFactory;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual JassFunctionDeclarationSyntax CreateRegions(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            var mapRegions = map.Regions;
            if (mapRegions is null)
            {
                throw new ArgumentException($"Function '{nameof(CreateRegions)}' cannot be generated without {nameof(MapRegions)}.", nameof(map));
            }

            var statements = new List<JassStatementSyntax>();

            if (UseWeatherEffectVariable)
            {
                statements.Add(SyntaxFactory.LocalVariableDeclarationStatement(SyntaxFactory.ParseTypeName(TypeName.WeatherEffect), VariableName.WeatherEffect));
                //statements.Add(JassEmptySyntax.Value);
            }

            foreach (var region in mapRegions.Regions)
            {
                var regionName = region.GetVariableName();

                statements.Add(SyntaxFactory.SetStatement(
                    regionName,
                    SyntaxFactory.InvocationExpression(
                        NativeName.Rect,
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(region.Left, precision: 1)),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(region.Bottom, precision: 1)),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(region.Right, precision: 1)),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(region.Top, precision: 1)))));

                if (region.WeatherType != WeatherType.None)
                {
                    if (UseWeatherEffectVariable)
                    {
                        statements.Add(SyntaxFactory.SetStatement(
                            VariableName.WeatherEffect,
                            SyntaxFactory.InvocationExpression(
                                NativeName.AddWeatherEffect,
                                SyntaxFactory.ParseIdentifierName(regionName),
                                SyntaxFactory.LiteralExpression(SyntaxFactory.FourCCLiteral((int)region.WeatherType)))));

                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.EnableWeatherEffect,
                            SyntaxFactory.ParseIdentifierName(VariableName.WeatherEffect),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(true))));
                    }
                    else
                    {
                        statements.Add(SyntaxFactory.CallStatement(
                            NativeName.EnableWeatherEffect,
                            SyntaxFactory.InvocationExpression(
                                NativeName.AddWeatherEffect,
                                SyntaxFactory.ParseIdentifierName(regionName),
                                SyntaxFactory.LiteralExpression(SyntaxFactory.FourCCLiteral((int)region.WeatherType))),
                            SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(true))));
                    }
                }

                if (!string.IsNullOrEmpty(region.AmbientSound))
                {
                    statements.Add(SyntaxFactory.CallStatement(
                        NativeName.SetSoundPosition,
                        SyntaxFactory.ParseIdentifierName(region.AmbientSound),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(region.CenterX)),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(region.CenterY)),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(0f))));

                    statements.Add(SyntaxFactory.CallStatement(
                        NativeName.RegisterStackedSound,
                        SyntaxFactory.ParseIdentifierName(region.AmbientSound),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(true)),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(region.Width)),
                        SyntaxFactory.LiteralExpression(SyntaxFactory.Literal(region.Height))));
                }
            }

            return SyntaxFactory.FunctionDeclaration(SyntaxFactory.FunctionDeclarator(nameof(CreateRegions)), statements);
        }

        protected internal virtual bool CreateRegionsCondition(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (map.Info is not null && map.Info.FormatVersion == MapInfoFormatVersion.v8)
            {
                return true;
            }

            return map.Regions is not null
                && map.Regions.Regions.Any();
        }
    }
}
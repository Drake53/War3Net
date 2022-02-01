// ------------------------------------------------------------------------------
// <copyright file="MapRegionsDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using War3Net.Build;
using War3Net.Build.Environment;
using War3Net.CodeAnalysis.Decompilers.Extensions;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        public bool TryDecompileMapRegions(MapRegionsFormatVersion formatVersion, [NotNullWhen(true)] out MapRegions? mapRegions)
        {
            foreach (var candidateFunction in GetCandidateFunctions("CreateRegions"))
            {
                if (TryDecompileMapRegions(candidateFunction.FunctionDeclaration, formatVersion, out mapRegions))
                {
                    candidateFunction.Handled = true;

                    return true;
                }
            }

            mapRegions = null;
            return false;
        }

        public bool TryDecompileMapRegions(JassFunctionDeclarationSyntax functionDeclaration, MapRegionsFormatVersion formatVersion, [NotNullWhen(true)] out MapRegions? mapRegions)
        {
            if (functionDeclaration is null)
            {
                throw new ArgumentNullException(nameof(functionDeclaration));
            }

            Region? currentRegion = null;
            var createdRegions = new List<Region>();
            var regions = new Dictionary<string, Region>(StringComparer.Ordinal);

            foreach (var statement in functionDeclaration.Body.Statements)
            {
                if (statement is JassLocalVariableDeclarationStatementSyntax ||
                    statement is JassEmptySyntax)
                {
                    continue;
                }
                else if (statement is JassSetStatementSyntax setStatement)
                {
                    if (setStatement.Indexer is null &&
                        setStatement.Value.Expression is JassInvocationExpressionSyntax invocationExpression)
                    {
                        if (setStatement.IdentifierName.Name.StartsWith("gg_rct_", StringComparison.Ordinal) &&
                            string.Equals(invocationExpression.IdentifierName.Name, "Rect", StringComparison.Ordinal))
                        {
                            if (invocationExpression.Arguments.Arguments.Length == 4 &&
                                invocationExpression.Arguments.Arguments[0].TryGetRealExpressionValue(out var minx) &&
                                invocationExpression.Arguments.Arguments[1].TryGetRealExpressionValue(out var miny) &&
                                invocationExpression.Arguments.Arguments[2].TryGetRealExpressionValue(out var maxx) &&
                                invocationExpression.Arguments.Arguments[3].TryGetRealExpressionValue(out var maxy))
                            {
                                currentRegion = new Region
                                {
                                    Name = setStatement.IdentifierName.Name["gg_rct_".Length..].Replace('_', ' '),
                                    Left = minx,
                                    Bottom = miny,
                                    Right = maxx,
                                    Top = maxy,
                                    Color = System.Drawing.Color.FromArgb(unchecked((int)0xFF8080FF)),
                                    CreationNumber = regions.Count,
                                    AmbientSound = string.Empty,
                                };

                                createdRegions.Add(currentRegion);

                                if (!regions.TryAdd(setStatement.IdentifierName.Name, currentRegion))
                                {
                                    regions[setStatement.IdentifierName.Name] = currentRegion;
                                }
                            }
                            else
                            {
                                mapRegions = null;
                                return false;
                            }
                        }
                        else if (string.Equals(setStatement.IdentifierName.Name, "we", StringComparison.Ordinal) &&
                                 string.Equals(invocationExpression.IdentifierName.Name, "AddWeatherEffect", StringComparison.Ordinal))
                        {
                            if (invocationExpression.Arguments.Arguments.Length == 2 &&
                                invocationExpression.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax regionVariableReferenceExpression &&
                                invocationExpression.Arguments.Arguments[1] is JassFourCCLiteralExpressionSyntax fourCCLiteralExpression &&
                                regions.TryGetValue(regionVariableReferenceExpression.IdentifierName.Name, out var region))
                            {
                                region.WeatherType = (WeatherType)fourCCLiteralExpression.Value.InvertEndianness();
                            }
                            else
                            {
                                mapRegions = null;
                                return false;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (statement is JassCallStatementSyntax callStatement)
                {
                    if (string.Equals(callStatement.IdentifierName.Name, "SetSoundPosition", StringComparison.Ordinal))
                    {
                        if (callStatement.Arguments.Arguments.Length == 4 &&
                            callStatement.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax soundVariableReferenceExpression &&
                            callStatement.Arguments.Arguments[1].TryGetRealExpressionValue(out var x) &&
                            callStatement.Arguments.Arguments[2].TryGetRealExpressionValue(out var y) &&
                            currentRegion is not null &&
                            currentRegion.CenterX == x &&
                            currentRegion.CenterY == y &&
                            (string.IsNullOrEmpty(currentRegion.AmbientSound) ||
                             string.Equals(currentRegion.AmbientSound, soundVariableReferenceExpression.IdentifierName.Name, StringComparison.Ordinal)))
                        {
                            currentRegion.AmbientSound = soundVariableReferenceExpression.IdentifierName.Name;
                        }
                        else
                        {
                            mapRegions = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Name, "RegisterStackedSound", StringComparison.Ordinal))
                    {
                        if (callStatement.Arguments.Arguments.Length == 4 &&
                            callStatement.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax soundVariableReferenceExpression &&
                            callStatement.Arguments.Arguments[2].TryGetRealExpressionValue(out var rectWidth) &&
                            callStatement.Arguments.Arguments[3].TryGetRealExpressionValue(out var rectHeight) &&
                            currentRegion is not null &&
                            currentRegion.Width == rectWidth &&
                            currentRegion.Height == rectHeight &&
                            (string.IsNullOrEmpty(currentRegion.AmbientSound) ||
                             string.Equals(currentRegion.AmbientSound, soundVariableReferenceExpression.IdentifierName.Name, StringComparison.Ordinal)))
                        {
                            currentRegion.AmbientSound = soundVariableReferenceExpression.IdentifierName.Name;
                        }
                        else
                        {
                            mapRegions = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Name, "EnableWeatherEffect", StringComparison.Ordinal))
                    {
                        continue;
                    }
                    else
                    {
                        mapRegions = null;
                        return false;
                    }
                }
                else
                {
                    mapRegions = null;
                    return false;
                }
            }

            if (regions.Any())
            {
                mapRegions = new MapRegions(formatVersion);
                mapRegions.Regions.AddRange(createdRegions);
                return true;
            }

            mapRegions = null;
            return false;
        }
    }
}
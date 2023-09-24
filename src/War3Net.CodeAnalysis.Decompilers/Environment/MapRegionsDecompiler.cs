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

using War3Net.Build.Common;
using War3Net.Build.Environment;
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

            foreach (var statement in functionDeclaration.Statements)
            {
                if (statement is JassLocalVariableDeclarationStatementSyntax)
                {
                    continue;
                }
                else if (statement is JassSetStatementSyntax setStatement)
                {
                    if (setStatement.ElementAccessClause is null &&
                        setStatement.Value.Expression is JassInvocationExpressionSyntax invocationExpression)
                    {
                        if (setStatement.IdentifierName.Token.Text.StartsWith("gg_rct_", StringComparison.Ordinal) &&
                            string.Equals(invocationExpression.IdentifierName.Token.Text, "Rect", StringComparison.Ordinal))
                        {
                            if (invocationExpression.ArgumentList.ArgumentList.Items.Length == 4 &&
                                invocationExpression.ArgumentList.ArgumentList.Items[0].TryGetRealExpressionValue(out var minx) &&
                                invocationExpression.ArgumentList.ArgumentList.Items[1].TryGetRealExpressionValue(out var miny) &&
                                invocationExpression.ArgumentList.ArgumentList.Items[2].TryGetRealExpressionValue(out var maxx) &&
                                invocationExpression.ArgumentList.ArgumentList.Items[3].TryGetRealExpressionValue(out var maxy))
                            {
                                currentRegion = new Region
                                {
                                    Name = setStatement.IdentifierName.Token.Text["gg_rct_".Length..].Replace('_', ' '),
                                    Left = minx,
                                    Bottom = miny,
                                    Right = maxx,
                                    Top = maxy,
                                    Color = System.Drawing.Color.FromArgb(unchecked((int)0xFF8080FF)),
                                    CreationNumber = regions.Count,
                                    AmbientSound = string.Empty,
                                };

                                createdRegions.Add(currentRegion);

                                if (!regions.TryAdd(setStatement.IdentifierName.Token.Text, currentRegion))
                                {
                                    regions[setStatement.IdentifierName.Token.Text] = currentRegion;
                                }
                            }
                            else
                            {
                                mapRegions = null;
                                return false;
                            }
                        }
                        else if (string.Equals(setStatement.IdentifierName.Token.Text, "we", StringComparison.Ordinal) &&
                                 string.Equals(invocationExpression.IdentifierName.Token.Text, "AddWeatherEffect", StringComparison.Ordinal))
                        {
                            if (invocationExpression.ArgumentList.ArgumentList.Items.Length == 2 &&
                                invocationExpression.ArgumentList.ArgumentList.Items[0].TryGetIdentifierNameValue(out var regionVariableReference) &&
                                invocationExpression.ArgumentList.ArgumentList.Items[1].TryGetIntegerExpressionValue(out var weatherType) &&
                                regions.TryGetValue(regionVariableReference, out var region))
                            {
                                region.WeatherType = (WeatherType)weatherType.InvertEndianness();
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
                    if (string.Equals(callStatement.IdentifierName.Token.Text, "SetSoundPosition", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.ArgumentList.Items.Length == 4 &&
                            callStatement.ArgumentList.ArgumentList.Items[0].TryGetIdentifierNameValue(out var soundVariableReference) &&
                            callStatement.ArgumentList.ArgumentList.Items[1].TryGetRealExpressionValue(out var x) &&
                            callStatement.ArgumentList.ArgumentList.Items[2].TryGetRealExpressionValue(out var y) &&
                            currentRegion is not null &&
                            currentRegion.CenterX == x &&
                            currentRegion.CenterY == y &&
                            (string.IsNullOrEmpty(currentRegion.AmbientSound) ||
                             string.Equals(currentRegion.AmbientSound, soundVariableReference, StringComparison.Ordinal)))
                        {
                            currentRegion.AmbientSound = soundVariableReference;
                        }
                        else
                        {
                            mapRegions = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "RegisterStackedSound", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.ArgumentList.Items.Length == 4 &&
                            callStatement.ArgumentList.ArgumentList.Items[0].TryGetIdentifierNameValue(out var soundVariableReference) &&
                            callStatement.ArgumentList.ArgumentList.Items[2].TryGetRealExpressionValue(out var rectWidth) &&
                            callStatement.ArgumentList.ArgumentList.Items[3].TryGetRealExpressionValue(out var rectHeight) &&
                            currentRegion is not null &&
                            currentRegion.Width == rectWidth &&
                            currentRegion.Height == rectHeight &&
                            (string.IsNullOrEmpty(currentRegion.AmbientSound) ||
                             string.Equals(currentRegion.AmbientSound, soundVariableReference, StringComparison.Ordinal)))
                        {
                            currentRegion.AmbientSound = soundVariableReference;
                        }
                        else
                        {
                            mapRegions = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "EnableWeatherEffect", StringComparison.Ordinal))
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
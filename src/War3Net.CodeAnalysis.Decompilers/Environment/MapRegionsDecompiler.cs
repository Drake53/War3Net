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
                        setStatement.EqualsValueClause.Value is JassInvocationExpressionSyntax invocationExpression)
                    {
                        if (setStatement.IdentifierName.Token.Text.StartsWith("gg_rct_", StringComparison.Ordinal) &&
                            string.Equals(invocationExpression.IdentifierName.Token.Text, "Rect", StringComparison.Ordinal))
                        {
                            if (invocationExpression.ArgumentList.Arguments.Items.Length == 4 &&
                                invocationExpression.ArgumentList.Arguments.Items[0].TryGetRealExpressionValue(out var minx) &&
                                invocationExpression.ArgumentList.Arguments.Items[1].TryGetRealExpressionValue(out var miny) &&
                                invocationExpression.ArgumentList.Arguments.Items[2].TryGetRealExpressionValue(out var maxx) &&
                                invocationExpression.ArgumentList.Arguments.Items[3].TryGetRealExpressionValue(out var maxy))
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
                            if (invocationExpression.ArgumentList.Arguments.Items.Length == 2 &&
                                invocationExpression.ArgumentList.Arguments.Items[0].TryGetIdentifierNameValue(out var regionVariableName) &&
                                invocationExpression.ArgumentList.Arguments.Items[1].TryGetIntegerExpressionValue(out var weatherTypeValue) &&
                                regions.TryGetValue(regionVariableName, out var region))
                            {
                                region.WeatherType = (WeatherType)weatherTypeValue.InvertEndianness();
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
                        if (callStatement.ArgumentList.Arguments.Items.Length == 4 &&
                            callStatement.ArgumentList.Arguments.Items[0].TryGetIdentifierNameValue(out var soundVariableName) &&
                            callStatement.ArgumentList.Arguments.Items[1].TryGetRealExpressionValue(out var x) &&
                            callStatement.ArgumentList.Arguments.Items[2].TryGetRealExpressionValue(out var y) &&
                            currentRegion is not null &&
                            currentRegion.CenterX == x &&
                            currentRegion.CenterY == y &&
                            (string.IsNullOrEmpty(currentRegion.AmbientSound) ||
                             string.Equals(currentRegion.AmbientSound, soundVariableName, StringComparison.Ordinal)))
                        {
                            currentRegion.AmbientSound = soundVariableName;
                        }
                        else
                        {
                            mapRegions = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "RegisterStackedSound", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.Arguments.Items.Length == 4 &&
                            callStatement.ArgumentList.Arguments.Items[0].TryGetIdentifierNameValue(out var soundVariableName) &&
                            callStatement.ArgumentList.Arguments.Items[2].TryGetRealExpressionValue(out var rectWidth) &&
                            callStatement.ArgumentList.Arguments.Items[3].TryGetRealExpressionValue(out var rectHeight) &&
                            currentRegion is not null &&
                            currentRegion.Width == rectWidth &&
                            currentRegion.Height == rectHeight &&
                            (string.IsNullOrEmpty(currentRegion.AmbientSound) ||
                             string.Equals(currentRegion.AmbientSound, soundVariableName, StringComparison.Ordinal)))
                        {
                            currentRegion.AmbientSound = soundVariableName;
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

            if (regions.Count > 0)
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
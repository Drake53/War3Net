﻿// ------------------------------------------------------------------------------
// <copyright file="MapUnitsDecompiler.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

using War3Net.Build.Widget;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.Common.Extensions;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        public bool TryDecompileMapUnits(
            MapWidgetsFormatVersion formatVersion,
            MapWidgetsSubVersion subVersion,
            bool useNewFormat,
            [NotNullWhen(true)] out MapUnits? mapUnits)
        {
            var createAllUnits = GetFunction("CreateAllUnits");
            var config = GetFunction("config");
            var initCustomPlayerSlots = GetFunction("InitCustomPlayerSlots");

            if (createAllUnits is null ||
                config is null ||
                initCustomPlayerSlots is null)
            {
                mapUnits = null;
                return false;
            }

            if (TryDecompileMapUnits(
                createAllUnits.FunctionDeclaration,
                config.FunctionDeclaration,
                initCustomPlayerSlots.FunctionDeclaration,
                formatVersion,
                subVersion,
                useNewFormat,
                out mapUnits))
            {
                createAllUnits.Handled = true;
                initCustomPlayerSlots.Handled = true;

                return true;
            }

            mapUnits = null;
            return false;
        }

        public bool TryDecompileMapUnits(
            JassFunctionDeclarationSyntax createAllUnitsFunction,
            JassFunctionDeclarationSyntax configFunction,
            JassFunctionDeclarationSyntax initCustomPlayerSlotsFunction,
            MapWidgetsFormatVersion formatVersion,
            MapWidgetsSubVersion subVersion,
            bool useNewFormat,
            [NotNullWhen(true)] out MapUnits? mapUnits)
        {
            if (createAllUnitsFunction is null)
            {
                throw new ArgumentNullException(nameof(createAllUnitsFunction));
            }

            if (configFunction is null)
            {
                throw new ArgumentNullException(nameof(configFunction));
            }

            if (initCustomPlayerSlotsFunction is null)
            {
                throw new ArgumentNullException(nameof(initCustomPlayerSlotsFunction));
            }

            if (TryDecompileCreateUnitsFunction(createAllUnitsFunction, out var units) &&
                TryDecompileStartLocationPositionsConfigFunction(configFunction, out var startLocationPositions) &&
                TryDecompileInitCustomPlayerSlotsFunction(initCustomPlayerSlotsFunction, startLocationPositions, out var startLocations))
            {
                mapUnits = new MapUnits(formatVersion, subVersion, useNewFormat);

                mapUnits.Units.AddRange(units);
                mapUnits.Units.AddRange(startLocations);

                return true;
            }

            mapUnits = null;
            return false;
        }

        private bool TryDecompileCreateUnitsFunction(JassFunctionDeclarationSyntax createUnitsFunction, [NotNullWhen(true)] out List<UnitData>? units)
        {
            var localPlayerVariableName = (string?)null;
            var localPlayerVariableValue = (int?)null;

            var result = new List<UnitData>();

            foreach (var statement in createUnitsFunction.Statements)
            {
                if (statement is JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement)
                {
                    var typeName = localVariableDeclarationStatement.Declarator.GetVariableType().GetToken().Text;

                    if (string.Equals(typeName, "player", StringComparison.Ordinal))
                    {
                        if (localVariableDeclarationStatement.Declarator is JassVariableDeclaratorSyntax variableDeclarator &&
                            variableDeclarator.Value is not null &&
                            variableDeclarator.Value.Expression is JassInvocationExpressionSyntax playerInvocationExpression &&
                            string.Equals(playerInvocationExpression.IdentifierName.Token.Text, "Player", StringComparison.Ordinal) &&
                            playerInvocationExpression.ArgumentList.ArgumentList.Items.Length == 1 &&
                            playerInvocationExpression.ArgumentList.ArgumentList.Items[0].TryGetPlayerIdExpressionValue(Context.MaxPlayerSlots, out var playerId))
                        {
                            localPlayerVariableName = variableDeclarator.IdentifierName.Token.Text;
                            localPlayerVariableValue = playerId;
                        }
                        else
                        {
                            units = null;
                            return false;
                        }
                    }
                    else if (string.Equals(typeName, "unit", StringComparison.Ordinal) ||
                             string.Equals(typeName, "integer", StringComparison.Ordinal) ||
                             string.Equals(typeName, "trigger", StringComparison.Ordinal) ||
                             string.Equals(typeName, "real", StringComparison.Ordinal))
                    {
                        // TODO

                        if (localVariableDeclarationStatement.Declarator is not JassVariableDeclaratorSyntax variableDeclarator ||
                            variableDeclarator.Value is not null)
                        {
                            units = null;
                            return false;
                        }
                    }
                    else
                    {
                        units = null;
                        return false;
                    }
                }
                else if (statement is JassSetStatementSyntax setStatement)
                {
                    if (setStatement.ElementAccessClause is null)
                    {
                        if (setStatement.Value.Expression is JassInvocationExpressionSyntax invocationExpression)
                        {
                            if (string.Equals(invocationExpression.IdentifierName.Token.Text, "CreateUnit", StringComparison.Ordinal))
                            {
                                if (invocationExpression.ArgumentList.ArgumentList.Items.Length == 5 &&
                                    invocationExpression.ArgumentList.ArgumentList.Items[0].TryGetIdentifierNameValue(out var playerVariableName) &&
                                    invocationExpression.ArgumentList.ArgumentList.Items[1].TryGetIntegerExpressionValue(out var unitId) &&
                                    invocationExpression.ArgumentList.ArgumentList.Items[2].TryGetRealExpressionValue(out var x) &&
                                    invocationExpression.ArgumentList.ArgumentList.Items[3].TryGetRealExpressionValue(out var y) &&
                                    invocationExpression.ArgumentList.ArgumentList.Items[4].TryGetRealExpressionValue(out var face) &&
                                    string.Equals(playerVariableName, localPlayerVariableName, StringComparison.Ordinal))
                                {
                                    var unit = new UnitData
                                    {
                                        OwnerId = localPlayerVariableValue.Value,
                                        TypeId = unitId.InvertEndianness(),
                                        Position = new Vector3(x, y, 0f),
                                        Rotation = face * (MathF.PI / 180f),
                                        Scale = Vector3.One,
                                        Flags = 2,
                                        GoldAmount = 12500,
                                        HeroLevel = 1,
                                    };

                                    unit.SkinId = unit.TypeId;

                                    result.Add(unit);
                                }
                                else
                                {
                                    units = null;
                                    return false;
                                }
                            }
                            else if (string.Equals(invocationExpression.IdentifierName.Token.Text, "BlzCreateUnitWithSkin", StringComparison.Ordinal))
                            {
                                if (invocationExpression.ArgumentList.ArgumentList.Items.Length == 6 &&
                                    invocationExpression.ArgumentList.ArgumentList.Items[0].TryGetIdentifierNameValue(out var playerVariableName) &&
                                    invocationExpression.ArgumentList.ArgumentList.Items[1].TryGetIntegerExpressionValue(out var unitId) &&
                                    invocationExpression.ArgumentList.ArgumentList.Items[2].TryGetRealExpressionValue(out var x) &&
                                    invocationExpression.ArgumentList.ArgumentList.Items[3].TryGetRealExpressionValue(out var y) &&
                                    invocationExpression.ArgumentList.ArgumentList.Items[4].TryGetRealExpressionValue(out var face) &&
                                    invocationExpression.ArgumentList.ArgumentList.Items[5].TryGetIntegerExpressionValue(out var skinId) &&
                                    string.Equals(playerVariableName, localPlayerVariableName, StringComparison.Ordinal))
                                {
                                    var unit = new UnitData
                                    {
                                        OwnerId = localPlayerVariableValue.Value,
                                        TypeId = unitId.InvertEndianness(),
                                        Position = new Vector3(x, y, 0f),
                                        Rotation = face * (MathF.PI / 180f),
                                        Scale = Vector3.One,
                                        SkinId = skinId.InvertEndianness(),
                                        Flags = 2,
                                        GoldAmount = 12500,
                                        HeroLevel = 1,
                                    };

                                    result.Add(unit);
                                }
                                else
                                {
                                    units = null;
                                    return false;
                                }
                            }
                            else if (string.Equals(invocationExpression.IdentifierName.Token.Text, "CreateTrigger", StringComparison.Ordinal))
                            {
                                // TODO
                                continue;
                            }
                            else if (string.Equals(invocationExpression.IdentifierName.Token.Text, "GetUnitState", StringComparison.Ordinal))
                            {
                                // TODO
                                continue;
                            }
                            else if (string.Equals(invocationExpression.IdentifierName.Token.Text, "RandomDistChoose", StringComparison.Ordinal))
                            {
                                // TODO
                                continue;
                            }
                            else
                            {
                                units = null;
                                return false;
                            }
                        }
                        else if (setStatement.Value.Expression is JassElementAccessExpressionSyntax)
                        {
                            // TODO
                            continue;
                        }
                        else
                        {
                            units = null;
                            return false;
                        }
                    }
                    else
                    {
                        units = null;
                        return false;
                    }
                }
                else if (statement is JassCallStatementSyntax callStatement)
                {
                    if (string.Equals(callStatement.IdentifierName.Token.Text, "SetResourceAmount", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.ArgumentList.Items.Length == 2 &&
                            callStatement.ArgumentList.ArgumentList.Items[0].TryGetIdentifierNameValue(out var unitVariableName) &&
                            callStatement.ArgumentList.ArgumentList.Items[1].TryGetIntegerExpressionValue(out var amount))
                        {
                            result[^1].GoldAmount = amount;
                        }
                        else
                        {
                            units = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "SetUnitColor", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.ArgumentList.Items.Length == 2 &&
                            callStatement.ArgumentList.ArgumentList.Items[0].TryGetIdentifierNameValue(out var unitVariableName) &&
                            callStatement.ArgumentList.ArgumentList.Items[1] is JassInvocationExpressionSyntax convertPlayerColorInvocationExpression &&
                            string.Equals(convertPlayerColorInvocationExpression.IdentifierName.Token.Text, "ConvertPlayerColor", StringComparison.Ordinal) &&
                            convertPlayerColorInvocationExpression.ArgumentList.ArgumentList.Items.Length == 1 &&
                            convertPlayerColorInvocationExpression.ArgumentList.ArgumentList.Items[0].TryGetIntegerExpressionValue(out var playerColorId))
                        {
                            result[^1].CustomPlayerColorId = playerColorId;
                        }
                        else
                        {
                            units = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "SetUnitAcquireRange", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.ArgumentList.Items.Length == 2 &&
                            callStatement.ArgumentList.ArgumentList.Items[0].TryGetIdentifierNameValue(out var unitVariableReference) &&
                            callStatement.ArgumentList.ArgumentList.Items[1].TryGetRealExpressionValue(out var acquireRange))
                        {
                            const float CampAcquireRange = 200f;
                            result[^1].TargetAcquisition = acquireRange == CampAcquireRange ? -2f : acquireRange;
                        }
                        else
                        {
                            units = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "SetUnitState", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.ArgumentList.Items.Length == 3 &&
                            callStatement.ArgumentList.ArgumentList.Items[0].TryGetIdentifierNameValue(out var unitVariableReference) &&
                            callStatement.ArgumentList.ArgumentList.Items[1].TryGetIdentifierNameValue(out var unitStateName))
                        {
                            if (string.Equals(unitStateName, "UNIT_STATE_LIFE", StringComparison.Ordinal))
                            {
                                if (callStatement.ArgumentList.ArgumentList.Items[2] is JassBinaryExpressionSyntax binaryExpression &&
                                    binaryExpression.Left.TryGetRealExpressionValue(out var hp) &&
                                    binaryExpression.OperatorToken.SyntaxKind == Jass.JassSyntaxKind.AsteriskToken &&
                                    binaryExpression.Right is JassIdentifierNameSyntax)
                                {
                                    result[^1].HP = (int)(100 * hp);
                                }
                                else
                                {
                                    units = null;
                                    return false;
                                }
                            }
                            else if (string.Equals(unitStateName, "UNIT_STATE_MANA", StringComparison.Ordinal))
                            {
                                if (callStatement.ArgumentList.ArgumentList.Items[2].TryGetIntegerExpressionValue(out var mp))
                                {
                                    result[^1].MP = mp;
                                }
                                else
                                {
                                    units = null;
                                    return false;
                                }
                            }
                            else
                            {
                                units = null;
                                return false;
                            }
                        }
                        else
                        {
                            units = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "UnitAddItemToSlotById", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.ArgumentList.Items.Length == 3 &&
                            callStatement.ArgumentList.ArgumentList.Items[0].TryGetIdentifierNameValue(out _) &&
                            callStatement.ArgumentList.ArgumentList.Items[1].TryGetIntegerExpressionValue(out var itemId) &&
                            callStatement.ArgumentList.ArgumentList.Items[2].TryGetIntegerExpressionValue(out var slot))
                        {
                            result[^1].InventoryData.Add(new InventoryItemData
                            {
                                ItemId = itemId,
                                Slot = slot,
                            });
                        }
                        else
                        {
                            units = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "SetHeroLevel", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.ArgumentList.Items.Length == 3 &&
                            callStatement.ArgumentList.ArgumentList.Items[0].TryGetIdentifierNameValue(out var unitVariableName) &&
                            callStatement.ArgumentList.ArgumentList.Items[1].TryGetIntegerExpressionValue(out var level) &&
                            callStatement.ArgumentList.ArgumentList.Items[2].TryGetBooleanExpressionValue(out _))
                        {
                            result[^1].HeroLevel = level;
                        }
                        else
                        {
                            units = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "SetHeroStr", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.ArgumentList.Items.Length == 3 &&
                            callStatement.ArgumentList.ArgumentList.Items[0].TryGetIdentifierNameValue(out var unitVariableName) &&
                            callStatement.ArgumentList.ArgumentList.Items[1].TryGetIntegerExpressionValue(out var value) &&
                            callStatement.ArgumentList.ArgumentList.Items[2].TryGetBooleanExpressionValue(out _))
                        {
                            result[^1].HeroStrength = value;
                        }
                        else
                        {
                            units = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "SetHeroAgi", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.ArgumentList.Items.Length == 3 &&
                            callStatement.ArgumentList.ArgumentList.Items[0].TryGetIdentifierNameValue(out var unitVariableName) &&
                            callStatement.ArgumentList.ArgumentList.Items[1].TryGetIntegerExpressionValue(out var value) &&
                            callStatement.ArgumentList.ArgumentList.Items[2].TryGetBooleanExpressionValue(out _))
                        {
                            result[^1].HeroAgility = value;
                        }
                        else
                        {
                            units = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "SetHeroInt", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.ArgumentList.Items.Length == 3 &&
                            callStatement.ArgumentList.ArgumentList.Items[0].TryGetIdentifierNameValue(out var unitVariableName) &&
                            callStatement.ArgumentList.ArgumentList.Items[1].TryGetIntegerExpressionValue(out var value) &&
                            callStatement.ArgumentList.ArgumentList.Items[2].TryGetBooleanExpressionValue(out _))
                        {
                            result[^1].HeroIntelligence = value;
                        }
                        else
                        {
                            units = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "SelectHeroSkill", StringComparison.Ordinal))
                    {
                        // TODO
                        continue;
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "IssueImmediateOrder", StringComparison.Ordinal))
                    {
                        // TODO
                        continue;
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "RandomDistReset", StringComparison.Ordinal))
                    {
                        // TODO
                        continue;
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "RandomDistAddItem", StringComparison.Ordinal))
                    {
                        // TODO
                        continue;
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "TriggerRegisterUnitEvent", StringComparison.Ordinal))
                    {
                        // TODO
                        continue;
                    }
                    else if (string.Equals(callStatement.IdentifierName.Token.Text, "TriggerAddAction", StringComparison.Ordinal))
                    {
                        // TODO
                        continue;
                    }
                    else if (callStatement.ArgumentList.ArgumentList.Items.IsEmpty)
                    {
                        if (Context.FunctionDeclarations.TryGetValue(callStatement.IdentifierName.Token.Text, out var subFunction) &&
                            TryDecompileCreateUnitsFunction(subFunction.FunctionDeclaration, out var subFunctionResult))
                        {
                            result.AddRange(subFunctionResult);
                        }
                        else
                        {
                            units = null;
                            return false;
                        }
                    }
                    else
                    {
                        units = null;
                        return false;
                    }
                }
                else if (statement is JassIfStatementSyntax ifStatement)
                {
                    if (ifStatement.IfClause.IfClauseDeclarator.Condition.Deparenthesize() is JassBinaryExpressionSyntax binaryExpression &&
                        binaryExpression.Left.TryGetIdentifierNameValue(out _) &&
                        binaryExpression.OperatorToken.SyntaxKind == Jass.JassSyntaxKind.ExclamationEqualsToken &&
                        binaryExpression.Right.TryGetIntegerExpressionValue(out var value) &&
                        value == -1)
                    {
                        // TODO
                        continue;
                    }
                    else
                    {
                        units = null;
                        return false;
                    }
                }
                else
                {
                    units = null;
                    return false;
                }
            }

            units = result;
            return true;
        }

        private bool TryDecompileStartLocationPositionsConfigFunction(JassFunctionDeclarationSyntax configFunction, [NotNullWhen(true)] out Dictionary<int, Vector2>? startLocationPositions)
        {
            var result = new Dictionary<int, Vector2>();

            foreach (var statement in configFunction.Statements)
            {
                if (statement is JassCallStatementSyntax callStatement &&
                    string.Equals(callStatement.IdentifierName.Token.Text, "DefineStartLocation", StringComparison.Ordinal))
                {
                    if (callStatement.ArgumentList.ArgumentList.Items.Length == 3 &&
                        callStatement.ArgumentList.ArgumentList.Items[0].TryGetIntegerExpressionValue(out var index) &&
                        callStatement.ArgumentList.ArgumentList.Items[1].TryGetRealExpressionValue(out var x) &&
                        callStatement.ArgumentList.ArgumentList.Items[2].TryGetRealExpressionValue(out var y))
                    {
                        result.Add(index, new Vector2(x, y));
                    }
                    else
                    {
                        startLocationPositions = null;
                        return false;
                    }
                }
                else
                {
                    continue;
                }
            }

            startLocationPositions = result;
            return true;
        }

        private bool TryDecompileInitCustomPlayerSlotsFunction(
            JassFunctionDeclarationSyntax initCustomPlayerSlotsFunction,
            Dictionary<int, Vector2> startLocationPositions,
            [NotNullWhen(true)] out List<UnitData>? startLocations)
        {
            var result = new List<UnitData>();

            foreach (var statement in initCustomPlayerSlotsFunction.Statements)
            {
                if (statement is JassCallStatementSyntax callStatement)
                {
                    if (string.Equals(callStatement.IdentifierName.Token.Text, "SetPlayerStartLocation", StringComparison.Ordinal))
                    {
                        if (callStatement.ArgumentList.ArgumentList.Items.Length == 2 &&
                            callStatement.ArgumentList.ArgumentList.Items[0] is JassInvocationExpressionSyntax playerInvocationExpression &&
                            string.Equals(playerInvocationExpression.IdentifierName.Token.Text, "Player", StringComparison.Ordinal) &&
                            playerInvocationExpression.ArgumentList.ArgumentList.Items.Length == 1 &&
                            playerInvocationExpression.ArgumentList.ArgumentList.Items[0].TryGetPlayerIdExpressionValue(Context.MaxPlayerSlots, out var playerId) &&
                            callStatement.ArgumentList.ArgumentList.Items[1].TryGetIntegerExpressionValue(out var startLocationNumber) &&
                            startLocationPositions.TryGetValue(startLocationNumber, out var startLocationPosition))
                        {
                            var unit = new UnitData
                            {
                                OwnerId = playerId,
                                TypeId = "sloc".FromRawcode(),
                                Position = new Vector3(startLocationPosition, 0f),
                                Rotation = MathF.PI * 1.5f,
                                Scale = Vector3.One,
                                Flags = 2,
                                GoldAmount = 12500,
                                HeroLevel = 0,
                                TargetAcquisition = 0,
                            };

                            unit.SkinId = unit.TypeId;

                            result.Add(unit);
                        }
                        else
                        {
                            startLocations = null;
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
                    startLocations = null;
                    return false;
                }
            }

            startLocations = result;
            return true;
        }
    }
}
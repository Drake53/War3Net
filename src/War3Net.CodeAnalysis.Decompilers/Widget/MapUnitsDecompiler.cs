// ------------------------------------------------------------------------------
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
        private int CreationNumber = 0;

        public bool TryDecompileMapUnits(
            MapWidgetsFormatVersion formatVersion,
            MapWidgetsSubVersion subVersion,
            bool useNewFormat,
            [NotNullWhen(true)] out MapUnits? mapUnits)
        {
            var createAllUnits = GetFunction("CreateAllUnits");
            var createAllItems = GetFunction("CreateAllItems");
            var config = GetFunction("config");
            var initCustomPlayerSlots = GetFunction("InitCustomPlayerSlots");

            if (createAllUnits is null ||
                createAllItems is null ||
                config is null ||
                initCustomPlayerSlots is null)
            {
                mapUnits = null;
                return false;
            }

            if (TryDecompileMapUnits(
                createAllUnits.FunctionDeclaration,
                createAllItems.FunctionDeclaration,
                config.FunctionDeclaration,
                initCustomPlayerSlots.FunctionDeclaration,
                formatVersion,
                subVersion,
                useNewFormat,
                out mapUnits))
            {
                createAllUnits.Handled = true;
                createAllItems.Handled = true;
                initCustomPlayerSlots.Handled = true;

                return true;
            }

            mapUnits = null;
            return false;
        }

        public bool TryDecompileMapUnits(
            JassFunctionDeclarationSyntax createAllUnitsFunction,
            JassFunctionDeclarationSyntax createAllItemsFunction,
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

            if (createAllItemsFunction is null)
            {
                throw new ArgumentNullException(nameof(createAllItemsFunction));
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
                TryDecompileCreateItemsFunction(createAllItemsFunction, out var items) &&
                TryDecompileStartLocationPositionsConfigFunction(configFunction, out var startLocationPositions) &&
                TryDecompileInitCustomPlayerSlotsFunction(initCustomPlayerSlotsFunction, startLocationPositions, out var startLocations))
            {
                mapUnits = new MapUnits(formatVersion, subVersion, useNewFormat);

                mapUnits.Units.AddRange(units);
                mapUnits.Units.AddRange(items);
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

            foreach (var statement in createUnitsFunction.Body.Statements)
            {
                if (statement is JassCommentSyntax ||
                    statement is JassEmptySyntax)
                {
                    continue;
                }
                else if (statement is JassLocalVariableDeclarationStatementSyntax localVariableDeclarationStatement)
                {
                    var typeName = localVariableDeclarationStatement.Declarator.Type.TypeName.Name;

                    if (string.Equals(typeName, "player", StringComparison.Ordinal))
                    {
                        if (localVariableDeclarationStatement.Declarator is JassVariableDeclaratorSyntax variableDeclarator &&
                            variableDeclarator.Value is not null &&
                            variableDeclarator.Value.Expression is JassInvocationExpressionSyntax playerInvocationExpression &&
                            string.Equals(playerInvocationExpression.IdentifierName.Name, "Player", StringComparison.Ordinal) &&
                            playerInvocationExpression.Arguments.Arguments.Length == 1 &&
                            playerInvocationExpression.Arguments.Arguments[0].TryGetPlayerIdExpressionValue(Context.MaxPlayerSlots, out var playerId))
                        {
                            localPlayerVariableName = variableDeclarator.IdentifierName.Name;
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
                    if (setStatement.Indexer is null)
                    {
                        if (setStatement.Value.Expression is JassInvocationExpressionSyntax invocationExpression)
                        {
                            if (string.Equals(invocationExpression.IdentifierName.Name, "CreateUnit", StringComparison.Ordinal))
                            {
                                if (invocationExpression.Arguments.Arguments.Length == 5 &&
                                    invocationExpression.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax playerVariableReferenceExpression &&
                                    invocationExpression.Arguments.Arguments[1].TryGetIntegerExpressionValue(out var unitId) &&
                                    invocationExpression.Arguments.Arguments[2].TryGetRealExpressionValue(out var x) &&
                                    invocationExpression.Arguments.Arguments[3].TryGetRealExpressionValue(out var y) &&
                                    invocationExpression.Arguments.Arguments[4].TryGetRealExpressionValue(out var face) &&
                                    string.Equals(playerVariableReferenceExpression.IdentifierName.Name, localPlayerVariableName, StringComparison.Ordinal))
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
                                        CreationNumber = CreationNumber++
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
                            else if (string.Equals(invocationExpression.IdentifierName.Name, "BlzCreateUnitWithSkin", StringComparison.Ordinal))
                            {
                                if (invocationExpression.Arguments.Arguments.Length == 6 &&
                                    invocationExpression.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax playerVariableReferenceExpression &&
                                    invocationExpression.Arguments.Arguments[1].TryGetIntegerExpressionValue(out var unitId) &&
                                    invocationExpression.Arguments.Arguments[2].TryGetRealExpressionValue(out var x) &&
                                    invocationExpression.Arguments.Arguments[3].TryGetRealExpressionValue(out var y) &&
                                    invocationExpression.Arguments.Arguments[4].TryGetRealExpressionValue(out var face) &&
                                    invocationExpression.Arguments.Arguments[5].TryGetIntegerExpressionValue(out var skinId) &&
                                    string.Equals(playerVariableReferenceExpression.IdentifierName.Name, localPlayerVariableName, StringComparison.Ordinal))
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
                                        CreationNumber = CreationNumber++
                                    };

                                    result.Add(unit);
                                }
                                else
                                {
                                    units = null;
                                    return false;
                                }
                            }
                            else if (string.Equals(invocationExpression.IdentifierName.Name, "CreateTrigger", StringComparison.Ordinal))
                            {
                                // TODO
                                continue;
                            }
                            else if (string.Equals(invocationExpression.IdentifierName.Name, "GetUnitState", StringComparison.Ordinal))
                            {
                                // TODO
                                continue;
                            }
                            else if (string.Equals(invocationExpression.IdentifierName.Name, "RandomDistChoose", StringComparison.Ordinal))
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
                        else if (setStatement.Value.Expression is JassArrayReferenceExpressionSyntax)
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
                    if (string.Equals(callStatement.IdentifierName.Name, "SetResourceAmount", StringComparison.Ordinal))
                    {
                        if (callStatement.Arguments.Arguments.Length == 2 &&
                            callStatement.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax unitVariableReferenceExpression &&
                            callStatement.Arguments.Arguments[1].TryGetIntegerExpressionValue(out var amount))
                        {
                            result[^1].GoldAmount = amount;
                        }
                        else
                        {
                            units = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Name, "SetUnitColor", StringComparison.Ordinal))
                    {
                        if (callStatement.Arguments.Arguments.Length == 2 &&
                            callStatement.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax unitVariableReferenceExpression &&
                            callStatement.Arguments.Arguments[1] is JassInvocationExpressionSyntax convertPlayerColorInvocationExpression &&
                            string.Equals(convertPlayerColorInvocationExpression.IdentifierName.Name, "ConvertPlayerColor", StringComparison.Ordinal) &&
                            convertPlayerColorInvocationExpression.Arguments.Arguments.Length == 1 &&
                            convertPlayerColorInvocationExpression.Arguments.Arguments[0].TryGetIntegerExpressionValue(out var playerColorId))
                        {
                            result[^1].CustomPlayerColorId = playerColorId;
                        }
                        else
                        {
                            units = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Name, "SetUnitAcquireRange", StringComparison.Ordinal))
                    {
                        if (callStatement.Arguments.Arguments.Length == 2 &&
                            callStatement.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax unitVariableReferenceExpression &&
                            callStatement.Arguments.Arguments[1].TryGetRealExpressionValue(out var acquireRange))
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
                    else if (string.Equals(callStatement.IdentifierName.Name, "SetUnitState", StringComparison.Ordinal))
                    {
                        if (callStatement.Arguments.Arguments.Length == 3 &&
                            callStatement.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax unitVariableReferenceExpression &&
                            callStatement.Arguments.Arguments[1] is JassVariableReferenceExpressionSyntax unitStateVariableReferenceExpression)
                        {
                            if (string.Equals(unitStateVariableReferenceExpression.IdentifierName.Name, "UNIT_STATE_LIFE", StringComparison.Ordinal))
                            {
                                if (callStatement.Arguments.Arguments[2] is JassBinaryExpressionSyntax binaryExpression &&
                                    binaryExpression.Left.TryGetRealExpressionValue(out var hp) &&
                                    binaryExpression.Operator == BinaryOperatorType.Multiplication &&
                                    binaryExpression.Right is JassVariableReferenceExpressionSyntax)
                                {
                                    result[^1].HP = (int)(100 * hp);
                                }
                                else
                                {
                                    units = null;
                                    return false;
                                }
                            }
                            else if (string.Equals(unitStateVariableReferenceExpression.IdentifierName.Name, "UNIT_STATE_MANA", StringComparison.Ordinal))
                            {
                                if (callStatement.Arguments.Arguments[2].TryGetIntegerExpressionValue(out var mp))
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
                    else if (string.Equals(callStatement.IdentifierName.Name, "UnitAddItemToSlotById", StringComparison.Ordinal))
                    {
                        if (callStatement.Arguments.Arguments.Length == 3 &&
                            callStatement.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax &&
                            callStatement.Arguments.Arguments[1].TryGetIntegerExpressionValue(out var itemId) &&
                            callStatement.Arguments.Arguments[2].TryGetIntegerExpressionValue(out var slot))
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
                    else if (string.Equals(callStatement.IdentifierName.Name, "SetHeroLevel", StringComparison.Ordinal))
                    {
                        if (callStatement.Arguments.Arguments.Length == 3 &&
                            callStatement.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax unitVariableReferenceExpression &&
                            callStatement.Arguments.Arguments[1].TryGetIntegerExpressionValue(out var level) &&
                            callStatement.Arguments.Arguments[2] is JassBooleanLiteralExpressionSyntax)
                        {
                            result[^1].HeroLevel = level;
                        }
                        else
                        {
                            units = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Name, "SetHeroStr", StringComparison.Ordinal))
                    {
                        if (callStatement.Arguments.Arguments.Length == 3 &&
                            callStatement.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax unitVariableReferenceExpression &&
                            callStatement.Arguments.Arguments[1].TryGetIntegerExpressionValue(out var value) &&
                            callStatement.Arguments.Arguments[2] is JassBooleanLiteralExpressionSyntax)
                        {
                            result[^1].HeroStrength = value;
                        }
                        else
                        {
                            units = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Name, "SetHeroAgi", StringComparison.Ordinal))
                    {
                        if (callStatement.Arguments.Arguments.Length == 3 &&
                            callStatement.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax unitVariableReferenceExpression &&
                            callStatement.Arguments.Arguments[1].TryGetIntegerExpressionValue(out var value) &&
                            callStatement.Arguments.Arguments[2] is JassBooleanLiteralExpressionSyntax)
                        {
                            result[^1].HeroAgility = value;
                        }
                        else
                        {
                            units = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Name, "SetHeroInt", StringComparison.Ordinal))
                    {
                        if (callStatement.Arguments.Arguments.Length == 3 &&
                            callStatement.Arguments.Arguments[0] is JassVariableReferenceExpressionSyntax unitVariableReferenceExpression &&
                            callStatement.Arguments.Arguments[1].TryGetIntegerExpressionValue(out var value) &&
                            callStatement.Arguments.Arguments[2] is JassBooleanLiteralExpressionSyntax)
                        {
                            result[^1].HeroIntelligence = value;
                        }
                        else
                        {
                            units = null;
                            return false;
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Name, "SelectHeroSkill", StringComparison.Ordinal))
                    {
                        // TODO
                        continue;
                    }
                    else if (string.Equals(callStatement.IdentifierName.Name, "IssueImmediateOrder", StringComparison.Ordinal))
                    {
                        // TODO
                        continue;
                    }
                    else if (string.Equals(callStatement.IdentifierName.Name, "RandomDistReset", StringComparison.Ordinal))
                    {
                        // TODO
                        continue;
                    }
                    else if (string.Equals(callStatement.IdentifierName.Name, "RandomDistAddItem", StringComparison.Ordinal))
                    {
                        // TODO
                        continue;
                    }
                    else if (string.Equals(callStatement.IdentifierName.Name, "TriggerRegisterUnitEvent", StringComparison.Ordinal))
                    {
                        // TODO
                        continue;
                    }
                    else if (string.Equals(callStatement.IdentifierName.Name, "TriggerAddAction", StringComparison.Ordinal))
                    {
                        // TODO
                        continue;
                    }
                    else if (callStatement.Arguments.Arguments.IsEmpty)
                    {
                        if (Context.FunctionDeclarations.TryGetValue(callStatement.IdentifierName.Name, out var subFunction) &&
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
                    if (ifStatement.Condition.Deparenthesize() is JassBinaryExpressionSyntax binaryExpression &&
                        binaryExpression.Left is JassVariableReferenceExpressionSyntax &&
                        binaryExpression.Operator == BinaryOperatorType.NotEquals &&
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

        private bool TryDecompileCreateItemsFunction(JassFunctionDeclarationSyntax createItemsFunction, [NotNullWhen(true)] out List<UnitData>? items)
        {
            var result = new List<UnitData>();

            foreach (var statement in createItemsFunction.Body.Statements)
            {
                if (statement is JassCommentSyntax ||
                    statement is JassEmptySyntax)
                {
                    continue;
                }
                else if (statement is JassSetStatementSyntax setStatement)
                {
                    if (setStatement.Value.Expression is JassInvocationExpressionSyntax invocationExpression)
                    {
                        if (string.Equals(invocationExpression.IdentifierName.Name, "CreateItem", StringComparison.Ordinal))
                        {
                            if (invocationExpression.Arguments.Arguments.Length == 3 &&
                                invocationExpression.Arguments.Arguments[0].TryGetIntegerExpressionValue(out var unitId) &&
                                invocationExpression.Arguments.Arguments[1].TryGetRealExpressionValue(out var x) &&
                                invocationExpression.Arguments.Arguments[2].TryGetRealExpressionValue(out var y))
                            {
                                var unit = new UnitData
                                {
                                    OwnerId = Context.MaxPlayerSlots + 3, // NEUTRAL_PASSIVE
                                    TypeId = unitId.InvertEndianness(),
                                    Position = new Vector3(x, y, 0f),
                                    Rotation = 0,
                                    Scale = Vector3.One,
                                    Flags = 2,
                                    GoldAmount = 12500,
                                    HeroLevel = 1,
                                    CreationNumber = CreationNumber++
                                };

                                unit.SkinId = unit.TypeId;

                                result.Add(unit);
                            }
                        }
                        else if (string.Equals(invocationExpression.IdentifierName.Name, "BlzCreateItemWithSkin", StringComparison.Ordinal))
                        {
                            if (invocationExpression.Arguments.Arguments.Length == 4 &&
                                invocationExpression.Arguments.Arguments[0].TryGetIntegerExpressionValue(out var unitId) &&
                                invocationExpression.Arguments.Arguments[1].TryGetRealExpressionValue(out var x) &&
                                invocationExpression.Arguments.Arguments[2].TryGetRealExpressionValue(out var y) &&
                                invocationExpression.Arguments.Arguments[3].TryGetIntegerExpressionValue(out var skinId))
                            {
                                var unit = new UnitData
                                {
                                    OwnerId = Context.MaxPlayerSlots + 3, // NEUTRAL_PASSIVE
                                    TypeId = unitId.InvertEndianness(),
                                    Position = new Vector3(x, y, 0f),
                                    Rotation = 0,
                                    Scale = Vector3.One,
                                    SkinId = skinId.InvertEndianness(),
                                    Flags = 2,
                                    GoldAmount = 12500,
                                    HeroLevel = 1,
                                    CreationNumber = CreationNumber++
                                };

                                result.Add(unit);
                            }
                        }
                    }
                }
                else if (statement is JassCallStatementSyntax callStatement)
                {
                    if (string.Equals(callStatement.IdentifierName.Name, "CreateItem", StringComparison.Ordinal))
                    {
                        if (callStatement.Arguments.Arguments.Length == 3 &&
                            callStatement.Arguments.Arguments[0].TryGetIntegerExpressionValue(out var itemId) &&
                            callStatement.Arguments.Arguments[1].TryGetRealExpressionValue(out var x) &&
                            callStatement.Arguments.Arguments[2].TryGetRealExpressionValue(out var y))
                        {
                            var item = new UnitData
                            {
                                OwnerId = Context.MaxPlayerSlots + 3, // NEUTRAL_PASSIVE
                                TypeId = itemId.InvertEndianness(),
                                Position = new Vector3(x, y, 0f),
                                Rotation = 0,
                                Scale = Vector3.One,
                                Flags = 2,
                                GoldAmount = 12500,
                                HeroLevel = 1,
                                CreationNumber = CreationNumber++
                            };

                            item.SkinId = item.TypeId;

                            result.Add(item);
                        }
                    }
                    else if (string.Equals(callStatement.IdentifierName.Name, "BlzCreateItemWithSkin", StringComparison.Ordinal))
                    {
                        if (callStatement.Arguments.Arguments.Length == 4 &&
                            callStatement.Arguments.Arguments[0].TryGetIntegerExpressionValue(out var itemId) &&
                            callStatement.Arguments.Arguments[1].TryGetRealExpressionValue(out var x) &&
                            callStatement.Arguments.Arguments[2].TryGetRealExpressionValue(out var y) &&
                            callStatement.Arguments.Arguments[3].TryGetIntegerExpressionValue(out var skinId))
                        {
                            var item = new UnitData
                            {
                                OwnerId = Context.MaxPlayerSlots + 3, // NEUTRAL_PASSIVE
                                TypeId = itemId.InvertEndianness(),
                                Position = new Vector3(x, y, 0f),
                                Rotation = 0,
                                Scale = Vector3.One,
                                SkinId = skinId.InvertEndianness(),
                                Flags = 2,
                                GoldAmount = 12500,
                                HeroLevel = 1,
                                CreationNumber = CreationNumber++
                            };

                            result.Add(item);
                        }
                    }
                }
            }

            items = result;
            return true;
        }

        private bool TryDecompileStartLocationPositionsConfigFunction(JassFunctionDeclarationSyntax configFunction, [NotNullWhen(true)] out Dictionary<int, Vector2>? startLocationPositions)
        {
            var result = new Dictionary<int, Vector2>();

            foreach (var statement in configFunction.Body.Statements)
            {
                if (statement is JassCallStatementSyntax callStatement &&
                    string.Equals(callStatement.IdentifierName.Name, "DefineStartLocation", StringComparison.Ordinal))
                {
                    if (callStatement.Arguments.Arguments.Length == 3 &&
                        callStatement.Arguments.Arguments[0].TryGetIntegerExpressionValue(out var index) &&
                        callStatement.Arguments.Arguments[1].TryGetRealExpressionValue(out var x) &&
                        callStatement.Arguments.Arguments[2].TryGetRealExpressionValue(out var y))
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

            foreach (var statement in initCustomPlayerSlotsFunction.Body.Statements)
            {
                if (statement is JassCommentSyntax ||
                    statement is JassEmptySyntax)
                {
                    continue;
                }
                else if (statement is JassCallStatementSyntax callStatement)
                {
                    if (string.Equals(callStatement.IdentifierName.Name, "SetPlayerStartLocation", StringComparison.Ordinal))
                    {
                        if (callStatement.Arguments.Arguments.Length == 2 &&
                            callStatement.Arguments.Arguments[0] is JassInvocationExpressionSyntax playerInvocationExpression &&
                            string.Equals(playerInvocationExpression.IdentifierName.Name, "Player", StringComparison.Ordinal) &&
                            playerInvocationExpression.Arguments.Arguments.Length == 1 &&
                            playerInvocationExpression.Arguments.Arguments[0].TryGetPlayerIdExpressionValue(Context.MaxPlayerSlots, out var playerId) &&
                            callStatement.Arguments.Arguments[1].TryGetIntegerExpressionValue(out var startLocationNumber) &&
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
                                CreationNumber = CreationNumber++
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
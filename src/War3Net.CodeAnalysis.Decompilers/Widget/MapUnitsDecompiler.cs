// ------------------------------------------------------------------------------
// 
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// ------------------------------------------------------------------------------

using System;
using System.Linq;
using System.Numerics;

using War3Net.Build.Common;
using War3Net.Build.Environment;
using War3Net.Build.Widget;
using War3Net.CodeAnalysis.Jass.Extensions;
using War3Net.CodeAnalysis.Jass.Syntax;
using War3Net.Common.Extensions;

namespace War3Net.CodeAnalysis.Decompilers
{
    public partial class JassScriptDecompiler
    {
        [RegisterStatementParser]
        internal void ParseUnitCreation(StatementParserInput input)
        {
            var variableAssignment = GetVariableAssignment(input.StatementChildren);
            var unitData = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "CreateUnit" || x.IdentifierName.Name == "BlzCreateUnitWithSkin").SafeMapFirst(x =>
            {
                var args = x.Arguments.Arguments;

                int? ownerId = GetPlayerIndex(args[0]) ?? GetLastCreatedPlayerIndex();

                if (ownerId == null)
                {
                    return null;
                }

                if (!args[1].TryGetValue<int>(out var typeId))
                {
                    return null;
                }

                var result = new UnitData
                {
                    OwnerId = ownerId.Value,
                    TypeId = typeId.InvertEndianness(),
                    Position = new Vector3(
                            args[2].GetValueOrDefault<float>(),
                            args[3].GetValueOrDefault<float>(),
                            0f
                        ),
                    Rotation = args[4].GetValueOrDefault<float>() * (MathF.PI / 180f),
                    Scale = Vector3.One,
                    Flags = 2,
                    GoldAmount = 12500,
                    HeroLevel = 1,
                    CreationNumber = Context.GetNextCreationNumber()
                };

                result.SkinId = args.Length > 5 ? args[5].GetValueOrDefault<int>() : result.TypeId;
                return result;
            });

            if (unitData != null)
            {
                Context.HandledStatements.Add(input.Statement);
                Context.Add(unitData, variableAssignment);
            }
        }

        [RegisterStatementParser]
        internal void ParsePlayerIndex(StatementParserInput input)
        {
            var variableAssignment = GetVariableAssignment(input.StatementChildren);
            var match = input.StatementChildren.OfType<JassEqualsValueClauseSyntax>().Where(x => x.Expression is IInvocationSyntax invocationSyntax && invocationSyntax.IdentifierName.Name == "Player").SafeMapFirst(x =>
            {
                var playerId = GetPlayerIndex(x.Expression);
                if (!playerId.HasValue)
                {
                    return null;
                }

                return new
                {
                    PlayerIndex = playerId.Value
                };
            });

            if (match != null && variableAssignment != null)
            {
                Context.Add_Struct(match.PlayerIndex, Context.CreatePseudoVariableName(nameof(ParsePlayerIndex), variableAssignment));
                Context.Add_Struct(match.PlayerIndex, Context.CreatePseudoVariableName(nameof(ParsePlayerIndex)));
                Context.HandledStatements.Add(input.Statement);
            }
        }

        [RegisterStatementParser]
        internal void ParseResourceAmount(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "SetResourceAmount").SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    Amount = x.Arguments.Arguments[1].GetValueOrDefault<int>()
                };
            });

            if (match != null)
            {
                var unit = Context.Get<UnitData>(match.VariableName) ?? Context.GetLastCreated<UnitData>();
                if (unit != null)
                {
                    unit.GoldAmount = match.Amount;
                    Context.HandledStatements.Add(input.Statement);
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseSetUnitColor(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "SetUnitColor").SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    CustomPlayerColorId = x.Arguments.Arguments[1].GetValueOrDefault<int>()
                };
            });

            if (match != null)
            {
                var unit = Context.Get<UnitData>(match.VariableName) ?? Context.GetLastCreated<UnitData>();
                if (unit != null)
                {
                    unit.CustomPlayerColorId = match.CustomPlayerColorId;
                    Context.HandledStatements.Add(input.Statement);
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseAcquireRange(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x =>
                x.IdentifierName.Name == "SetUnitAcquireRange")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    Range = x.Arguments.Arguments[1].GetValueOrDefault<float>()
                };
            });

            if (match != null)
            {
                var unit = Context.Get<UnitData>(match.VariableName) ?? Context.GetLastCreated<UnitData>();
                if (unit != null)
                {
                    unit.TargetAcquisition = match.Range;
                    Context.HandledStatements.Add(input.Statement);
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseUnitState(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x =>
                x.IdentifierName.Name == "SetUnitState")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    StateType = ((JassVariableReferenceExpressionSyntax)x.Arguments.Arguments[1]).IdentifierName.Name,
                    Value = x.Arguments.Arguments[2].GetValueOrDefault<float>()
                };
            });


            if (match != null)
            {
                var unit = Context.Get<UnitData>(match.VariableName) ?? Context.GetLastCreated<UnitData>();
                if (unit != null)
                {
                    if (match.StateType == "UNIT_STATE_LIFE")
                    {
                        unit.HP = (int)match.Value;
                        Context.HandledStatements.Add(input.Statement);
                    }
                    else if (match.StateType == "UNIT_STATE_MANA")
                    {
                        unit.MP = (int)match.Value;
                        Context.HandledStatements.Add(input.Statement);
                    }
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseUnitInventory(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x =>
                x.IdentifierName.Name == "UnitAddItemToSlotById")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    ItemId = x.Arguments.Arguments[1].GetValueOrDefault<int>(),
                    Slot = x.Arguments.Arguments[2].GetValueOrDefault<int>()
                };
            });


            if (match != null)
            {
                var unit = Context.Get<UnitData>(match.VariableName) ?? Context.GetLastCreated<UnitData>();
                if (unit != null)
                {
                    unit.InventoryData.Add(new InventoryItemData
                    {
                        ItemId = match.ItemId,
                        Slot = match.Slot
                    });
                    Context.HandledStatements.Add(input.Statement);
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseHeroLevel(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "SetHeroLevel").SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    Level = x.Arguments.Arguments[1].GetValueOrDefault<int>()
                };
            });

            if (match != null)
            {
                var unit = Context.Get<UnitData>(match.VariableName) ?? Context.GetLastCreated<UnitData>();
                if (unit != null)
                {
                    unit.HeroLevel = match.Level;
                    Context.HandledStatements.Add(input.Statement);
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseHeroAttributes(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x =>
                x.IdentifierName.Name == "SetHeroStr" ||
                 x.IdentifierName.Name == "SetHeroAgi" ||
                 x.IdentifierName.Name == "SetHeroInt")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    Attribute = x.IdentifierName.Name,
                    Value = x.Arguments.Arguments[1].GetValueOrDefault<int>()
                };
            });


            if (match != null)
            {
                var unit = Context.Get<UnitData>(match.VariableName) ?? Context.GetLastCreated<UnitData>();
                if (unit != null)
                {
                    var handled = true;
                    switch (match.Attribute)
                    {
                        case "SetHeroStr":
                            unit.HeroStrength = match.Value;
                            break;
                        case "SetHeroAgi":
                            unit.HeroAgility = match.Value;
                            break;
                        case "SetHeroInt":
                            unit.HeroIntelligence = match.Value;
                            break;
                        default:
                            handled = false;
                            break;
                    }

                    if (handled)
                    {
                        Context.HandledStatements.Add(input.Statement);
                    }
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseHeroSkills(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x =>
                x.IdentifierName.Name == "SelectHeroSkill")
            .SafeMapFirst(x =>
            {
                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    SkillId = x.Arguments.Arguments[1].GetValueOrDefault<int>()
                };
            });

            if (match != null)
            {
                var unit = Context.Get<UnitData>(match.VariableName) ?? Context.GetLastCreated<UnitData>();
                if (unit != null)
                {
                    unit.AbilityData.Add(new ModifiedAbilityData
                    {
                        AbilityId = match.SkillId,
                        HeroAbilityLevel = 1,
                        IsAutocastActive = false
                    });
                    Context.HandledStatements.Add(input.Statement);
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseIssueImmediateOrder(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x =>
                x.IdentifierName.Name == "IssueImmediateOrder")
            .SafeMapFirst(x =>
            {
                var abilityName = (x.Arguments.Arguments[1] as JassStringLiteralExpressionSyntax)?.Value;
                bool? isAutoCastActive = null;
                if (abilityName?.EndsWith("on", StringComparison.OrdinalIgnoreCase) == true)
                {
                    isAutoCastActive = true;
                }
                else if (abilityName?.EndsWith("off", StringComparison.OrdinalIgnoreCase) == true)
                {
                    isAutoCastActive = false;
                }
                else
                {
                    return null;
                }

                return new
                {
                    VariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    //AbilityName = abilityName,
                    IsAutocastActive = isAutoCastActive.Value
                };
            });

            if (match != null)
            {
                var unit = Context.Get<UnitData>(match.VariableName) ?? Context.GetLastCreated<UnitData>();
                if (unit != null)
                {
                    //todo: lookup abilityId by searching for name in _context.ObjectData.map.AbilityObjectData
                    //todo: lookup on/off via SLK Metadata instead of abilityName suffix [example: Modifications with FourCC aoro/aord are names for "on", aorf/aoru are names for "off"]
                    var ability = unit.AbilityData.LastOrDefault();
                    if (ability != null)
                    {
                        ability.IsAutocastActive = match.IsAutocastActive;
                        Context.HandledStatements.Add(input.Statement);
                    }
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseDropItemsOnDeath_TriggerRegister(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x =>
                x.IdentifierName.Name == "TriggerRegisterUnitEvent")
            .SafeMapFirst(x =>
            {
                var eventName = (x.Arguments.Arguments[2] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name;
                if (eventName != "EVENT_UNIT_DEATH" && eventName != "EVENT_UNIT_CHANGE_OWNER")
                {
                    return null;
                }

                return new
                {
                    TriggerVariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    UnitVariableName = (x.Arguments.Arguments[1] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name
                };
            });

            if (match != null)
            {
                Context.Add(match.UnitVariableName, Context.CreatePseudoVariableName(nameof(ParseDropItemsOnDeath_TriggerRegister), match.TriggerVariableName));
                Context.HandledStatements.Add(input.Statement);
            }
        }

        [RegisterStatementParser]
        internal void ParseDropItemsOnDeath_TriggerAction(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x =>
                x.IdentifierName.Name == "TriggerAddAction")
            .SafeMapFirst(x =>
            {
                return new
                {
                    TriggerVariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    FunctionName = (x.Arguments.Arguments[1] as JassFunctionReferenceExpressionSyntax)?.IdentifierName?.Name
                };
            });

            if (match != null)
            {
                var unitVariableName = Context.Get<string>(Context.CreatePseudoVariableName(nameof(ParseDropItemsOnDeath_TriggerRegister), match.TriggerVariableName));

                if (match.FunctionName != null && Context.FunctionDeclarations.TryGetValue(match.FunctionName, out var function))
                {
                    var statements = function.FunctionDeclaration.GetChildren_RecursiveDepthFirst().OfType<IStatementLineSyntax>().ToList();
                    foreach (var statement in statements)
                    {
                        ProcessStatementParsers(statement, new Action<StatementParserInput>[] {
                            input => ParseRandomDistReset(input, unitVariableName),
                            input => ParseRandomDistAddItem(input, unitVariableName),
                        });
                    }

                    Context.HandledStatements.Add(input.Statement);
                }
            }
        }

        private void ParseRandomDistReset(StatementParserInput input, string unitVariableName)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x =>
                x.IdentifierName.Name == "RandomDistReset")
            .SafeMapFirst(x =>
            {
                return new RandomItemSet();
            });

            if (match != null)
            {
                var unit = Context.Get<UnitData>(unitVariableName) ?? Context.GetLastCreated<UnitData>();
                if (unit != null)
                {
                    unit.ItemTableSets.Add(match);
                }
                Context.Add(match);
                Context.HandledStatements.Add(input.Statement);
            }
        }

        private void ParseRandomDistAddItem(StatementParserInput input, string unitVariableName)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x =>
                x.IdentifierName.Name == "RandomDistAddItem")
            .SafeMapFirst(x =>
            {
                return new RandomItemSetItem()
                {
                    ItemId = x.Arguments.Arguments[0].GetValueOrDefault<int>().InvertEndianness(),
                    Chance = x.Arguments.Arguments[1].GetValueOrDefault<int>(),
                };
            });

            if (match != null)
            {
                var unit = Context.Get<UnitData>(unitVariableName) ?? Context.GetLastCreated<UnitData>();
                if (unit != null)
                {
                    var itemSet = unit.ItemTableSets.LastOrDefault();
                    if (itemSet != null)
                    {
                        itemSet.Items.Add(match);
                        Context.HandledStatements.Add(input.Statement);
                    }
                }
            }
        }

        [RegisterStatementParser]
        internal void ParseItemCreation(StatementParserInput input)
        {
            var variableAssignment = GetVariableAssignment(input.StatementChildren);
            var unitData = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "CreateItem" || x.IdentifierName.Name == "BlzCreateItemWithSkin").SafeMapFirst(x =>
            {
                var args = x.Arguments.Arguments;

                if (!args[0].TryGetValue<int>(out var typeId))
                {
                    return null;
                }

                var result = new UnitData
                {
                    OwnerId = Context.MaxPlayerSlots + 3, // NEUTRAL_PASSIVE
                    TypeId = typeId.InvertEndianness(),
                    Position = new Vector3(
                            args[1].GetValueOrDefault<float>(),
                            args[2].GetValueOrDefault<float>(),
                            0f
                        ),
                    Rotation = 0,
                    Scale = Vector3.One,
                    Flags = 2,
                    GoldAmount = 12500,
                    HeroLevel = 1,
                    CreationNumber = Context.GetNextCreationNumber()
                };

                result.SkinId = args.Length > 3 ? args[3].GetValueOrDefault<int>() : result.TypeId;

                return result;
            });

            if (unitData != null)
            {
                Context.Add(unitData, variableAssignment);
                Context.HandledStatements.Add(input.Statement);
            }
        }

        [RegisterStatementParser]
        internal void ParseDefineStartLocation(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x =>
                x.IdentifierName.Name == "DefineStartLocation")
            .SafeMapFirst(x =>
            {
                return new
                {
                    Index = x.Arguments.Arguments[0].GetValueOrDefault<int>(),
                    Location = new Vector2(x.Arguments.Arguments[1].GetValueOrDefault<float>(), x.Arguments.Arguments[2].GetValueOrDefault<float>())
                };
            });

            if (match != null)
            {
                Context.Add_Struct(match.Location, Context.CreatePseudoVariableName(nameof(ParseDefineStartLocation), match.Index.ToString()));
                Context.HandledStatements.Add(input.Statement);
            }
        }

        [RegisterStatementParser]
        internal void ParseSetPlayerStartLocation(StatementParserInput input)
        {
            var variableAssignment = GetVariableAssignment(input.StatementChildren);
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x =>
               x.IdentifierName.Name == "SetPlayerStartLocation")
           .SafeMapFirst(x =>
           {
               int? playerId = GetPlayerIndex(x.Arguments.Arguments[0]) ?? GetLastCreatedPlayerIndex();

               if (playerId == null)
               {
                   return null;
               }

               var startLocationIndex = x.Arguments.Arguments[1].GetValueOrDefault<int>();
               var startLocationPosition = Context.Get_Struct<Vector2>(Context.CreatePseudoVariableName(nameof(ParseDefineStartLocation), startLocationIndex.ToString()));

               if (startLocationPosition == null)
               {
                   return null;
               }

               var args = x.Arguments.Arguments;
               var result = new UnitData
               {
                   OwnerId = playerId.Value,
                   TypeId = "sloc".FromRawcode(),
                   Position = new Vector3(startLocationPosition.Value, 0f),
                   Rotation = MathF.PI * 1.5f,
                   Scale = Vector3.One,
                   Flags = 2,
                   GoldAmount = 12500,
                   HeroLevel = 0,
                   TargetAcquisition = 0,
                   CreationNumber = Context.GetNextCreationNumber()
               };

               result.SkinId = result.TypeId;

               return result;
           });

            if (match != null)
            {
                Context.Add(match, variableAssignment);
                Context.HandledStatements.Add(input.Statement);
            }
        }

        [RegisterStatementParser]
        internal void ParseWaygateDestination(StatementParserInput input)
        {
            var match = input.StatementChildren.OfType<IInvocationSyntax>().Where(x =>
                x.IdentifierName.Name == "WaygateSetDestination")
            .SafeMapFirst(x =>
            {
                var regionVariableName = input.StatementChildren.OfType<IInvocationSyntax>().Where(x => x.IdentifierName.Name == "GetRectCenterX" || x.IdentifierName.Name == "GetRectCenterY").SafeMapFirst(x => ((JassVariableReferenceExpressionSyntax)x.Arguments.Arguments[0]).IdentifierName.Name);
                if (regionVariableName == null)
                {
                    var destination = new Vector2(x.Arguments.Arguments[1].GetValueOrDefault<float>(), x.Arguments.Arguments[2].GetValueOrDefault<float>());
                    var region = Context.GetAll<Region>().LastOrDefault(x => x.CenterX == destination.X && x.CenterY == destination.Y);
                    regionVariableName = Context.GetVariableName(region);
                }

                return new
                {
                    UnitVariableName = (x.Arguments.Arguments[0] as JassVariableReferenceExpressionSyntax)?.IdentifierName?.Name,
                    RegionVariableName = regionVariableName
                };
            });

            if (match != null)
            {
                var unit = Context.Get<UnitData>(match.UnitVariableName) ?? Context.GetLastCreated<UnitData>();
                var region = Context.Get<Region>(match.RegionVariableName) ?? Context.GetLastCreated<Region>();
                if (unit != null && region != null)
                {
                    unit.WaygateDestinationRegionId = region.CreationNumber;
                    Context.HandledStatements.Add(input.Statement);
                }
            }
        }
    }
}
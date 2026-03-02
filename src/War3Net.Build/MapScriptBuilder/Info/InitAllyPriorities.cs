// ------------------------------------------------------------------------------
// <copyright file="InitAllyPriorities.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Linq;
using War3Net.Build.Info;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateInitAllyPriorities(Map map, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var mapInfo = map.Info;
            if (mapInfo is null)
            {
                throw new ArgumentException($"Function '{GeneratedFunctionName.InitAllyPriorities}' cannot be generated without {nameof(MapInfo)}.", nameof(map));
            }

            writer.WriteFunction(GeneratedFunctionName.InitAllyPriorities);

            var playerDataCount = mapInfo.Players.Count;
            for (var i = 0; i < playerDataCount; i++)
            {
                var playerData = mapInfo.Players[i];

                var allySlotIndex = 0;
                var enemySlotIndex = 0;

                writer.WriteLine();

                var allyCountIndex = 0;
                for (var j = 0; j < MaxPlayerSlots; j++)
                {
                    if (playerData.AllyLowPriorityFlags[j] || playerData.AllyHighPriorityFlags[j])
                    {
                        allyCountIndex++;
                    }
                }

                writer.WriteCall(
                    NativeName.SetStartLocPrioCount,
                    JassLiteral.Int(i),
                    JassLiteral.Int(allyCountIndex));

                for (var j = 0; j < MaxPlayerSlots; j++)
                {
                    var hasLowFlag = playerData.AllyLowPriorityFlags[j];
                    var hasHighFlag = playerData.AllyHighPriorityFlags[j];
                    if (hasLowFlag || hasHighFlag)
                    {
                        writer.WriteCall(
                            NativeName.SetStartLocPrio,
                            JassLiteral.Int(i),
                            JassLiteral.Int(allySlotIndex++),
                            JassLiteral.Int(j),
                            hasHighFlag ? StartLocPrioName.High : StartLocPrioName.Low);
                    }

                    if (mapInfo.FormatVersion >= MapInfoFormatVersion.v31)
                    {
                        hasLowFlag = playerData.EnemyLowPriorityFlags[j];
                        hasHighFlag = playerData.EnemyHighPriorityFlags[j];
                        if (hasLowFlag || hasHighFlag)
                        {
                            if (enemySlotIndex == 0)
                            {
                                writer.WriteLine();

                                var enemyCountIndex = 0;
                                for (var k = 0; k < MaxPlayerSlots; k++)
                                {
                                    if (playerData.EnemyLowPriorityFlags[k] || playerData.EnemyHighPriorityFlags[k])
                                    {
                                        enemyCountIndex++;
                                    }
                                }

                                writer.WriteCall(
                                    NativeName.SetEnemyStartLocPrioCount,
                                    JassLiteral.Int(i),
                                    JassLiteral.Int(enemyCountIndex));
                            }

                            writer.WriteCall(
                                NativeName.SetEnemyStartLocPrio,
                                JassLiteral.Int(i),
                                JassLiteral.Int(enemySlotIndex++),
                                JassLiteral.Int(j),
                                hasHighFlag ? StartLocPrioName.High : StartLocPrioName.Low);
                        }
                    }
                }
            }

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateInitAllyPriorities(Map map)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (map.Info is null)
            {
                return false;
            }

            var ids = Enumerable.Range(0, MaxPlayerSlots).ToArray();
            return map.Info.Players.Any(p => ids.Any(id => p.AllyLowPriorityFlags[id] || p.AllyHighPriorityFlags[id]));
        }
    }
}
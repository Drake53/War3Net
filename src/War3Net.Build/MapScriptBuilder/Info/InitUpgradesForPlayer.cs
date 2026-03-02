// ------------------------------------------------------------------------------
// <copyright file="InitUpgradesForPlayer.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using War3Net.Build.Info;
using War3Net.CodeAnalysis;
using War3Net.CodeAnalysis.Jass;
using War3Net.CodeAnalysis.Jass.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateInitUpgradesForPlayer(Map map, int playerId, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var functionName = GeneratedFunctionName.InitUpgradesForPlayer(playerId);

            var mapInfo = map.Info;
            if (mapInfo is null)
            {
                throw new ArgumentException($"Function '{functionName}' cannot be generated without {nameof(MapInfo)}.", nameof(map));
            }

            writer.WriteFunction(functionName);

            var maxLevel = new Dictionary<int, int>();
            var researched = new Dictionary<int, int>();
            for (var i = 0; i < mapInfo.UpgradeData.Count; i++)
            {
                var upgradeData = mapInfo.UpgradeData[i];
                if (upgradeData.Availability != UpgradeAvailability.Available && upgradeData.Players[playerId])
                {
                    if (upgradeData.Availability == UpgradeAvailability.Unavailable)
                    {
                        if (maxLevel.TryGetValue(upgradeData.Id, out var level))
                        {
                            if (upgradeData.Level < level)
                            {
                                maxLevel[upgradeData.Id] = upgradeData.Level;
                            }
                        }
                        else
                        {
                            maxLevel.Add(upgradeData.Id, upgradeData.Level);
                        }
                    }
                    else if (upgradeData.Availability == UpgradeAvailability.Researched)
                    {
                        if (researched.TryGetValue(upgradeData.Id, out var level))
                        {
                            if (upgradeData.Level > level)
                            {
                                researched[upgradeData.Id] = upgradeData.Level;
                            }
                        }
                        else
                        {
                            researched.Add(upgradeData.Id, upgradeData.Level);
                        }
                    }
                }
            }

            foreach (var tech in maxLevel)
            {
                writer.WriteCall(
                    NativeName.SetPlayerTechMaxAllowed,
                    JassExpression.Invoke(NativeName.Player, JassLiteral.Int(playerId)),
                    JassLiteral.FourCC(tech.Key),
                    JassLiteral.Int(tech.Value));
            }

            foreach (var tech in researched)
            {
                writer.WriteCall(
                    NativeName.SetPlayerTechResearched,
                    JassExpression.Invoke(NativeName.Player, JassLiteral.Int(playerId)),
                    JassLiteral.FourCC(tech.Key),
                    JassLiteral.Int(tech.Value + 1));
            }

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateInitUpgradesForPlayer(Map map, int playerId)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null
                && map.Info.UpgradeData.Any(upgradeData => upgradeData.Availability != UpgradeAvailability.Available && upgradeData.Players[playerId]);
        }
    }
}
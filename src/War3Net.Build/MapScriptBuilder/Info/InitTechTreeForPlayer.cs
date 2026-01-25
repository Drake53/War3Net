// ------------------------------------------------------------------------------
// <copyright file="InitTechTreeForPlayer.cs" company="Drake53">
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
using War3Net.Common.Extensions;

namespace War3Net.Build
{
    public partial class MapScriptBuilder
    {
        protected internal virtual void GenerateInitTechTreeForPlayer(Map map, int playerId, IndentedTextWriter writer)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            var functionName = GeneratedFunctionName.InitTechTreeForPlayer(playerId);

            var mapInfo = map.Info;
            if (mapInfo is null)
            {
                throw new ArgumentException($"Function '{functionName}' cannot be generated without {nameof(MapInfo)}.", nameof(map));
            }

            writer.WriteFunction(functionName);

            foreach (var techData in mapInfo.TechData)
            {
                if (techData.Players[playerId])
                {
                    if (techData.Id.ToRawcode()[0] == 'A')
                    {
                        writer.WriteCall(
                            NativeName.SetPlayerAbilityAvailable,
                            JassExpression.Invoke(NativeName.Player, JassLiteral.Int(playerId)),
                            JassLiteral.FourCC(techData.Id),
                            JassKeyword.False);
                    }
                    else
                    {
                        writer.WriteCall(
                            NativeName.SetPlayerTechMaxAllowed,
                            JassExpression.Invoke(NativeName.Player, JassLiteral.Int(playerId)),
                            JassLiteral.FourCC(techData.Id),
                            JassLiteral.Int(0));
                    }
                }
            }

            writer.EndFunction();
        }

        protected internal virtual bool ShouldGenerateInitTechTreeForPlayer(Map map, int playerId)
        {
            if (map is null)
            {
                throw new ArgumentNullException(nameof(map));
            }

            return map.Info is not null
                && map.Info.TechData.Any(techData => techData.Players[playerId]);
        }
    }
}
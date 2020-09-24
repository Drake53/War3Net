// ------------------------------------------------------------------------------
// <copyright file="PlayerGameResult.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace War3Net.Runtime.Common.Enums
{
    public sealed class PlayerGameResult
    {
        private static readonly Dictionary<int, PlayerGameResult> _results = GetTypes().ToDictionary(t => (int)t, t => new PlayerGameResult(t));

        private readonly Type _type;

        private PlayerGameResult(Type type)
        {
            _type = type;
        }

        public enum Type
        {
            Victory = 0,
            Defeat = 1,
            Tie = 2,
            Neutral = 3,
        }

        public static PlayerGameResult GetPlayerGameResult(int i)
        {
            if (!_results.TryGetValue(i, out var playerGameResult))
            {
                playerGameResult = new PlayerGameResult((Type)i);
                _results.Add(i, playerGameResult);
            }

            return playerGameResult;
        }

        private static IEnumerable<Type> GetTypes()
        {
            foreach (Type type in Enum.GetValues(typeof(Type)))
            {
                yield return type;
            }
        }
    }
}
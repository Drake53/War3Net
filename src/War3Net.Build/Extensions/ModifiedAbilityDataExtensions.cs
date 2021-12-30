// ------------------------------------------------------------------------------
// <copyright file="ModifiedAbilityDataExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using War3Net.Build.Widget;
using War3Net.Common.Extensions;

namespace War3Net.Build.Extensions
{
    public static class ModifiedAbilityDataExtensions
    {
        private static readonly Lazy<Dictionary<int, string>> _abilityOrderOffStrings = new(GetAbilityOrderOffStrings);

        public static bool TryGetOrderOffString(this ModifiedAbilityData abilityData, [NotNullWhen(true)] out string? orderOffString)
        {
            return _abilityOrderOffStrings.Value.TryGetValue(abilityData.AbilityId, out orderOffString);
        }

        private static Dictionary<int, string> GetAbilityOrderOffStrings()
        {
            return new Dictionary<int, string>
            {
                { "ANia".FromRawcode(), "incineratearrowon" },
                { "ACpa".FromRawcode(), "parasiteoff" },
                { "ANms".FromRawcode(), "manashieldoff" },
                { "ANba".FromRawcode(), "blackarrowoff" },
                { "Anhe".FromRawcode(), "healoff" },
                { "ACbb".FromRawcode(), "bloodlustoff" },
                { "Afzy".FromRawcode(), "frenzyoff" },
                { "ACbl".FromRawcode(), "bloodlustoff" },
                { "ACcs".FromRawcode(), "curseoff" },
                { "ACff".FromRawcode(), "faeriefireoff" },
                { "ACf2".FromRawcode(), "frostarmoroff" },
                { "Anh1".FromRawcode(), "healoff" },
                { "Anh2".FromRawcode(), "healoff" },
                { "ACif".FromRawcode(), "innerfireoff" },
                { "ACrd".FromRawcode(), "raisedeadoff" },
                { "Ahrp".FromRawcode(), "repairoff" },
                { "ACwb".FromRawcode(), "weboff" },
                { "ACdm".FromRawcode(), "autodispeloff" },
                { "ACd2".FromRawcode(), "autodispeloff" },
            };
        }
    }
}
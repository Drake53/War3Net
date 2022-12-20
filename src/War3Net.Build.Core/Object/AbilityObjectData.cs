// ------------------------------------------------------------------------------
// <copyright file="AbilityObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Object
{
    public abstract partial class AbilityObjectData
    {
        internal AbilityObjectData(ObjectDataFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        public ObjectDataFormatVersion FormatVersion { get; set; }

        public List<LevelObjectModification> BaseAbilities { get; init; } = new();

        public List<LevelObjectModification> NewAbilities { get; init; } = new();
    }
}
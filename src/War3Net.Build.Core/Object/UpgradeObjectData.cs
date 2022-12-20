// ------------------------------------------------------------------------------
// <copyright file="UpgradeObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Object
{
    public abstract partial class UpgradeObjectData
    {
        internal UpgradeObjectData(ObjectDataFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        public ObjectDataFormatVersion FormatVersion { get; set; }

        public List<LevelObjectModification> BaseUpgrades { get; init; } = new();

        public List<LevelObjectModification> NewUpgrades { get; init; } = new();
    }
}
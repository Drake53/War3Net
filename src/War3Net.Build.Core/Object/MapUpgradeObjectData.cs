// ------------------------------------------------------------------------------
// <copyright file="MapUpgradeObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Object
{
    public sealed partial class MapUpgradeObjectData : UpgradeObjectData
    {
        public const string FileName = "war3map.w3q";

        /// <summary>
        /// Initializes a new instance of the <see cref="MapUpgradeObjectData"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public MapUpgradeObjectData(ObjectDataFormatVersion formatVersion)
            : base(formatVersion)
        {
        }

        public override string ToString() => FileName;
    }
}
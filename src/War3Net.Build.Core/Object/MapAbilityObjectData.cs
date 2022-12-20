// ------------------------------------------------------------------------------
// <copyright file="MapAbilityObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Object
{
    public sealed partial class MapAbilityObjectData : AbilityObjectData
    {
        public const string FileName = "war3map.w3a";

        /// <summary>
        /// Initializes a new instance of the <see cref="MapAbilityObjectData"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public MapAbilityObjectData(ObjectDataFormatVersion formatVersion)
            : base(formatVersion)
        {
        }

        public override string ToString() => FileName;
    }
}
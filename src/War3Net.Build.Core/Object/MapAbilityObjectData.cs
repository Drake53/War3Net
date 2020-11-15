// ------------------------------------------------------------------------------
// <copyright file="MapAbilityObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Object
{
    public sealed class MapAbilityObjectData : AbilityObjectData
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

        internal MapAbilityObjectData(BinaryReader reader)
            : base(reader)
        {
        }
    }
}
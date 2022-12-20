// ------------------------------------------------------------------------------
// <copyright file="MapUnitObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Object
{
    public sealed partial class MapUnitObjectData : UnitObjectData
    {
        public const string FileName = "war3map.w3u";

        /// <summary>
        /// Initializes a new instance of the <see cref="MapUnitObjectData"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public MapUnitObjectData(ObjectDataFormatVersion formatVersion)
            : base(formatVersion)
        {
        }

        public override string ToString() => FileName;
    }
}
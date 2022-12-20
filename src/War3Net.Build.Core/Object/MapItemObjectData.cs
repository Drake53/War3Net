// ------------------------------------------------------------------------------
// <copyright file="MapItemObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Object
{
    public sealed partial class MapItemObjectData : ItemObjectData
    {
        public const string FileName = "war3map.w3t";

        /// <summary>
        /// Initializes a new instance of the <see cref="MapItemObjectData"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public MapItemObjectData(ObjectDataFormatVersion formatVersion)
            : base(formatVersion)
        {
        }

        public override string ToString() => FileName;
    }
}
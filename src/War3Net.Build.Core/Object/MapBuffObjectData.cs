// ------------------------------------------------------------------------------
// <copyright file="MapBuffObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Object
{
    public sealed partial class MapBuffObjectData : BuffObjectData
    {
        public const string FileName = "war3map.w3h";

        /// <summary>
        /// Initializes a new instance of the <see cref="MapBuffObjectData"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public MapBuffObjectData(ObjectDataFormatVersion formatVersion)
            : base(formatVersion)
        {
        }

        public override string ToString() => FileName;
    }
}
// ------------------------------------------------------------------------------
// <copyright file="MapDestructableObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Object
{
    public sealed partial class MapDestructableObjectData : DestructableObjectData
    {
        public const string FileName = "war3map.w3b";

        /// <summary>
        /// Initializes a new instance of the <see cref="MapDestructableObjectData"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public MapDestructableObjectData(ObjectDataFormatVersion formatVersion)
            : base(formatVersion)
        {
        }

        public override string ToString() => FileName;
    }
}
// ------------------------------------------------------------------------------
// <copyright file="MapBuffObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Object
{
    public sealed class MapBuffObjectData : BuffObjectData
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

        internal MapBuffObjectData(BinaryReader reader)
            : base(reader)
        {
        }

        public override string ToString() => FileName;
    }
}
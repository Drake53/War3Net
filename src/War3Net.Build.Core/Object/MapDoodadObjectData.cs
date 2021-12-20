// ------------------------------------------------------------------------------
// <copyright file="MapDoodadObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Object
{
    public sealed class MapDoodadObjectData : DoodadObjectData
    {
        public const string FileName = "war3map.w3d";

        /// <summary>
        /// Initializes a new instance of the <see cref="MapDoodadObjectData"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public MapDoodadObjectData(ObjectDataFormatVersion formatVersion)
            : base(formatVersion)
        {
        }

        internal MapDoodadObjectData(BinaryReader reader)
            : base(reader)
        {
        }

        public override string ToString() => FileName;
    }
}
// ------------------------------------------------------------------------------
// <copyright file="ForceData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Common;

namespace War3Net.Build.Info
{
    public sealed partial class ForceData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ForceData"/> class.
        /// </summary>
        public ForceData()
        {
        }

        internal ForceData(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        public ForceFlags Flags { get; set; }

        public Bitmask32 Players { get; set; }

        public string Name { get; set; }

        public override string ToString() => Name;
    }
}
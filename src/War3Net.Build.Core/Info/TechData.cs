// ------------------------------------------------------------------------------
// <copyright file="TechData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Common;
using War3Net.Common.Extensions;

namespace War3Net.Build.Info
{
    public sealed partial class TechData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TechData"/> class.
        /// </summary>
        public TechData()
        {
        }

        internal TechData(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        public Bitmask32 Players { get; set; }

        public int Id { get; set; }

        public override string ToString() => Id.ToRawcode();
    }
}
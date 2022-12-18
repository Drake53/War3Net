// ------------------------------------------------------------------------------
// <copyright file="RandomItemSetItem.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Info;
using War3Net.Build.Widget;
using War3Net.Common.Extensions;

namespace War3Net.Build.Common
{
    public sealed partial class RandomItemSetItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomItemSetItem"/> class.
        /// </summary>
        public RandomItemSetItem()
        {
        }

        internal RandomItemSetItem(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        internal RandomItemSetItem(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            ReadFrom(reader, formatVersion, subVersion, useNewFormat);
        }

        public int Chance { get; set; }

        public int ItemId { get; set; }

        public override string ToString() => $"{ItemId.ToRawcode()} ({Chance}%)";
    }
}
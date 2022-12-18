﻿// ------------------------------------------------------------------------------
// <copyright file="RandomItemSet.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;

using War3Net.Build.Info;
using War3Net.Build.Widget;

namespace War3Net.Build.Common
{
    public sealed partial class RandomItemSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomItemSet"/> class.
        /// </summary>
        public RandomItemSet()
        {
        }

        internal RandomItemSet(BinaryReader reader, MapInfoFormatVersion formatVersion)
        {
            ReadFrom(reader, formatVersion);
        }

        internal RandomItemSet(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            ReadFrom(reader, formatVersion, subVersion, useNewFormat);
        }

        public List<RandomItemSetItem> Items { get; init; } = new();
    }
}
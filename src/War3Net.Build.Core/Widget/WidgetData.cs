// ------------------------------------------------------------------------------
// <copyright file="WidgetData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Numerics;

using War3Net.Build.Common;

namespace War3Net.Build.Widget
{
    public abstract class WidgetData
    {
        internal WidgetData()
        {
        }

        public int TypeId { get; set; }

        public int Variation { get; set; }

        public Vector3 Position { get; set; }

        /// <summary>
        /// The unit's facing angle (in radians).
        /// </summary>
        public float Rotation { get; set; }

        public Vector3 Scale { get; set; }

        public int SkinId { get; set; }

        public int MapItemTableId { get; set; } = -1;

        public List<RandomItemSet> ItemTableSets { get; init; } = new();

        public int CreationNumber { get; set; }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="Extent.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Numerics;

namespace War3Net.Modeling.DataStructures
{
    public struct Extent
    {
        public float BoundsRadius { get; set; }

        public Vector3 MinimumExtent { get; set; }

        public Vector3 MaximumExtent { get; set; }
    }
}
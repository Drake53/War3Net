// ------------------------------------------------------------------------------
// <copyright file="Region.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Drawing;

using War3Net.Build.Common;

namespace War3Net.Build.Environment
{
    public sealed partial class Region
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Region"/> class.
        /// </summary>
        public Region()
        {
        }

        public float Left { get; set; }

        public float Bottom { get; set; }

        public float Right { get; set; }

        public float Top { get; set; }

        public string Name { get; set; }

        public int CreationNumber { get; set; }

        public WeatherType WeatherType { get; set; }

        public string AmbientSound { get; set; }

        public Color Color { get; set; }

        public float Width => Right - Left;

        public float Height => Top - Bottom;

        public float CenterX => 0.5f * (Left + Right);

        public float CenterY => 0.5f * (Top + Bottom);

        public override string ToString() => Name;
    }
}
// ------------------------------------------------------------------------------
// <copyright file="PreviewIcon.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Drawing;

namespace War3Net.Build.Environment
{
    public sealed partial class PreviewIcon
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewIcon"/> class.
        /// </summary>
        public PreviewIcon()
        {
        }

        public PreviewIconType IconType { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public Color Color { get; set; }

        public override string ToString() => IconType.ToString();
    }
}
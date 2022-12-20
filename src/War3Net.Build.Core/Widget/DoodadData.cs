// ------------------------------------------------------------------------------
// <copyright file="DoodadData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Widget
{
    public sealed partial class DoodadData : WidgetData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DoodadData"/> class.
        /// </summary>
        public DoodadData()
        {
        }

        public DoodadState State { get; set; }

        // in %, where 0x64 = 100%
        public byte Life { get; set; }
    }
}
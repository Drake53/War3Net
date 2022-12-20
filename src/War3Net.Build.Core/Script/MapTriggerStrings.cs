// ------------------------------------------------------------------------------
// <copyright file="MapTriggerStrings.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Script
{
    public sealed partial class MapTriggerStrings : TriggerStrings
    {
        public const string FileName = "war3map.wts";

        /// <summary>
        /// Initializes a new instance of the <see cref="MapTriggerStrings"/> class.
        /// </summary>
        public MapTriggerStrings()
        {
        }

        public override string ToString() => FileName;
    }
}
// ------------------------------------------------------------------------------
// <copyright file="MapTriggerStrings.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

namespace War3Net.Build.Script
{
    public sealed partial class MapTriggerStrings : TriggerStrings
    {
        internal MapTriggerStrings(StreamReader reader)
            : base(reader)
        {
        }
    }
}
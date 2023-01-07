// ------------------------------------------------------------------------------
// <copyright file="TriggerStrings.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Script
{
    public sealed partial class TriggerStrings
    {
        public const string FileExtension = ".wts";
        public const string CampaignFileName = "war3campaign.wts";
        public const string MapFileName = "war3map.wts";

        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerStrings"/> class.
        /// </summary>
        public TriggerStrings()
        {
        }

        public List<TriggerString> Strings { get; init; } = new();

        public override string ToString() => $"{FileExtension} file";
    }
}
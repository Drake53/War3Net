// ------------------------------------------------------------------------------
// <copyright file="StreamReaderExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Build.Script;

namespace War3Net.Build.Extensions
{
    public static class StreamReaderExtensions
    {
        public static TriggerStrings ReadTriggerStrings(this StreamReader reader, bool fromCampaign) => fromCampaign ? reader.ReadCampaignTriggerStrings() : reader.ReadMapTriggerStrings();

        public static CampaignTriggerStrings ReadCampaignTriggerStrings(this StreamReader reader) => new CampaignTriggerStrings(reader);

        public static MapTriggerStrings ReadMapTriggerStrings(this StreamReader reader) => new MapTriggerStrings(reader);

        public static TriggerString ReadTriggerString(this StreamReader reader) => new TriggerString(reader);
    }
}
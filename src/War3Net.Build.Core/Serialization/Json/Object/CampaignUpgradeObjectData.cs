// ------------------------------------------------------------------------------
// <copyright file="CampaignUpgradeObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;

using War3Net.Build.Serialization.Json;

namespace War3Net.Build.Object
{
    [JsonConverter(typeof(JsonCampaignUpgradeObjectDataConverter))]
    public sealed partial class CampaignUpgradeObjectData : UpgradeObjectData
    {
        internal CampaignUpgradeObjectData(JsonElement jsonElement)
            : base(jsonElement)
        {
        }

        internal CampaignUpgradeObjectData(ref Utf8JsonReader reader)
            : base(ref reader)
        {
        }
    }
}
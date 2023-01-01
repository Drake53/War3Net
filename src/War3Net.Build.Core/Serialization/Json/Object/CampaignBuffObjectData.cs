// ------------------------------------------------------------------------------
// <copyright file="CampaignBuffObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

namespace War3Net.Build.Object
{
    public sealed partial class CampaignBuffObjectData : BuffObjectData
    {
        internal CampaignBuffObjectData(JsonElement jsonElement)
            : base(jsonElement)
        {
        }

        internal CampaignBuffObjectData(ref Utf8JsonReader reader)
            : base(ref reader)
        {
        }
    }
}
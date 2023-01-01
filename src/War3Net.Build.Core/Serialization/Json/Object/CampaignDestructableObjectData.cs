// ------------------------------------------------------------------------------
// <copyright file="CampaignDestructableObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

namespace War3Net.Build.Object
{
    public sealed partial class CampaignDestructableObjectData : DestructableObjectData
    {
        internal CampaignDestructableObjectData(JsonElement jsonElement)
            : base(jsonElement)
        {
        }

        internal CampaignDestructableObjectData(ref Utf8JsonReader reader)
            : base(ref reader)
        {
        }
    }
}
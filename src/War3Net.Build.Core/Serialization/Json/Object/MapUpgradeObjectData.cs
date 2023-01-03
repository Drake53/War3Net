// ------------------------------------------------------------------------------
// <copyright file="MapUpgradeObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;

using War3Net.Build.Serialization.Json;

namespace War3Net.Build.Object
{
    [JsonConverter(typeof(JsonMapUpgradeObjectDataConverter))]
    public sealed partial class MapUpgradeObjectData : UpgradeObjectData
    {
        internal MapUpgradeObjectData(JsonElement jsonElement)
            : base(jsonElement)
        {
        }

        internal MapUpgradeObjectData(ref Utf8JsonReader reader)
            : base(ref reader)
        {
        }
    }
}
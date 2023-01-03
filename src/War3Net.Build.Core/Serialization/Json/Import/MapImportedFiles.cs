// ------------------------------------------------------------------------------
// <copyright file="MapImportedFiles.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;

using War3Net.Build.Serialization.Json;

namespace War3Net.Build.Import
{
    [JsonConverter(typeof(JsonMapImportedFilesConverter))]
    public sealed partial class MapImportedFiles : ImportedFiles
    {
        internal MapImportedFiles(JsonElement jsonElement)
            : base(jsonElement)
        {
        }

        internal MapImportedFiles(ref Utf8JsonReader reader)
            : base(ref reader)
        {
        }
    }
}
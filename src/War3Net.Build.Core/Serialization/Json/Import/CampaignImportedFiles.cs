// ------------------------------------------------------------------------------
// <copyright file="CampaignImportedFiles.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

namespace War3Net.Build.Import
{
    public sealed partial class CampaignImportedFiles : ImportedFiles
    {
        internal CampaignImportedFiles(JsonElement jsonElement)
            : base(jsonElement)
        {
        }

        internal CampaignImportedFiles(ref Utf8JsonReader reader)
            : base(ref reader)
        {
        }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="SearchResultVersion.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Newtonsoft.Json;

namespace NuGetPackageUploader
{
    /// <summary>
    /// A single version from a <see cref="SearchResult"/>.
    ///
    /// See https://docs.microsoft.com/en-us/nuget/api/search-query-service-resource#search-result
    /// </summary>
    public class SearchResultVersion
    {
        /// <summary>
        /// The package's full NuGet version after normalization, including any SemVer 2.0.0 build metadata.
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        /// The downloads for this single version of the matched package.
        /// </summary>
        [JsonProperty("downloads")]
        public long Downloads { get; set; }
    }
}
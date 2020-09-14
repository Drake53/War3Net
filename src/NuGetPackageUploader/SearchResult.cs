// ------------------------------------------------------------------------------
// <copyright file="SearchResult.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using Newtonsoft.Json;

namespace NuGetPackageUploader
{
    /// <summary>
    /// A package that matched a search query.
    ///
    /// See https://docs.microsoft.com/en-us/nuget/api/search-query-service-resource#search-result
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// The ID of the matched package.
        /// </summary>
        [JsonProperty("id")]
        public string PackageId { get; set; }

        /// <summary>
        /// The total downloads for all versions of the matched package.
        /// </summary>
        [JsonProperty("totalDownloads")]
        public long TotalDownloads { get; set; }

        /// <summary>
        /// The versions of the matched package.
        /// </summary>
        [JsonProperty("versions")]
        public IReadOnlyList<SearchResultVersion> Versions { get; set; }
    }
}
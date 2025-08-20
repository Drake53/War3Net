// ------------------------------------------------------------------------------
// <copyright file="CascRegion.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.IO.Casc.Enums
{
    /// <summary>
    /// Represents regional server codes used in CASC/TACT systems.
    /// </summary>
    /// <remarks>
    /// <para>
    /// These region codes determine which patch servers and CDN endpoints are used
    /// when accessing game data. Different regions may have different versions,
    /// configurations, and content due to localization or regulatory requirements.
    /// </para>
    /// <para>
    /// The region code is used in URLs like: http://{region}.patch.battle.net:1119/{product}/versions
    /// and determines which CDN servers are listed in the CDN configuration.
    /// </para>
    /// </remarks>
    public static class CascRegion
    {
        /// <summary>
        /// United States / Americas region.
        /// </summary>
        /// <remarks>
        /// Primary region for North and South America. Usually includes English,
        /// Spanish (Latin America), and Portuguese (Brazil) localizations.
        /// </remarks>
        public const string US = "us";

        /// <summary>
        /// Europe region.
        /// </summary>
        /// <remarks>
        /// Covers European countries with multiple language support including
        /// English (UK), German, French, Spanish (Spain), Italian, Russian, and Polish.
        /// </remarks>
        public const string EU = "eu";

        /// <summary>
        /// Korea region.
        /// </summary>
        /// <remarks>
        /// Dedicated region for South Korea with Korean localization.
        /// Often has different content or monetization models.
        /// </remarks>
        public const string KR = "kr";

        /// <summary>
        /// Taiwan region.
        /// </summary>
        /// <remarks>
        /// Region for Taiwan with Traditional Chinese localization.
        /// </remarks>
        public const string TW = "tw";

        /// <summary>
        /// China region.
        /// </summary>
        /// <remarks>
        /// Mainland China region operated by NetEase with Simplified Chinese localization.
        /// May have significant content differences due to regulatory requirements.
        /// Note: This region often uses different infrastructure and may not be accessible
        /// from outside China.
        /// </remarks>
        public const string CN = "cn";
    }
}
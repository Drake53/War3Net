// ------------------------------------------------------------------------------
// <copyright file="Utf8JsonReaderExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Text.Json;

using War3Net.Build.Info;

namespace War3Net.Build.Extensions
{
    internal static class Utf8JsonReaderExtensions
    {
        public static MapInfo ReadMapInfo(this ref Utf8JsonReader reader) => new MapInfo(ref reader);
    }
}
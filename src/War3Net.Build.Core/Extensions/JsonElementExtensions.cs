// ------------------------------------------------------------------------------
// <copyright file="JsonElementExtensions.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;
using System.Drawing;
using System.Numerics;
using System.Text.Json;

using War3Net.Build.Common;
using War3Net.Build.Info;
using War3Net.Build.Widget;
using War3Net.Common.Extensions;

namespace War3Net.Build.Extensions
{
    internal static class JsonElementExtensions
    {
        public static Color GetColor(this JsonElement jsonElement)
        {
            return Color.FromArgb(
                jsonElement.GetInt32(nameof(Color.A)),
                jsonElement.GetInt32(nameof(Color.R)),
                jsonElement.GetInt32(nameof(Color.G)),
                jsonElement.GetInt32(nameof(Color.B)));
        }

        public static Vector2 GetVector2(this JsonElement jsonElement)
        {
            return new Vector2(
                jsonElement.GetSingle(nameof(Vector2.X)),
                jsonElement.GetSingle(nameof(Vector2.Y)));
        }

        public static Version? GetVersion(this JsonElement jsonElement)
        {
            var versionString = jsonElement.GetString();
            return versionString is null ? null : new Version(versionString);
        }

        public static Bitmask32 GetBitmask32(this JsonElement jsonElement) => new Bitmask32(jsonElement);

        public static Quadrilateral GetQuadrilateral(this JsonElement jsonElement) => new Quadrilateral(jsonElement);

        public static RandomItemSet GetRandomItemSet(this JsonElement jsonElement, MapInfoFormatVersion formatVersion) => new RandomItemSet(jsonElement, formatVersion);

        public static RandomItemSet GetRandomItemSet(this JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => new RandomItemSet(jsonElement, formatVersion, subVersion, useNewFormat);

        public static RandomItemSetItem GetRandomItemSetItem(this JsonElement jsonElement, MapInfoFormatVersion formatVersion) => new RandomItemSetItem(jsonElement, formatVersion);

        public static RandomItemSetItem GetRandomItemSetItem(this JsonElement jsonElement, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat) => new RandomItemSetItem(jsonElement, formatVersion, subVersion, useNewFormat);

        public static RectangleMargins GetRectangleMargins(this JsonElement jsonElement) => new RectangleMargins(jsonElement);

        public static MapInfo GetMapInfo(this JsonElement jsonElement) => new MapInfo(jsonElement);

        public static PlayerData GetPlayerData(this JsonElement jsonElement, MapInfoFormatVersion formatVersion) => new PlayerData(jsonElement, formatVersion);

        public static ForceData GetForceData(this JsonElement jsonElement, MapInfoFormatVersion formatVersion) => new ForceData(jsonElement, formatVersion);

        public static UpgradeData GetUpgradeData(this JsonElement jsonElement, MapInfoFormatVersion formatVersion) => new UpgradeData(jsonElement, formatVersion);

        public static TechData GetTechData(this JsonElement jsonElement, MapInfoFormatVersion formatVersion) => new TechData(jsonElement, formatVersion);

        public static RandomUnitTable GetRandomUnitTable(this JsonElement jsonElement, MapInfoFormatVersion formatVersion) => new RandomUnitTable(jsonElement, formatVersion);

        public static RandomItemTable GetRandomItemTable(this JsonElement jsonElement, MapInfoFormatVersion formatVersion) => new RandomItemTable(jsonElement, formatVersion);

        public static RandomUnitSet GetRandomUnitSet(this JsonElement jsonElement, MapInfoFormatVersion formatVersion, int setSize) => new RandomUnitSet(jsonElement, formatVersion, setSize);

        public static Color GetColor(this JsonElement jsonElement, ReadOnlySpan<char> propertyName) => jsonElement.GetProperty(propertyName).GetColor();

        public static Vector2 GetVector2(this JsonElement jsonElement, ReadOnlySpan<char> propertyName) => jsonElement.GetProperty(propertyName).GetVector2();

        public static Version? GetVersion(this JsonElement jsonElement, ReadOnlySpan<char> propertyName) => jsonElement.GetProperty(propertyName).GetVersion();

        public static Bitmask32 GetBitmask32(this JsonElement jsonElement, ReadOnlySpan<char> propertyName) => jsonElement.GetProperty(propertyName).GetBitmask32();

        public static Quadrilateral GetQuadrilateral(this JsonElement jsonElement, ReadOnlySpan<char> propertyName) => jsonElement.GetProperty(propertyName).GetQuadrilateral();

        public static RectangleMargins GetRectangleMargins(this JsonElement jsonElement, ReadOnlySpan<char> propertyName) => jsonElement.GetProperty(propertyName).GetRectangleMargins();
    }
}
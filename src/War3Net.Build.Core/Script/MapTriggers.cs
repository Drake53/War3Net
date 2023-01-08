// ------------------------------------------------------------------------------
// <copyright file="MapTriggers.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.Common.Extensions;

namespace War3Net.Build.Script
{
    // https://github.com/stijnherfst/HiveWE/wiki/war3map.wtg-Triggers
    // http://www.wc3c.net/tools/specs/index.html
    public sealed partial class MapTriggers
    {
        public const string FileExtension = ".wtg";
        public const string FileName = "war3map.wtg";

        public static readonly int FileFormatSignature = "WTG!".FromRawcode();

        /// <summary>
        /// Initializes a new instance of the <see cref="MapTriggers"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        /// <param name="subVersion"></param>
        public MapTriggers(MapTriggersFormatVersion formatVersion, MapTriggersSubVersion? subVersion)
        {
            FormatVersion = formatVersion;
            SubVersion = subVersion;
        }

        public MapTriggersFormatVersion FormatVersion { get; set; }

        public MapTriggersSubVersion? SubVersion { get; set; }

        public int GameVersion { get; set; }

        public List<VariableDefinition> Variables { get; init; } = new();

        public List<TriggerItem> TriggerItems { get; init; } = new();

        public Dictionary<TriggerItemType, int> TriggerItemCounts { get; init; } = new();

        public override string ToString() => FileName;
    }
}
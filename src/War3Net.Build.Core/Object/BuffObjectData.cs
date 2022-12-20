// ------------------------------------------------------------------------------
// <copyright file="BuffObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Object
{
    public abstract partial class BuffObjectData
    {
        internal BuffObjectData(ObjectDataFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        public ObjectDataFormatVersion FormatVersion { get; set; }

        public List<SimpleObjectModification> BaseBuffs { get; init; } = new();

        public List<SimpleObjectModification> NewBuffs { get; init; } = new();
    }
}
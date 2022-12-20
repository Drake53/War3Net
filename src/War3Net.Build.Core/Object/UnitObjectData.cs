// ------------------------------------------------------------------------------
// <copyright file="UnitObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Object
{
    public abstract partial class UnitObjectData
    {
        internal UnitObjectData(ObjectDataFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        public ObjectDataFormatVersion FormatVersion { get; set; }

        public List<SimpleObjectModification> BaseUnits { get; init; } = new();

        public List<SimpleObjectModification> NewUnits { get; init; } = new();
    }
}
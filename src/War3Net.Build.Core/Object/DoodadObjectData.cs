// ------------------------------------------------------------------------------
// <copyright file="DoodadObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

namespace War3Net.Build.Object
{
    public abstract partial class DoodadObjectData
    {
        internal DoodadObjectData(ObjectDataFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        public ObjectDataFormatVersion FormatVersion { get; set; }

        public List<VariationObjectModification> BaseDoodads { get; init; } = new();

        public List<VariationObjectModification> NewDoodads { get; init; } = new();
    }
}
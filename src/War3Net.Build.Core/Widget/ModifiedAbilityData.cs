// ------------------------------------------------------------------------------
// <copyright file="ModifiedAbilityData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.IO;

using War3Net.Common.Extensions;

namespace War3Net.Build.Widget
{
    public sealed class ModifiedAbilityData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModifiedAbilityData"/> class.
        /// </summary>
        public ModifiedAbilityData()
        {
        }

        internal ModifiedAbilityData(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            ReadFrom(reader, formatVersion, subVersion, useNewFormat);
        }

        public int AbilityId { get; set; }

        public bool IsAutocastActive { get; set; }

        public int HeroAbilityLevel { get; set; }

        internal void ReadFrom(BinaryReader reader, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            AbilityId = reader.ReadInt32();
            IsAutocastActive = reader.ReadBool();
            HeroAbilityLevel = reader.ReadInt32();
        }

        internal void WriteTo(BinaryWriter writer, MapWidgetsFormatVersion formatVersion, MapWidgetsSubVersion subVersion, bool useNewFormat)
        {
            writer.Write(AbilityId);
            writer.WriteBool(IsAutocastActive);
            writer.Write(HeroAbilityLevel);
        }
    }
}
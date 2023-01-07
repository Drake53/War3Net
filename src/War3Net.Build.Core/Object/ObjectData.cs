// ------------------------------------------------------------------------------
// <copyright file="ObjectData.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Object
{
    public sealed partial class ObjectData
    {
        public const string FileExtension = ".w3o";

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectData"/> class.
        /// </summary>
        /// <param name="formatVersion"></param>
        public ObjectData(ObjectDataFormatVersion formatVersion)
        {
            FormatVersion = formatVersion;
        }

        public ObjectDataFormatVersion FormatVersion { get; set; }

        public UnitObjectData? UnitData { get; set; }

        public ItemObjectData? ItemData { get; set; }

        public DestructableObjectData? DestructableData { get; set; }

        public DoodadObjectData? DoodadData { get; set; }

        public AbilityObjectData? AbilityData { get; set; }

        public BuffObjectData? BuffData { get; set; }

        public UpgradeObjectData? UpgradeData { get; set; }

        public override string ToString() => $"{FileExtension} file";
    }
}
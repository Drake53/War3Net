// ------------------------------------------------------------------------------
// <copyright file="VariationObjectDataModification.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Build.Object
{
    public sealed partial class VariationObjectDataModification : ObjectDataModification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VariationObjectDataModification"/> class.
        /// </summary>
        public VariationObjectDataModification()
        {
        }

        public int Variation { get; set; }

        public int Pointer { get; set; }
    }
}
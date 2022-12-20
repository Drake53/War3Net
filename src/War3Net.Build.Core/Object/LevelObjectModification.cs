// ------------------------------------------------------------------------------
// <copyright file="LevelObjectModification.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System.Collections.Generic;

using War3Net.Common.Extensions;

namespace War3Net.Build.Object
{
    public sealed partial class LevelObjectModification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LevelObjectModification"/> class.
        /// </summary>
        public LevelObjectModification()
        {
        }

        public int OldId { get; set; }

        public int NewId { get; set; }

        public List<int> Unk { get; init; } = new();

        public List<LevelObjectDataModification> Modifications { get; init; } = new();

        public override string ToString() => NewId == 0 ? OldId.ToRawcode() : $"{NewId.ToRawcode()}:{OldId.ToRawcode()}";
    }
}
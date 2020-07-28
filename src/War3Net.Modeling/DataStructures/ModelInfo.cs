// ------------------------------------------------------------------------------
// <copyright file="ModelInfo.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Modeling.DataStructures
{
    public struct ModelInfo
    {
        public string Name { get; set; }

        public string AnimationFileName { get; set; }

        public float BlendTime { get; set; }

        public Extent Extent { get; set; }
    }
}
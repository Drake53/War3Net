// ------------------------------------------------------------------------------
// <copyright file="InstanceModelResources.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using Veldrid;

namespace War3Net.Rendering.DataStructures
{
    /// <summary>
    /// Graphics resources for a single model instance.
    /// </summary>
    public sealed class InstanceModelResources
    {
        public DeviceBuffer[] NodeBuffers { get; set; }

        public DeviceBuffer TransformationBuffer { get; set; }

        public Pipeline[][] Pipelines { get; set; }

        public ResourceSet[][] ResourceSets { get; set; }
    }
}
// ------------------------------------------------------------------------------
// <copyright file="CachedModelResources.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

namespace War3Net.Rendering.DataStructures
{
    public sealed class CachedModelResources
    {
        public LoadedModelResources LoadedResources { get; set; }

        public SharedModelResources SharedResources { get; set; }
    }
}
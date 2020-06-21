// ------------------------------------------------------------------------------
// <copyright file="LayerShading.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Modeling.Enums
{
    [Flags]
    public enum LayerShading
    {
        Unshaded = 1,
        SphereEnvMap = 2,
        TwoSided = 16,
        Unfogged = 32,
        NoDepthTest = 64,
        NoDepthSet = 128,
    }
}
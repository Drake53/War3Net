// ------------------------------------------------------------------------------
// <copyright file="NodeFlags.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

using System;

namespace War3Net.Modeling.Enums
{
    [Flags]
    public enum NodeFlags
    {
        DontInheritTranslation = 1 << 0,
        DontInheritRotation = 1 << 1,
        DontInheritScaling = 1 << 2,
        Billboarded = 1 << 3,
        BollboardedLockX = 1 << 4,
        BollboardedLockY = 1 << 5,
        BollboardedLockZ = 1 << 6,
        CameraAnchored = 1 << 7,
    }
}
// ------------------------------------------------------------------------------
// <copyright file="TargetFlagApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Enums;

namespace War3Net.Runtime.Api.Common.Enums
{
    public static class TargetFlagApi
    {
        public static readonly TargetFlag TARGET_FLAG_NONE = ConvertTargetFlag((int)TargetFlag.Type.None);
        public static readonly TargetFlag TARGET_FLAG_GROUND = ConvertTargetFlag((int)TargetFlag.Type.Ground);
        public static readonly TargetFlag TARGET_FLAG_AIR = ConvertTargetFlag((int)TargetFlag.Type.Air);
        public static readonly TargetFlag TARGET_FLAG_STRUCTURE = ConvertTargetFlag((int)TargetFlag.Type.Structure);
        public static readonly TargetFlag TARGET_FLAG_WARD = ConvertTargetFlag((int)TargetFlag.Type.Ward);
        public static readonly TargetFlag TARGET_FLAG_ITEM = ConvertTargetFlag((int)TargetFlag.Type.Item);
        public static readonly TargetFlag TARGET_FLAG_TREE = ConvertTargetFlag((int)TargetFlag.Type.Tree);
        public static readonly TargetFlag TARGET_FLAG_WALL = ConvertTargetFlag((int)TargetFlag.Type.Wall);
        public static readonly TargetFlag TARGET_FLAG_DEBRIS = ConvertTargetFlag((int)TargetFlag.Type.Debris);
        public static readonly TargetFlag TARGET_FLAG_DECORATION = ConvertTargetFlag((int)TargetFlag.Type.Decoration);
        public static readonly TargetFlag TARGET_FLAG_BRIDGE = ConvertTargetFlag((int)TargetFlag.Type.Bridge);

        public static TargetFlag ConvertTargetFlag(int i)
        {
            return TargetFlag.GetTargetFlag(i);
        }
    }
}
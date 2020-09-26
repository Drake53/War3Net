// ------------------------------------------------------------------------------
// <copyright file="MoveTypeApi.cs" company="Drake53">
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
    public static class MoveTypeApi
    {
        public static readonly MoveType MOVE_TYPE_UNKNOWN = ConvertMoveType((int)MoveType.Type.Unknown);
        public static readonly MoveType MOVE_TYPE_FOOT = ConvertMoveType((int)MoveType.Type.Foot);
        public static readonly MoveType MOVE_TYPE_FLY = ConvertMoveType((int)MoveType.Type.Fly);
        public static readonly MoveType MOVE_TYPE_HORSE = ConvertMoveType((int)MoveType.Type.Horse);
        public static readonly MoveType MOVE_TYPE_HOVER = ConvertMoveType((int)MoveType.Type.Hover);
        public static readonly MoveType MOVE_TYPE_FLOAT = ConvertMoveType((int)MoveType.Type.Float);
        public static readonly MoveType MOVE_TYPE_AMPHIBIOUS = ConvertMoveType((int)MoveType.Type.Amphibious);
        public static readonly MoveType MOVE_TYPE_UNBUILDABLE = ConvertMoveType((int)MoveType.Type.Unbuildable);

        public static MoveType ConvertMoveType(int i)
        {
            return MoveType.GetMoveType(i);
        }
    }
}
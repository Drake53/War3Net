// ------------------------------------------------------------------------------
// <copyright file="LimitOpApi.cs" company="Drake53">
// Licensed under the MIT license.
// See the LICENSE file in the project root for more information.
// </copyright>
// ------------------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA2211 // Non-constant fields should not be visible
#pragma warning disable SA1310 // Field names should not contain underscore
#pragma warning disable SA1401 // Fields should be private

using War3Net.Runtime.Common.Enums.Event;

namespace War3Net.Runtime.Common.Api.Enums.Event
{
    public static class LimitOpApi
    {
        public static readonly LimitOp LESS_THAN = ConvertLimitOp((int)LimitOp.Type.LessThan);
        public static readonly LimitOp LESS_THAN_OR_EQUAL = ConvertLimitOp((int)LimitOp.Type.LessThanOrEqual);
        public static readonly LimitOp EQUAL = ConvertLimitOp((int)LimitOp.Type.Equal);
        public static readonly LimitOp GREATER_THAN_OR_EQUAL = ConvertLimitOp((int)LimitOp.Type.GreaterThanOrEqual);
        public static readonly LimitOp GREATER_THAN = ConvertLimitOp((int)LimitOp.Type.GreaterThan);
        public static readonly LimitOp NOT_EQUAL = ConvertLimitOp((int)LimitOp.Type.NotEqual);

        public static LimitOp ConvertLimitOp(int i)
        {
            return LimitOp.GetLimitOp(i);
        }
    }
}